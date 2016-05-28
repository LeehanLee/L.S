using jsTree3.Models;
using L.S.Home.Models;
using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using L.S.Model.DomainModel.Sys;
using L.S.Service;
using L.Study.Common;
using L.Study.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Areas.admin.Controllers
{
    [LSAuthorize]
    public class SysRightController : LsBaseController
    {
        public IRightService rightService;
        public SysRightController(IRightService _rightService)
        {

            rightService = _rightService;
        }

        [LSAuthorize("RightsManage", "SysManage", "RightsManage")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = rightService.GetPagedList(r => !r.IsDel, page, pageSize, modellist => modellist.OrderBy(r=>r.SortNo).ThenByDescending(r => r.UpdateDate).ThenByDescending(r => r.AddDate));
            return View(model);
        }
        [LSAuthorize("RightsManage", "SysManage", "RightsManage")]
        public ActionResult TreeIndex(int page = 1, int pageSize = 10)
        {
            ViewBag.RightPositionList = GetRightPositionItem();
            ViewBag.RightActionTypeItem = GetRightActionTypeItem();
            return View();
        }
        [LSAuthorize("RightsManage", "SysManage", "RightsManage")]
        public ActionResult GetSysRight(string id)
        {
            var model = 
                /*CacheMaker.RedisCache.GetOrSetThenGet("SysRightTreeNodeData-" + id, () =>
                {
                    return*/ rightService.GetList(r => r.ID == id).Select(r => new SysRightViewModel
                    {
                        ID = r.ID,
                        Name = r.Name,
                        ParentID = r.ParentID,
                        ParentName = r.Parent == null ? "" : r.Parent.Name,
                        MenuUrl = r.MenuUrl,
                        IsAvailable = r.IsAvailable,
                        AddBy=r.AddBy,
                        AddDate=r.AddDate,
                        ActionType=r.ActionType,
                        Position=r.Position,
                        SortNo = r.SortNo,
                        DisplayName=r.DisplayName,                        
                    }).FirstOrDefault();
                //},5);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [LSAuthorize("RightCreate", "SysManage", "RightsManage")]
        public ActionResult Create()
        {
            return View();
        }

        [LSAuthorize("RightCreate", "SysManage", "RightsManage")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IsAvailable,Name,ParentID,ActionType,MenuUrl,Position,SortNo,DisplayName")] SysRight sysRight)
        {            
            sysRight.AddBy = cuser.UserID;
            sysRight.AddByName = cuser.LoginName;
            sysRight.AddDate = DateTime.Now;
            sysRight.IsDel = false;
            sysRight.ParentID = string.IsNullOrEmpty(sysRight.ParentID) ? null : sysRight.ParentID;
            var parent = rightService.Find(sysRight.ParentID);
            if ((sysRight.ParentID==null) || parent != null)
            {
                sysRight.RightLevel = parent == null ? 1 : parent.RightLevel + 1;
                sysRight.RightIDPath = parent == null ? "/"+ sysRight.ID + "/" : parent.RightIDPath + sysRight.ID + "/";
                if (!string.IsNullOrEmpty(sysRight.ID))
                {
                    var exist = rightService.Exist(r => r.ID == sysRight.ID.Trim());
                    if (!exist)
                    {
                        rightService.Add(sysRight);
                        if (rightService.SaveChanges(out msg) > 0)
                        {
                            CacheMaker.IISCache.Remove("all_sys_right");
                            return Json(new AjaxResult() { success = true, msg = insertSuccess, url = Url.Action("index", "sysright", "admin"), moremsg = msg });
                        }
                        else
                        {
                            return Json(new AjaxResult() { success = false, msg = insertFailure, moremsg = msg });
                        }
                    }
                    {
                        return Json(new AjaxResult() { success = false, msg = insertFailure + ",ID已被占用", moremsg = "ID已被占用" });
                    }
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = insertFailure, moremsg = "ID不能为空" });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = insertFailure+ ",上级权限ID只能为root或者已存在的权限的ID" });
            }
        }

        [LSAuthorize("RightEdit", "SysManage", "RightsManage")]
        public ActionResult Edit(string id)
        {            
            ViewBag.RightPositionList = GetRightPositionItem();
            ViewBag.RightActionTypeItem = GetRightActionTypeItem();            
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysRight sysRight = rightService.Find(id);
            if (sysRight == null)
            {
                return View("_NoDataInLayout");
            }
            var rightParent=sysRight.Parent;
            if (rightParent != null)
            {
                ViewBag.rightParentName = rightParent.Name;
                ViewBag.rightParentID = rightParent.ID;
            }
            return View(sysRight);
        }

        
        [LSAuthorize("RightEdit", "SysManage", "RightsManage")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,IsAvailable,Name,ParentID,AddBy,AddDate,ActionType,MenuUrl,Position,SortNo,DisplayName")] SysRight sysRight)
        {            
            
            sysRight.UpdateBy = cuser.UserID;
            sysRight.UpdateByName = cuser.LoginName;
            sysRight.UpdateDate = DateTime.Now;
            sysRight.ParentID = string.IsNullOrEmpty(sysRight.ParentID) ? null : sysRight.ParentID;
            if (sysRight.ParentID != sysRight.ID)
            {
                var parent = rightService.Find(sysRight.ParentID);
                if (sysRight.ParentID==null || parent != null)
                {
                    sysRight.RightLevel = parent == null ? 0 : parent.RightLevel + 1;
                    sysRight.RightIDPath = parent == null ? "/"+ sysRight.ID + "/" : parent.RightIDPath  + sysRight.ID + "/";
                    rightService.Update(sysRight);
                    if (rightService.SaveChanges(out msg) > 0)
                    {                        
                        CacheMaker.IISCache.Remove("all_sys_right");
                        return Json(new AjaxResult() { success = true, msg = updateSuccess, url = Url.Action("index", "sysright", "admin"), moremsg = msg });
                    }
                    else
                    {
                        return Json(new AjaxResult() { success = false, msg = updateFailure, moremsg = msg });
                    }
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = updateFailure + ",上级权限ID只能为root或者已存在的权限的ID" });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = updateFailure + ",权限ID不能与上级权限的ID相同" });
            }
        }

        [LSAuthorize("RightDelete", "SysManage", "RightsManage")]
        [HttpPost]
        public ActionResult Delete(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                if (rightService.RightsDelete(ids, out msg) > 0)
                {
                    CacheMaker.IISCache.Remove("all_sys_right");
                    return Json(new AjaxResult() { success = true, msg = deleteSuccess, url = Url.Action("Index") });
                }
                else
                {                    
                    return Json(new AjaxResult() { success = false, msg =errorOutPutToPage?deleteFailure+"<br />"+msg:deleteFailure, moremsg=msg});
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = didnotchoosedata });
            }
        }
        [LSAuthorize("RightAvailable", "SysManage", "RightsManage")]
        public ActionResult Available(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                var rightList=rightService.GetList(r => idarray.Contains(r.ID));
                foreach(var r in rightList)
                {
                    r.IsAvailable = true;
                    rightService.Update(r);
                }
                int successCount = rightService.SaveChanges(out msg);
                if (successCount > 0)
                {
                    CacheMaker.IISCache.Remove("all_sys_right");
                    return Json(new AjaxResult() { success = true, msg = AvailableSuccess, url = Url.Action("Index") });
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = AvailableFailure });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = didnotchoosedata });
            }
        }
        [LSAuthorize("RightUnAvailable", "SysManage", "RightsManage")]
        public ActionResult UnAvailable(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                var rightList = rightService.GetList(r => idarray.Contains(r.ID));
                foreach (var r in rightList)
                {
                    r.IsAvailable = false;
                    rightService.Update(r);
                }
                int successCount = rightService.SaveChanges(out msg);
                if (successCount > 0)
                {
                    CacheMaker.IISCache.Remove("all_sys_right");
                    return Json(new AjaxResult() { success = true, msg = UnAvailableSuccess, url = Url.Action("Index") });
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = UnAvailableFailure });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = didnotchoosedata });
            }
        }

        private IEnumerable<SelectListItem> GetRightPositionItem()
        {
            return new List<SelectListItem> {
                new SelectListItem { Text = "--请选择--", Value = "" },
                new SelectListItem { Text = StaticResourse.RoleTypeDict[RightPositionType.ListTop.ToString()], Value = RightPositionType.ListTop.ToString() },
                new SelectListItem { Text = StaticResourse.RoleTypeDict[RightPositionType.ListRight.ToString()], Value = RightPositionType.ListRight.ToString() } };
        }
        private IEnumerable<SelectListItem> GetRightActionTypeItem()
        {
            return new List<SelectListItem> {
                new SelectListItem { Text = "--请选择--", Value = "" },
                new SelectListItem { Text = RightActionType.View.ToString(), Value = RightActionType.View.ToString() },
                new SelectListItem { Text = RightActionType.Delete.ToString(), Value = RightActionType.Delete.ToString() },
                new SelectListItem { Text = RightActionType.Available.ToString(), Value = RightActionType.Available.ToString() },
                new SelectListItem { Text = RightActionType.UnAvailable.ToString(), Value = RightActionType.UnAvailable.ToString() } };
        }

        
    }
}
