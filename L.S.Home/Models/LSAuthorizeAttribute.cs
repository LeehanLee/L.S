using L.S.Model.DatabaseModel.Entity;
using L.Study.Common.Cache;
using L.Study.Common.Config;
using L.Study.Common.Cookie;
using L.Study.Common.Crypt;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using L.S.Model.DomainModel;
using L.S.BLL.SysManage;
using L.S.Interface;
using Autofac;

namespace L.S.Home.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class LSAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        //IRightService rightService;
        public LSAuthorizeAttribute()
        {
            
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rightCode">当前权限ID</param>
        /// <param name="currentTopRightCode">当前顶部选中权限ID</param>
        /// <param name="currentLeftRightCode">当前右侧选中权限ID</param>
        public LSAuthorizeAttribute(string rightCode, string currentTopRightCode = "", string currentLeftRightCode = "")
        {
            RightCode = rightCode;
            CurrentActiveTopMenuCode = currentTopRightCode;
            CurrentActiveLeftMenuCode = currentLeftRightCode;            
        }

        /// <summary>
        /// 当前权限ID
        /// </summary>
        public string RightCode { get; private set; }

        /// <summary>
        /// 当前的顶部锁定的权限ID
        /// </summary>
        public string CurrentActiveTopMenuCode { get; private set; }

        /// <summary>
        /// 当前锁定的左侧权限ID
        /// </summary>
        public string CurrentActiveLeftMenuCode { get; private set; }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            CurrentUser cuser;
            UrlHelper Url = new UrlHelper(filterContext.RequestContext);
            var request = filterContext.RequestContext.HttpContext.Request;
            string url = string.Empty;
            if (IsSignIn(request, out cuser))
            {                
                IList<SysRight> allRightList = new SysRightBLL().GetAllSysRights();
                ControllerBase c = filterContext.Controller;
                c.ViewBag.cuser = cuser;
                c.ViewBag.RightList = allRightList;
                var userRightIDList = cuser.RightIDs.Split(',');
                
                var UserRightList = allRightList.Where(ar => userRightIDList.Contains(ar.ID)).ToList();
                c.ViewBag.UserRightList = UserRightList;
                var pageRightList = UserRightList.Where(r => r.ParentID == CurrentActiveLeftMenuCode).ToList();
                c.ViewBag.pageTopRightList = pageRightList.Where(r => r.Position == "ListTop").OrderBy(r=>r.SortNo).ToList();
                c.ViewBag.pageRightRightList = pageRightList.Where(r => r.Position == "ListRight").OrderBy(r=>r.SortNo).ToList();

                c.ViewBag.RightCode = RightCode;
                c.ViewBag.CurrentActiveTopMenuCode = CurrentActiveTopMenuCode;
                c.ViewBag.CurrentActiveLeftMenuCode = CurrentActiveLeftMenuCode;
                PropertyInfo p = c.GetType().GetProperty("cuser");
                p.SetValue(c, cuser);
                if (string.IsNullOrEmpty(RightCode)||cuser == null||string.IsNullOrEmpty(cuser.RightIDs)||!cuser.RightIDs.Contains(RightCode))//若没有权限，重定向到一个无权限的说明页面
                {
                    string no_permission_url = Url.Action("nopermission", "home", new { area = "" });
                    HttpContext.Current.Response.Redirect(no_permission_url, true);
                    return;
                }
            }
            else
            {
                url =string.IsNullOrEmpty(url)? Url.Action("index", "signin", new { area = "", returnurl = request.Url.ToString() }):url;
                HttpContext.Current.Response.Redirect(url, true);
                return;                
            }            
        }
        /// <summary>
        /// 判断当前请求是否还了用户登录的cookie信息，如果带了，则再判断服务器缓存中记录是否有该用户的登录信息并是否一致
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cuser"></param>
        /// <returns></returns>
        private bool IsSignIn(HttpRequestBase request,out CurrentUser cuser)
        {
            cuser = null;
            string cookieLoginInfo=CookieMgr.Get(UserBLL.LoginCookieName);
            if (!string.IsNullOrEmpty(cookieLoginInfo))
            {
                var CrypteKey = ConfigMgr.GetAppSettingString("CrypteKey");
                cuser = JsonConvert.DeserializeObject<CurrentUser>(Cryptor.DesDecrypt(cookieLoginInfo, CrypteKey));
                string cacheLoginInfo = CacheMaker.RedisCache.Get<string>("sidkey" + cuser.UserID);
                if (!string.IsNullOrEmpty(cacheLoginInfo))
                {
                    return cookieLoginInfo.Equals(cacheLoginInfo);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

}