using L.S.Home.BLL;
using L.Study.Common.Cache;
using L.Study.Common.Config;
using L.Study.Common.Cookie;
using L.Study.Common.Crypt;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace L.S.Home.Models
{
    public class CurrentUser
    {
        public string UserID { get; set; }
        public string LoginName { get; set; }
        public string DisplayName { get; set; }
        public string DepName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public string RolesID { get; set; }
        public string RolesName { get; set; }
        public string RightIDs { get; set;}
        public string HomePath { get; set; }
        public static CurrentUser GetCurrentUser()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            CurrentUser cuser = null;
            string cookieLoginInfo = CookieMgr.Get(UserBLL.LoginCookieName);
            if (!string.IsNullOrEmpty(cookieLoginInfo))
            {
                var CrypteKey = ConfigMgr.GetAppSettingString("CrypteKey");
                cuser = JsonConvert.DeserializeObject<CurrentUser>(Cryptor.DesDecrypt(cookieLoginInfo, CrypteKey));
                string cacheLoginInfo = CacheMaker.RedisCache.Get<string>(cuser.UserID);
                if (!string.IsNullOrEmpty(cacheLoginInfo))
                {
                    if(cookieLoginInfo.Equals(cacheLoginInfo))
                    {
                        return cuser;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}