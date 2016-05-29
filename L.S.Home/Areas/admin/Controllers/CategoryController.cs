using L.S.Common;
using L.S.Home.Models;
using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using L.S.Model.DomainModel.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Areas.admin.Controllers
{
    public class CategoryController : LsBaseController
    {
        public ICategoryService cateService;
        public ICategoryTypeService cateTypeService;
        public CategoryController(ICategoryService _cateService, ICategoryTypeService _cateTypeService)
        {
            cateService = _cateService;
            cateTypeService = _cateTypeService;
        }

        [LSAuthorize("CategoryManage", "SysManage", "CategoryManage")]
        public ActionResult Index(string cateTypeID = "")
        {
            var list = cateService.GetPagedList(cate => cate.CateTypeID == cateTypeID && !cate.IsDel, page, pageSize, modellist => modellist.OrderBy(l => l.SortNo).ThenByDescending(d => d.AddDate));
            var cateTypeList = cateTypeService.GetList(catetype => !catetype.IsDel && catetype.IsAvailable);
            ViewBag.cateTypeList = cateTypeList;
            if (string.IsNullOrEmpty(cateTypeID)) { cateTypeID = cateTypeList.FirstOrDefault().ID; }
            ViewBag.cateTypeID = cateTypeID;
            return View(list);
        }
        [LSAuthorize("CategoryManage", "SysManage", "CategoryManage")]
        public ActionResult TreeIndex(string cateTypeID = "")
        {            
            var cateTypeList = cateTypeService.GetList(catetype => !catetype.IsDel && catetype.IsAvailable);
            ViewBag.cateTypeList = cateTypeList;
            if (string.IsNullOrEmpty(cateTypeID)) { cateTypeID = cateTypeList.FirstOrDefault().ID; }
            ViewBag.cateTypeID = cateTypeID;
            return View();
        }
        [LSAuthorize("CategoryManage", "SysManage", "CategoryManage")]
        public ActionResult GetCategory(string id)
        {
            var model = cateService.GetList(r => r.ID == id).Select(r => new CategoryViewModel
            {
                ID = r.ID,
                Name = r.Name,
                ParentID = r.ParentID,
                ParentName = r.Parent == null ? "" : r.Parent.Name,
                IsAvailable = r.IsAvailable,
                AddBy = r.AddBy,
                AddDate = r.AddDate,
                SortNo = r.SortNo,
                CateTypeID=r.CateTypeID,
            }).FirstOrDefault();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [LSAuthorize("CategoryCreate", "SysManage", "CategoryManage")]
        public ActionResult Create(string cateTypeID = "")
        {
            ViewBag.cateType = cateTypeService.Find(cateTypeID);
            return View();
        }

        [LSAuthorize("CategoryCreate", "SysManage", "CategoryManage")]
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,IsAvailable,Name,SortNo,CateTypeID,ParentID")]Category model, string SysRolesID)
        {
            if (string.IsNullOrEmpty(Request["ID"])) { model.ID = IdentityCreator.NextIdentity; }//如果用户写了ID，就使用管理员写的ID            
            model.AddBy = cuser.UserID;
            model.AddByName = cuser.LoginName;
            model.AddDate = DateTime.Now;
            model.IsDel = false;
            if (string.IsNullOrEmpty(Request["SortNo"])) { model.SortNo = 0; }
            var parent = cateService.Find(model.ParentID);
            if (parent == null)
            {
                model.Level = 0;
                model.CategoryFullIDPath = "/" + model.ID + "/";
                model.CategoryFullNamePath = "/" + model.Name + "/";
            }
            else
            {
                model.Level = parent.Level++;
                model.CategoryFullIDPath = parent.CategoryFullIDPath + model.ID + "/";
                model.CategoryFullNamePath = parent.CategoryFullIDPath + model.Name + "/";
            }
            cateService.Add(model);
            if (cateService.SaveChanges(out msg) > 0)
            {
                return Json(new AjaxResult() { success = true, msg = insertSuccess, url = Url.Action("treeindex", "category", new { cateTypeID = model.CateTypeID }), moremsg = msg });
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = insertFailure, moremsg = msg });
            }
        }

        [LSAuthorize("CategoryEdit", "SysManage", "CategoryManage")]
        public ActionResult Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                //Category model = cateService.Find(u => u.ID == id);
                CategoryViewModel model = cateService.GetQueryable(u => u.ID == id).Select(m => new CategoryViewModel {
                    ID = m.ID,
                    AddBy = m.AddBy,
                    AddByName = m.AddByName,
                    AddDate = m.AddDate,
                    Name = m.Name,
                    ParentName = string.IsNullOrEmpty(m.ParentID) ?"": m.Parent.Name,
                    ParentID = string.IsNullOrEmpty(m.ParentID) ? "" : m.ParentID,
                    IsAvailable = m.IsAvailable,
                    CateTypeID=m.CateTypeID,
                }).FirstOrDefault();
                if (model != null)
                {
                    ViewBag.cateType = cateTypeService.Find(model.CateTypeID);
                    return View(model);
                }
                else
                {
                    return View("_NoDataInLayout");
                }
            }
            else
            {
                return View("_NoDataInLayout");
            }
        }
        [LSAuthorize("CategoryEdit", "SysManage", "CategoryManage")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,IsAvailable,Name,SortNo,AddBy,AddByName,AddDate,CateTypeID,ParentID")]Category model, string SysRolesID)
        {
            if (!string.IsNullOrEmpty(model.ID))
            {
                model.UpdateBy = cuser.UserID;
                model.UpdateByName = cuser.LoginName;
                model.UpdateDate = DateTime.Now;
                model.IsDel = false;
                if (string.IsNullOrEmpty(Request["SortNo"])) { model.SortNo = 0; }
                var parent = cateService.Find(model.ParentID);
                if (parent == null)
                {
                    model.Level = 0;
                    model.CategoryFullIDPath = "/" + model.ID + "/";
                    model.CategoryFullNamePath = "/" + model.Name + "/";
                }
                else
                {
                    model.Level = parent.Level++;
                    model.CategoryFullIDPath = parent.CategoryFullIDPath + model.ID + "/";
                    model.CategoryFullNamePath = parent.CategoryFullIDPath + model.Name + "/";
                }

                cateService.Update(model);
                var result = cateService.SaveChanges(out msg);
                if (result > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = updateSuccess, url = Url.Action("treeindex", new { cateTypeID = model.CateTypeID }) });
                }
                else
                {
                    msg = errorOutPutToPage ? msg : updateFailure;
                    return Json(new AjaxResult() { success = false, msg = msg });
                }
            }
            else
            {
                return View("_NoDataInLayout");
            }
        }

        [LSAuthorize("CategoryDelete", "SysManage", "CategoryManage")]
        public ActionResult Delete(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                if (cateService.CategorysDelete(ids, out msg) > 0)
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

        [LSAuthorize("CategoryAvailable", "SysManage", "CategoryManage")]
        public ActionResult Available(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update Category set isavailable=1 where id in (" + sqlids + ")";
                if (cateService.ExecuteSql(sql, out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = AvailableSuccess, url = Url.Action("treeindex") });
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
        [LSAuthorize("CategoryUnAvailable", "SysManage", "CategoryManage")]
        public ActionResult UnAvailable(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update Category set isavailable=0 where id in (" + sqlids + ")";
                if (cateService.ExecuteSql(sql, out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = UnAvailableSuccess, url = Url.Action("treeindex") });
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