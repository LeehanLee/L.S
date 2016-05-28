using L.S.Home.Models;
using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using L.S.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Areas.admin.Controllers
{
    public class InfoController : LsBaseController
    {
        public IInfoService infoService;
        public InfoController(IInfoService _infoService)
        {
            infoService = _infoService;
        }

        [LSAuthorize("InfoList", "InfoManage", "InfoList")]
        public ActionResult Index()
        {
            var list = infoService.GetPagedList(d => true, page, pageSize, modellist => modellist.OrderByDescending(d => d.AddDate));
            return View(list);
        }
        [LSAuthorize("InfoCreate", "InfoManage", "InfoList")]
        public ActionResult Create()
        {
            return View();
        }
        [LSAuthorize("InfoCreate", "InfoManage", "InfoList")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "IsAvailable,Name,ImgPath,Introduction,Content,Source,Author")] Info info, string ImgPathToDelete = "")
        {
            int successCount = 0, notExistCount = 0;
            string msg = "";
            CommonUtil.RemoveFiles(ImgPathToDelete, out successCount, out notExistCount, out msg);
            info.ID = IdentityCreator.NextIdentity;
            info.AddBy = cuser.UserID;
            info.AddByName = cuser.LoginName;
            info.AddDate = DateTime.Now;
            info.IsDel = false;
            if (string.IsNullOrEmpty(Request["Introduction"]))
            {
                info.Introduction = info.Content.ToNoHtml().ToMaxString(200);
            }
            infoService.Add(info);
            if (infoService.SaveChanges(out msg) > 0)
            {
                return Json(new AjaxResult() { success = true, msg = insertSuccess, url = Url.Action("index", "info", "admin"), moremsg = msg });
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = insertFailure, moremsg = msg });
            }
        }

        [LSAuthorize("InfoEdit", "InfoManage", "InfoList")]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Info model = infoService.Find(id);
            if (model == null)
            {
                return View("_NoDataInLayout");
            }
            return View(model);
        }
        [LSAuthorize("InfoEdit", "InfoManage", "InfoList")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,AddBy,AddByName,AddDate,IsAvailable,Name,Introduction,ImgPath,Content,Source,Author")] Info model,string ImgPathToDelete="")
        {
            int successCount = 0, notExistCount = 0;
            string msg = "";
            CommonUtil.RemoveFiles(ImgPathToDelete, out successCount, out notExistCount, out msg);
            model.UpdateBy = cuser.UserID;
            model.UpdateByName = cuser.LoginName;
            model.UpdateDate = DateTime.Now;
            infoService.Update(model);
            if (infoService.SaveChanges(out msg) > 0)
            {
                return Json(new AjaxResult() { success = true, msg = updateSuccess, url = Url.Action("index", "info", "admin"), moremsg = msg });
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = updateFailure, moremsg = msg });
            }
        }

        [LSAuthorize("InfoView", "InfoManage", "InfoList")]
        public ActionResult InfoView(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Info info = infoService.Find(id);            
            if (info == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }

        [LSAuthorize("InfoDelete", "InfoManage", "InfoList")]
        [HttpPost]
        public ActionResult Delete(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                if (infoService.InfoesDelete(ids, out msg) > 0)
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
        [LSAuthorize("InfoAvailable", "InfoManage", "InfoList")]
        public ActionResult Available(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update Info set isavailable=1 where id in (" + sqlids + ")";
                if (infoService.ExecuteSql(sql, out msg) > 0)
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
        [LSAuthorize("InfoUnAvailable", "InfoManage", "InfoList")]
        public ActionResult UnAvailable(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update Info set isavailable=0 where id in (" + sqlids + ")";
                if (infoService.ExecuteSql(sql, out msg) > 0)
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