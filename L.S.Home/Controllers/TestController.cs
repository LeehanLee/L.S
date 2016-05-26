using L.S.Common;
using L.Study.Common;
using L.Study.Common.Cache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult jsTree()
        {
            return View();
        }
        public ActionResult FormUrlTest()
        {
            return View();
        }
        public ActionResult CacheTest()
        {
            ICacheMgr cache = new MemcachedMgr();
            var r1=cache.Add<string>("username", "EricLee");
            var r2=cache.Add<string>("username", "EricLLLLL");
            ViewBag.cache = cache;
            return View();
        }
        public ActionResult GetThreadMaxCount()
        {
            int maxwork = 0, maxcomp = 0;
            int minwork = 0, mincomp = 0;
            ThreadPool.GetMaxThreads(out maxwork,out maxcomp);
            ThreadPool.GetMinThreads(out minwork, out mincomp);
            return Content(maxwork+"   "+maxcomp+"<br />"+minwork+"   "+mincomp);
        }

        public ActionResult GetResultAfterSleep10()
        {
            Thread.Sleep(10*1000);
            return Content("远程接口调用结果如下：GetResultAfterSleep10 complete");
        }

        #region Uploadify的测试
        public ActionResult Uploadify()
        {
            return View();
        }
        public ActionResult UploadifySave()
        {
            List<string> list = CommonUtil.SaveUploadFiles(Request.Files);
            if (list.Count > 0)
            {
                return Content(list.FirstOrDefault());
            }
            return Json(new { success = false, msg = "无文件上传." });
        }
        
        #endregion

        #region UEditorTest
        public ActionResult UEditorTest()
        {
            return View();
        } 
        #endregion
    }
}
