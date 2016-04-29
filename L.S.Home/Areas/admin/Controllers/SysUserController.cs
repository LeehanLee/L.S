﻿using Autofac;
using L.S.Home.BLL;
using L.S.Home.Models;
using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using L.Study.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Areas.admin.Controllers
{
    
    public class SysUserController : LsBaseController
    {
        public IUserService userService;
        public IDepService depService;
        private RoleBLL roleBll;
        public SysUserController(IUserService _userService, IDepService _depService)
        {
            userService = _userService;
            depService = _depService;
            roleBll = IocConfig.Container.Resolve<RoleBLL>();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userService.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 用户管理
        [LSAuthorize("UsersManage", "SysManage", "UsersManage")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            if (page <= 0) { page = 1; }
            var list = userService.GetPagedList(su => true, page, pageSize, modellist => modellist.OrderByDescending(su => su.ID));
            return View(list);
        }

        [LSAuthorize("UserCreate", "SysManage", "UsersManage")]
        public ActionResult Create()
        {
            return View();
        }

        [LSAuthorize("UserCreate", "SysManage", "UsersManage")]
        [HttpPost]
        public ActionResult Create(SysUser model, string SysRolesID)
        {
            SysUser u = model;
            u.ID = IdentityCreator.NextIdentity;
            u.AddBy = "before login";
            u.AddDate = DateTime.Now;
            if (!userService.Exist(user => user.LoginName == model.LoginName))
            {
                var dep = depService.Find(u.SysDepID);
                if (dep != null)
                {
                    u.SysDepName = dep.Name;
                    userService.Add(u);
                    if (userService.SaveChanges(out msg) > 0)
                    {
                        if (roleBll.SetUserRoles(model.ID, SysRolesID, out msg))
                        {
                            return Json(new AjaxResult() { success = true, msg = insertSuccess, url = Url.Action("Index"), moremsg = msg });
                        }
                        else
                        {
                            return Json(new AjaxResult() { success = false, msg = msg });
                        }
                    }
                    else
                    {
                        msg = errorOutPutToPage ? msg : insertFailure;
                        return Json(new AjaxResult() { success = false, msg = msg });
                    }
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = "未找到对应的部门" });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = "登录名已存在" });
            }
        }

        [LSAuthorize("UserEdit", "SysManage", "UsersManage")]
        public ActionResult Edit(string id)
        {            
            if (!string.IsNullOrEmpty(id))
            {
                SysUser user = userService.Find(u => u.ID == id);
                var userRolesInfo=user.SysUserRoles.Select(ur => new { ur.SysRole.Name, ur.SysRole.ID ,RightLists=ur.SysRole.SysRoleRights.Select(rr=>rr.SysRight.Name)}).ToList();
                ViewBag.roleNameStr = string.Join(",", userRolesInfo.Select(r=>r.Name).ToArray());
                ViewBag.roleIDStr = string.Join(",", userRolesInfo.Select(r => r.ID).ToArray());
                ViewBag.UserRights = string.Join(",", userRolesInfo.SelectMany(r => r.RightLists).Distinct().ToArray());
                return View(user);
            }
            else
            {
                return View("_NoDataInLayout");
            }
        }
        [LSAuthorize("UserEdit", "SysManage", "UsersManage")]
        [HttpPost]
        public ActionResult Edit(SysUser model,string SysRolesID)
        {
            if (!string.IsNullOrEmpty(model.ID))
            {
                var dep = depService.Find(model.SysDepID);
                if (dep != null)
                {
                    model.SysDepName = dep.Name;
                    userService.Update(model);
                    var result = userService.SaveChanges(out msg);
                    if (result > 0)
                    {
                        if (roleBll.SetUserRoles(model.ID, SysRolesID, out msg))
                        {
                            return Json(new AjaxResult() { success = true, msg = updateSuccess, url = Url.Action("Index") });
                        }
                        else
                        {
                            return Json(new AjaxResult() { success = false, msg = msg });
                        }
                    }
                    else
                    {
                        msg = errorOutPutToPage ? msg : updateFailure;
                        return Json(new AjaxResult() { success = false, msg = msg });
                    }
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = "未找到对应的部门" });
                }
            }

            else
            {
                return View("_NoDataInLayout");
            }
            
        }

        [LSAuthorize("UserDelete", "SysManage", "UsersManage")]
        public ActionResult Delete(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                if (userService.UsersDelete(ids, out msg) > 0)
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

        [LSAuthorize("UserAvailable", "SysManage", "UsersManage")]
        public ActionResult Available(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update SysUser set isavailable=1 where id in (" + sqlids + ")";
                if (userService.ExecuteSql(sql, out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = AvailableSuccess, url = Url.Action("index") });
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
        [LSAuthorize("UserUnAvailable", "SysManage", "UsersManage")]
        public ActionResult UnAvailable(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update SysUser set isavailable=0 where id in (" + sqlids + ")";
                if (userService.ExecuteSql(sql, out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = UnAvailableSuccess, url = Url.Action("index") });
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
        #endregion
    }
}