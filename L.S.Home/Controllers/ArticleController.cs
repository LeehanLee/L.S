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

namespace L.S.Home.Controllers
{
    public class ArticleController : Controller
    {
        private string msg = string.Empty;
        public IInfoService infoService;
        public ArticleController(IInfoService _infoService)
        {
            infoService = _infoService;
        }

        // GET: Article
        public ActionResult List(int page = 1)
        {
            var result = infoService.GetPagedList(info => !info.IsDel && info.IsAvailable, page, 5, orderby => orderby.OrderByDescending(info => info.UpdateDate).OrderByDescending(info => info.AddDate));
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
            return View(info);
        }
    }
}
