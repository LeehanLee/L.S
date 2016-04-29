using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace L.Study.Common.Cookie
{
    public class CookieMgr
    {
        /// <summary>
        /// 设置cookie操作
        /// </summary>
        /// <param name="name">cookie名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">时间 /秒</param>
        /// <param name="domain">cookie域</param>
        public static void Set(string name, string value, int expires = 0, string domain = "")
        {
            if (HttpContext.Current == null)
            {
                return;
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                cookie = new HttpCookie(name);
            else
            {
                if (string.IsNullOrEmpty(domain) && cookie.Domain != null)
                {
                    domain = cookie.Domain;
                }
            }
            if (expires != 0)
            {
                cookie.Expires = DateTime.Now.AddSeconds(expires);
            }
            if (!string.IsNullOrEmpty(domain))
            {
                cookie.Domain = domain;
            }
            cookie.Value = HttpUtility.UrlEncode(value, Encoding.UTF8);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddMinutes(-1));
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 根据name移除cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        public static void Remove(string name, string domain = "")
        {
            Set(name, "", -3600, domain);
        }
        /// <summary>
        /// 根据name获取cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Get(string name)
        {
            if (HttpContext.Current == null) return "";
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                return HttpUtility.UrlDecode(cookie.Value, Encoding.UTF8);
            }
            return "";
        }

        public static string Get(object userBll)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除所有cookie
        /// </summary>
        /// <param name="filter"></param>        
        public static void RemoveAll(string filter = "")
        {
            HttpCookieCollection hcc = HttpContext.Current.Request.Cookies;
            foreach (string key in hcc.AllKeys)
            {
                if (string.IsNullOrEmpty(filter) || key.IndexOf(filter) > -1)
                {
                    Remove(key);                    
                }
            }
        }
        public static string GetDomain(string url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }

            var uri = new Uri(url);
            string rootDomain;
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    {
                        if (uri.IsLoopback)
                        {
                            rootDomain = uri.Host;
                        }
                        else
                        {
                            string host = uri.Host;
                            rootDomain = host;
                        }
                    }
                    break;
                default:
                    rootDomain = uri.Host;
                    break;
            }
            return rootDomain;
        }
    }
}
