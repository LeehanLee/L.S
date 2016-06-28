using L.S.Common;
using L.S.Home.Models;
using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Areas.admin.Controllers
{
    public class CategoryTypeController : LsBaseController
    {
        public ICategoryTypeService cateTypeService;
        public CategoryTypeController(ICategoryTypeService _cateTypeService)
        {
            cateTypeService = _cateTypeService;
        }

        [LSAuthorize("CategoryTypeManage", "SysManage", "CategoryTypeManage")]
        public ActionResult Index()
        {
            var list = cateTypeService.GetPagedList(d => true, page, pageSize, modellist => modellist.OrderBy(l=>l.SortNo).ThenByDescending(d => d.AddDate));
            return View(list);
        }

        [LSAuthorize("CategoryTypeCreate", "SysManage", "CategoryTypeManage")]
        public ActionResult Create()
        {
            return View();
        }

        [LSAuthorize("CategoryTypeCreate", "SysManage", "CategoryTypeManage")]
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,IsAvailable,Name,SortNo")]CategoryType model, string SysRolesID)
        {
            if (string.IsNullOrEmpty(Request["ID"])) { model.ID = IdentityCreator.NextIdentity; }//如果用户写了ID，就使用管理员写的ID            
            model.AddBy = cuser.UserID;
            model.AddByName = cuser.LoginName;
            model.AddDate = DateTime.Now;
            model.IsDel = false;
            if (string.IsNullOrEmpty(Request["SortNo"])) { model.SortNo = 0; }
            cateTypeService.Add(model);
            if (cateTypeService.SaveChanges(out msg) > 0)
            {
                return Json(new AjaxResult() { success = true, msg = insertSuccess, url = Url.Action("index", "categorytype", "admin"), moremsg = msg });
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = insertFailure, moremsg = msg });
            }
        }

        [LSAuthorize("CategoryTypeEdit", "SysManage", "CategoryTypeManage")]
        public ActionResult Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                CategoryType model = cateTypeService.Find(u => u.ID == id);
                return View(model);
            }
            else
            {
                return View("_NoDataInLayout");
            }
        }
        [LSAuthorize("CategoryTypeEdit", "SysManage", "CategoryTypeManage")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,IsAvailable,Name,SortNo,AddBy,AddByName,AddDate")]CategoryType model, string SysRolesID)
        {
            if (!string.IsNullOrEmpty(model.ID))
            {
                model.UpdateBy = cuser.UserID;
                model.UpdateByName = cuser.LoginName;
                model.UpdateDate = DateTime.Now;
                model.IsDel = false;
                cateTypeService.Update(model);
                var result = cateTypeService.SaveChanges(out msg);
                if (result > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = updateSuccess, url = Url.Action("index") });
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

        [LSAuthorize("CategoryTypeDelete", "SysManage", "CategoryTypeManage")]
        public ActionResult Delete(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                if (cateTypeService.CategoryTypesDelete(ids, out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = deleteSuccess, url = Url.Action("index") });
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

        [LSAuthorize("CategoryTypeAvailable", "SysManage", "CategoryTypeManage")]
        public ActionResult Available(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update CategoryType set isavailable=1,UpdateDate=GETDATE(),UpdateBy='" + cuser.UserID + "',UpdateByName='" + cuser.LoginName + "' where id in (" + sqlids + ")";
                if (cateTypeService.ExecuteSql(sql, out msg) > 0)
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
        [LSAuthorize("CategoryTypeUnAvailable", "SysManage", "CategoryTypeManage")]
        public ActionResult UnAvailable(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update CategoryType set isavailable=0,UpdateDate=GETDATE(),UpdateBy='" + cuser.UserID + "',UpdateByName='" + cuser.LoginName + "' where id in (" + sqlids + ")";
                if (cateTypeService.ExecuteSql(sql, out msg) > 0)
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
    }
}
