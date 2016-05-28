using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using L.S.Model.DatabaseModel.Context;
using L.S.Model.DatabaseModel.Entity;
using L.S.Interface;
using L.S.Home.Models;
using L.Study.Common;
using Autofac;
using L.S.Interface.BLL;
using L.S.Common;
using L.S.Model.DomainModel.Sys;

namespace L.S.Home.Areas.admin.Controllers
{
    [LSAuthorize]
    public class SysRoleController : LsBaseController
    {
        private IRoleService roleService;
        private IRoleBLL roleBll;

        public SysRoleController(IRoleService _roleService,IRoleBLL _roleBLL)
        {
            roleService = _roleService;
            //roleBll = IocConfig.Container.Resolve<RoleBLL>();
            roleBll = _roleBLL;
        }

        [LSAuthorize("RolesManage", "SysManage", "RolesManage")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var result = roleService.GetPagedList(r => true, page, pageSize, modellist => modellist.OrderByDescending(d => d.AddDate));
            return View(result);
        }

        [LSAuthorize("RolesManage", "SysManage", "RolesManage")]
        public ActionResult TreeIndex(int page = 1, int pageSize = 10)
        {
            return View();
        }

        [LSAuthorize("DepsManage", "SysManage", "DepsManage")]
        public ActionResult GetSysRole(string id)
        {
            var model = roleService.GetList(r => r.ID == id).Select(r => new SysRoleViewModel
            {
                ID = r.ID,
                Name = r.Name,
                ParentID = r.ParentID,
                ParentName = r.Parent == null ? "" : r.Parent.Name,
                IsAvailable = r.IsAvailable,
                AddBy = r.AddBy,
                AddDate = r.AddDate,
                DefaultHomePath=r.DefaultHomePath,
                SysRightsID = r.SysRoleRights.Select(rr=>rr.SysRight.ID).ToList(),
                SysRightsName = r.SysRoleRights.Select(rr => rr.SysRight.Name).ToList(),
                SortNo=r.SortNo,
            }).FirstOrDefault();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [LSAuthorize("RoleCreate", "SysManage", "RolesManage")]
        public ActionResult Create()
        {
            return View(new SysRole());
        }

        // POST: admin/sysrole/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [LSAuthorize("RoleCreate", "SysManage", "RolesManage")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IsAvailable,Name,ParentID,ParentName,DefaultHomePath,SortNo")] SysRole sysRole,string SysRightsID)
        {
            sysRole.ID = IdentityCreator.NextIdentity;
            sysRole.AddBy = cuser.UserID;
            sysRole.AddByName = cuser.LoginName;
            sysRole.AddDate = DateTime.Now;
            sysRole.IsDel = false;
            if (string.IsNullOrEmpty(Request["SortNo"])) { sysRole.SortNo = 0; }

            var parentRole = roleService.Find(sysRole.ParentID);
            if (parentRole != null)
            {
                if (sysRole.ID != "superadmin")
                {
                    var parentRolesRightIDList = parentRole.SysRoleRights.Select(srr => srr.RightID).Distinct().ToList();
                    var roleRightIDList = SysRightsID.Split(',');
                    if (roleRightIDList.Any(rid => !parentRolesRightIDList.Contains(rid)))//如果所选权限里有一个是上级角色所没有的，就表示所选的上级角色权限太小了，将不允许保存
                    {
                        return Json(new AjaxResult() { success = false, msg = insertFailure + "，所选上级角色的权限小于为此角色所选择的权限", moremsg = msg });
                    }
                    else
                    {
                        sysRole.ParentName = parentRole.Name;
                        sysRole.Level = parentRole.Level + 1;
                        sysRole.RoleIDPath = parentRole.RoleIDPath  + sysRole.ID + "/";
                        sysRole.RoleNamePath = parentRole.RoleNamePath  + sysRole.Name + "/";
                    }
                }
                roleService.Add(sysRole);
                if (roleService.SaveChanges(out msg) > 0)//这里不提交，把提交放到下面的SetRoleRights方法（方法里有调用SaveChange）里看是否可行，事实证明是可行的，然并卵！
                {
                    if (roleBll.SetRoleRights(sysRole.ID, SysRightsID, out msg))
                    {
                        return Json(new AjaxResult() { success = true, msg = insertSuccess, url = Url.Action("treeindex", "sysrole", new { area = "admin" }), moremsg = msg });
                    }
                    else
                    {
                        roleService.Remove(sysRole);
                        roleService.SaveChanges(out msg);//妈的，要是SetRoleRights执行失败了，这里还要手动删除一下刚才插入的SysRole记录。你问我为什么不能放在一个事务里插入SysRole和此角色的权限？因为角色权限有一个字段作为外键关联的SysRole的ID！！
                        return Json(new AjaxResult() { success = false, msg = insertFailure, moremsg = msg });
                    }
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = insertFailure, moremsg = msg });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = "未找到上级角色，保存失败", moremsg = msg });
            }
        }

        [LSAuthorize("RoleEdit", "SysManage", "RolesManage")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysRole sysRole = roleService.Find(id);
            if (sysRole == null)
            {
                return View("_NoDataInLayout");
            }
            SysRole parent = sysRole.Parent;
            if (parent != null)
            {
                ViewBag.ParentRoleNameStr = parent.Name;
                ViewBag.ParentRoleIDStr = parent.ID;
            }
            var rightList = sysRole.SysRoleRights.Select(a => new { a.SysRight.Name ,a.SysRight.ID}).ToList();
            var rightIDStr=string.Join(",",rightList.Select(r => r.ID).ToArray());
            var rightNameStr = string.Join(",", rightList.Select(r => r.Name).ToArray());
            ViewBag.rightIDStr = rightIDStr;
            ViewBag.rightNameStr = rightNameStr;
            return View(sysRole);
        }

        [LSAuthorize("RoleEdit", "SysManage", "RolesManage")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AddBy,AddByName,AddDate,IsAvailable,Name,ParentID,ParentName,DefaultHomePath,SortNo")] SysRole sysRole,string SysRightsID)
        {
            var tmpList= roleService.GetQueryable(role=>role.ID==sysRole.ID).SelectMany(role=>role.SysRoleRights.Select(roleright=>roleright.RightID)).ToList();
            bool rightIncrease = !tmpList.Any(tr => !SysRightsID.Contains(tr));//数据库中的此角色的权限，无任何一个未选，则表示权限未降低
            
            bool allowedModify = rightIncrease || sysRole.ID != "superadmin";
            if (allowedModify)
            {
                #region 除系统内置的超级管理员，其他角色可以修改
                sysRole.UpdateBy = cuser.UserID;
                sysRole.UpdateByName = cuser.LoginName;
                sysRole.UpdateDate = DateTime.Now;
                sysRole.IsDel = false;
                if (string.IsNullOrEmpty(Request["SortNo"])) { sysRole.SortNo = 0; }
                var parentRole = roleService.Find(sysRole.ParentID);
                if (parentRole != null || sysRole.ID == "superadmin")
                {
                    if (sysRole.ID != "superadmin")
                    {
                        var parentRolesRightIDList = parentRole.SysRoleRights.Select(srr => srr.RightID).Distinct().ToList();
                        var roleRightIDList = SysRightsID.Split(',');
                        if (roleRightIDList.Any(rid => !parentRolesRightIDList.Contains(rid)))//如果所选权限里有一个是上级角色所没有的，就表示所选的上级角色权限太小了，将不允许保存
                        {
                            return Json(new AjaxResult() { success = false, msg = updateFailure + "，所选上级角色的权限小于为此角色所选择的权限", moremsg = msg });
                        }
                        else
                        {
                            sysRole.ParentName = parentRole.Name;
                            sysRole.Level = parentRole.Level + 1;
                            sysRole.RoleIDPath = parentRole.RoleIDPath  + sysRole.ID + "/";
                            sysRole.RoleNamePath = parentRole.RoleNamePath  + sysRole.Name + "/";
                        }
                    }
                    else
                    {
                        sysRole.RoleIDPath = "/" + sysRole.ID + "/";
                        sysRole.RoleNamePath = "/" + sysRole.Name + "/";
                        sysRole.Level = 0;
                    }
                    roleService.Update(sysRole);
                    if (roleService.SaveChanges(out msg) > 0)
                    {
                        if (roleBll.SetRoleRights(sysRole.ID, SysRightsID, out msg))
                        {
                            return Json(new AjaxResult() { success = true, msg = updateSuccess, url = Url.Action("treeindex", "sysrole", new { area = "admin" }), moremsg = msg });
                        }
                        else
                        {
                            return Json(new AjaxResult() { success = false, msg = updateFailure, moremsg = msg });
                        }
                    }
                    else
                    {
                        return Json(new AjaxResult() { success = false, msg = updateFailure, moremsg = msg });
                    }
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = "未找到上级角色，保存失败", moremsg = msg });
                }
                #endregion
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = "系统内置超级管理员不允许降低权限" });
            }
        }

        [LSAuthorize("RoleDelete", "SysManage", "RolesManage")]
        public ActionResult Delete(string ids)
        {          
            if (!string.IsNullOrEmpty(ids))
            {
                if (roleService.RoleDelete(ids, out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = deleteSuccess, url = Url.Action("treeindex") });
                }
                else
                {
                    msg = errorOutPutToPage ? msg : deleteFailure;
                    return Json(new AjaxResult() { success = false, msg = msg });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = didnotchoosedata });
            }
        }
        [LSAuthorize("RoleAvailable", "SysManage", "RolesManage")]
        public ActionResult Available(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update SysRole set isavailable=1 where id in (" + sqlids + ")";
                if (roleService.ExecuteSql(sql, out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = AvailableSuccess, url = Url.Action("treeindex", "sysrole", new { area = "admin" }) });
                }
                else
                {
                    msg = errorOutPutToPage ? msg : AvailableFailure;
                    return Json(new AjaxResult() { success = false, msg = msg });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = didnotchoosedata });
            }
        }
        [LSAuthorize("RoleUnAvailable", "SysManage", "RolesManage")]
        public ActionResult UnAvailable(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update SysRole set isavailable=0 where id in (" + sqlids + ")";
                if (roleService.ExecuteSql(sql, out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = UnAvailableSuccess, url = Url.Action("treeindex", "sysrole",new { area="admin"}) });
                }
                else
                {
                    msg = errorOutPutToPage ? msg : UnAvailableFailure;
                    return Json(new AjaxResult() { success = false, msg = msg });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = didnotchoosedata });
            }
        }
    }
}
