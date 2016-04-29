using Autofac;
using L.S.Home.Models;
using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using L.S.Service;
using L.Study.Common.Cache;
using L.Study.Common.Config;
using L.Study.Common.Cookie;
using L.Study.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L.S.Home.BLL
{
    public class UserBLL
    {
        public static string LoginCookieName=ConfigMgr.GetAppSettingString("LoginCookieName");

        #region 登录注销
        /// <summary>
        /// 登录，将用户信息保存到浏览器的cookie与服务器cache里
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static bool SignIn(SysUser Model,out string homePath)
        {
            CurrentUser cuser = new CurrentUser();
            cuser.UserID = Model.ID;
            cuser.LoginName = Model.LoginName;
            cuser.LastLoginTime = DateTime.Now;
            var roles = Model.SysUserRoles.Select(sur => new { sur.SysRole.Name, sur.SysRole.ID, sur.SysRole.Level, sur.SysRole.DefaultHomePath, Rights = sur.SysRole.SysRoleRights.Select(rr => rr.RightID) }).ToList();

            cuser.HomePath = roles.FirstOrDefault(ro => ro.Level == roles.Min(r => r.Level)).DefaultHomePath;
            homePath = cuser.HomePath;
            cuser.RolesID = string.Join(",", roles.Select(r => r.ID).ToArray());
            cuser.RolesName = string.Join(",", roles.Select(r => r.Name).ToArray());
            var rightIDs = roles.SelectMany(rr => rr.Rights).Distinct().ToArray();
            cuser.RightIDs = string.Join(",", rightIDs);
            var cuserStr = Newtonsoft.Json.JsonConvert.SerializeObject(cuser);
            var CrypteKey = ConfigMgr.GetAppSettingString("CrypteKey");
            var cuserHash = Cryptor.DesEncrypt(cuserStr, CrypteKey);
            string domain = CookieMgr.GetDomain(HttpContext.Current.Request.Url.ToString());
            CookieMgr.Set(LoginCookieName, cuserHash, 0, domain);
            if (CacheMaker.RedisCache.Set(cuser.UserID, cuserHash))
            {
                return true;
            }
            else
            {
                CookieMgr.Remove(LoginCookieName);
                return false;
            }
        }
        /// <summary>
        /// 注销，清除浏览器的cookie和服务器中缓存的用户信息
        /// </summary>
        /// <returns></returns>
        public static bool SignOut()
        {
            var cuser=CurrentUser.GetCurrentUser();
            if (cuser == null)
            {
                return false;
            }
            else
            {
                CookieMgr.Remove(LoginCookieName);
                CacheMaker.RedisCache.Remove(cuser.UserID);
                return true;
            }
        }

        #endregion
    }
}