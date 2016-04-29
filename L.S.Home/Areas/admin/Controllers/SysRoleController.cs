﻿using System;
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
using L.S.Home.BLL;
using Autofac;

namespace L.S.Home.Areas.admin.Controllers
{
    [LSAuthorize]
    public class SysRoleController : LsBaseController
    {
        private IRoleService roleService;
        private RoleBLL roleBll;

        public SysRoleController(IRoleService _roleService)
        {
            roleService = _roleService;
            roleBll = IocConfig.Container.Resolve<RoleBLL>();
        }

        [LSAuthorize("RolesManage", "SysManage", "RolesManage")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var result = roleService.GetPagedList(r => true, page, pageSize, modellist => modellist.OrderByDescending(d => d.AddDate));
            return View(result);
        }


        [LSAuthorize("RoleCreate", "SysManage", "RolesManage")]
        public ActionResult Create()
        {
            //var RoleList = GetRoleListAsEnumerable(roleService, cuser.RolesID);
            //ViewBag.RoleList = RoleList;
            return View(new SysRole());
        }

        // POST: admin/sysrole/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [LSAuthorize("RoleCreate", "SysManage", "RolesManage")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IsAvailable,Name,ParentID,ParentName,DefaultHomePath")] SysRole sysRole,string SysRightsID)
        {
            sysRole.ID = IdentityCreator.NextIdentity;
            sysRole.AddBy = "before login";
            sysRole.AddByName = "before login";
            sysRole.AddDate = DateTime.Now;
            sysRole.IsDel = false;
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
                        sysRole.RoleIDPath = parentRole.RoleIDPath + "/" + sysRole.ID;
                        sysRole.RoleNamePath = parentRole.RoleNamePath + "/" + sysRole.Name;
                    }
                }
                roleService.Add(sysRole);
                if (roleService.SaveChanges(out msg) > 0)
                {
                    if (roleBll.SetRoleRights(sysRole.ID, SysRightsID, out msg))
                    {
                        return Json(new AjaxResult() { success = true, msg = updateSuccess, url = Url.Action("index", "sysrole", new { area = "admin" }), moremsg = msg });
                    }
                    else
                    {
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
        public ActionResult Edit([Bind(Include = "ID,AddBy,AddByName,AddDate,IsAvailable,Name,ParentID,ParentName,DefaultHomePath")] SysRole sysRole,string SysRightsID)
        {
            sysRole.UpdateBy = "before login";
            sysRole.UpdateByName = "before login";
            sysRole.UpdateDate = DateTime.Now;
            sysRole.IsDel = false;
            if (ModelState.IsValid)
            {
                var parentRole = roleService.Find(sysRole.ParentID);
                if (parentRole != null|| sysRole.ID== "superadmin")
                {
                    if (sysRole.ID != "superadmin")
                    {
                        var parentRolesRightIDList = parentRole.SysRoleRights.Select(srr => srr.RightID).Distinct().ToList();
                        var roleRightIDList = SysRightsID.Split(',');
                        if(roleRightIDList.Any(rid => !parentRolesRightIDList.Contains(rid)))//如果所选权限里有一个是上级角色所没有的，就表示所选的上级角色权限太小了，将不允许保存
                        {
                            return Json(new AjaxResult() { success = false, msg = updateFailure + "，所选上级角色的权限小于为此角色所选择的权限", moremsg = msg });
                        }
                        else
                        {
                            sysRole.ParentName = parentRole.Name;
                            sysRole.Level = parentRole.Level + 1;
                            sysRole.RoleIDPath = parentRole.RoleIDPath + "/" + sysRole.ID;
                            sysRole.RoleNamePath = parentRole.RoleNamePath + "/" + sysRole.Name;
                        }
                    }
                    else
                    {                        
                        sysRole.RoleIDPath =  sysRole.ID;
                        sysRole.RoleNamePath = sysRole.Name;
                        sysRole.Level = 0;
                    }
                    roleService.Update(sysRole);
                    if (roleService.SaveChanges(out msg) > 0)
                    {
                        if (roleBll.SetRoleRights(sysRole.ID, SysRightsID, out msg))
                        {
                            return Json(new AjaxResult() { success = true, msg = updateSuccess, url = Url.Action("index", "sysrole", new { area = "admin" }), moremsg = msg });
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
            }
            return Json(new AjaxResult() { success = false, msg = updateFailure, moremsg = "数据验证失败" });
        }

        [LSAuthorize("RoleDelete", "SysManage", "RolesManage")]
        public ActionResult Delete(string ids)
        {          
            if (!string.IsNullOrEmpty(ids))
            {
                if (roleService.RoleDelete(ids, out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = deleteSuccess, url = Url.Action("Index") });
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
                    return Json(new AjaxResult() { success = true, msg = AvailableSuccess, url = Url.Action("Index", "sysrole", new { area = "admin" }) });
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
                    return Json(new AjaxResult() { success = true, msg = UnAvailableSuccess, url = Url.Action("Index","sysrole",new { area="admin"}) });
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                roleService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}