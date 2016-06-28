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
using System.Data.SqlClient;
using L.Study.Common.Cache;
using System.Linq.Expressions;

namespace L.S.Home.Controllers
{
    public class ArticleController : Controller
    {
        private string msg = string.Empty;
        public IInfoService infoService;
        public ICategoryService cateService;
        public ArticleController(IInfoService _infoService, ICategoryService _cateService)
        {
            infoService = _infoService;
            cateService = _cateService;
        }

        // GET: Article
        public ActionResult List(string infoCategoryID = "", int page = 1)
        {
            Expression<Func<Info, bool>> exp = info => !info.IsDel && info.IsAvailable;
            if (!string.IsNullOrEmpty(infoCategoryID))
            {
                exp = info => !info.IsDel && info.CategoryID == infoCategoryID;
                ViewBag.infoCategoryID = infoCategoryID;
            }
            var result = infoService.GetPagedList(exp, page, 5, orderby => orderby.OrderByDescending(info => info.UpdateDate).ThenByDescending(info => info.AddDate));
            var InfoCategoryTypeList = CacheMaker.IISCache.GetOrSetThenGet("InfoCategoryType_Cache_Key", () =>
            {
                return cateService.GetQueryable(cate => cate.CateTypeID == "InfoCategoryType").OrderByDescending(c => c.UpdateDate).ThenByDescending(c => c.AddDate).Select(cate => new SelectListItem { Value = cate.ID, Text = cate.Name }).ToList();
            });
            ViewBag.InfoCategoryTypeList = InfoCategoryTypeList;
            return View(result);
        }

        // GET: Article/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Info info = infoService.Find(id);
            infoService.ExecuteSql("update Info set ViewCount=ViewCount+1 where id=@id", out msg, new SqlParameter("@id", id));
            if (info == null)
            {
                return HttpNotFound();
            }
            var InfoCategoryTypeList = CacheMaker.IISCache.GetOrSetThenGet("InfoCategoryType_Cache_Key", () =>
            {
                return cateService.GetQueryable(cate => cate.CateTypeID == "InfoCategoryType").OrderByDescending(c => c.UpdateDate).ThenByDescending(c => c.AddDate).Select(cate => new SelectListItem { Value = cate.ID, Text = cate.Name }).ToList();
            });
            ViewBag.InfoCategoryTypeList = InfoCategoryTypeList;
            return View(info);
        }
    }
}
