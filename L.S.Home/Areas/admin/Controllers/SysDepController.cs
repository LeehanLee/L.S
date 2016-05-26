using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace L.S.Home.Areas.admin.Controllers
{
    using L.S.Model.DatabaseModel.Context;
    using L.S.Model.DatabaseModel.Entity;
    using L.S.Interface;
    using L.S.Home.Models;
    using L.Study.Common;
    using Common;

    public class SysDepController : LsBaseController
    {
        public IDepService depService;

        public SysDepController(IDepService _depService)
        {
            depService = _depService;
        }

        [LSAuthorize("DepsManage", "SysManage", "DepsManage")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var list = depService.GetPagedList(d => true, page, pageSize, modellist => modellist.OrderByDescending(d => d.AddDate));
            return View(list);
        }

        [LSAuthorize("DepCreate", "SysManage", "DepsManage")]
        public ActionResult Create()
        {
            return View();
        }


        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。        
        [LSAuthorize("DepCreate", "SysManage", "DepsManage")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParentID,IsAvailable,IsDel,Name")] SysDep sysDep)
        {            
            sysDep.ID = IdentityCreator.NextIdentity;
            sysDep.AddBy = "before login";
            sysDep.AddDate = DateTime.Now;
            sysDep.IsDel = false;
            var parentDep=depService.Find(sysDep.ParentID);
            if (parentDep != null)
            {
                sysDep.ParentName = parentDep == null ? null : parentDep.Name;
                sysDep.DepFullIDPath = parentDep.DepFullIDPath + "/" + sysDep.ID;
                sysDep.DepFullNamePath = parentDep.DepFullNamePath + "/" + sysDep.Name;
                depService.Add(sysDep);
                if(depService.SaveChanges(out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = insertSuccess, url = Url.Action("index", "sysdep", "admin"), moremsg = msg });
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = insertFailure, moremsg = msg });
                }
                
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = insertFailure, moremsg = "未找到上级组织" });
            }            
        }

        [LSAuthorize("DepEdit", "SysManage", "DepsManage")]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysDep sysDep = depService.Find(id);
            if (sysDep == null)
            {
                return View("_NoDataInLayout");
            }
            return View(sysDep);
        }

        // POST: admin/sysdep/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [LSAuthorize("DepEdit", "SysManage", "DepsManage")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AddBy,AddDate,ParentID,IsAvailable,Name")] SysDep sysDep)
        {
            sysDep.UpdateBy = cuser.UserID;
            sysDep.UpdateByName = cuser.LoginName;
            sysDep.UpdateDate = DateTime.Now;
            sysDep.IsDel = false;
            var parentDep = depService.Find(sysDep.ParentID);
            if (parentDep != null||sysDep.ID=="root")//一般组织一定要有上级，顶级组织的ID为root，并且没有上级
            {
                sysDep.ParentName = parentDep == null ? null : parentDep.Name;
                sysDep.DepFullIDPath = parentDep == null ? sysDep.ID : parentDep.DepFullIDPath + "/" + sysDep.ID;
                sysDep.DepFullNamePath = parentDep == null ? sysDep.Name : parentDep.DepFullNamePath + "/" + sysDep.Name;
                depService.Update(sysDep);
                if (depService.SaveChanges(out msg) > 0)
                {
                    return Json(new AjaxResult() { success = true, msg = updateSuccess, url = Url.Action("index", "sysdep", "admin"), moremsg = msg });
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = updateFailure, moremsg = msg });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = updateFailure, moremsg = "未找到上级组织" });
            }            
        }

        [LSAuthorize("DepDelete", "SysManage", "DepsManage")]
        [HttpPost]
        public ActionResult Delete(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {                
                if (depService.DepsDelete(ids, out msg) > 0)
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
        [LSAuthorize("DepAvailable", "SysManage", "DepsManage")]
        public ActionResult Available(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update SysDep set isavailable=1 where id in (" + sqlids + ")";
                if (depService.ExecuteSql(sql, out msg) > 0)
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
        [LSAuthorize("DepUnAvailable", "SysManage", "DepsManage")]
        public ActionResult UnAvailable(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idarray = ids.Split(',');
                string sqlids = "'" + string.Join("','", idarray) + "'";
                string sql = "update SysDep set isavailable=0 where id in (" + sqlids + ")";
                if (depService.ExecuteSql(sql, out msg) > 0)
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
