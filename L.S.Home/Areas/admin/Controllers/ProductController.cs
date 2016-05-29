using L.S.Home.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Areas.admin.Controllers
{
    public class ProductController : LsBaseController
    {
        [LSAuthorize("ProductManage", "SysManage", "ProductManage")]
        public ActionResult Index()
        {
            return View();
        }
    }
}