using L.S.Interface.BLL;
using L.S.Model.DatabaseModel.Entity;
using L.S.Model.DomainModel;
using L.Study.Common.Cache;
using L.Study.Common.Config;
using L.Study.Common.Cookie;
using L.Study.Common.Crypt;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace L.S.BLL.SysManage
{
    public class UserBLL:IUserBLL
    {
        public static string LoginCookieName = ConfigMgr.GetAppSettingString("LoginCookieName");

        #region 登录注销
        /// <summary>
        /// 登录，将用户信息保存到浏览器的cookie与服务器cache里
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SignIn(SysUser model, out string homePathOrMsg)
        {
            CurrentUser cuser = new CurrentUser();
            cuser.UserID = model.ID;
            cuser.LoginName = model.LoginName;
            cuser.LastLoginTime = DateTime.Now;
            var roles = model.SysUserRoles.Where(ur => !ur.SysRole.IsDel && ur.SysRole.IsAvailable).Select(sur => new
            {
                sur.SysRole.Name,
                sur.SysRole.ID,
                sur.SysRole.Level,
                sur.SysRole.DefaultHomePath,
                Rights = sur.SysRole.SysRoleRights.Where(rr => !rr.SysRight.IsDel && rr.SysRight.IsAvailable).Select(rr => rr.RightID)
            }).ToList();
            if (roles != null && roles.Count > 0)
            {
                cuser.HomePath = roles.FirstOrDefault(ro => ro.Level == roles.Min(r => r.Level)).DefaultHomePath;
                homePathOrMsg = cuser.HomePath;
                cuser.RolesID = string.Join(",", roles.Select(r => r.ID).ToArray());
                cuser.RolesName = string.Join(",", roles.Select(r => r.Name).ToArray());
                var rightIDs = roles.SelectMany(rr => rr.Rights).Distinct().ToArray();
                cuser.RightIDs = string.Join(",", rightIDs);
                var cuserStr = JsonConvert.SerializeObject(cuser);
                var CrypteKey = ConfigMgr.GetAppSettingString("CrypteKey");
                var cuserHash = Cryptor.DesEncrypt(cuserStr, CrypteKey);
                string domain = CookieMgr.GetDomain(HttpContext.Current.Request.Url.ToString());
                CookieMgr.Set(LoginCookieName, cuserHash, 0, domain);
                if (CacheMaker.RedisCache.Set("sidkey" + cuser.UserID, cuserHash))
                {
                    return true;
                }
                else
                {
                    CookieMgr.Remove(LoginCookieName);
                    homePathOrMsg = "缓存设置失败，请管理员检查是否安装缓存服务";
                    return false;
                }
            }
            else
            {
                homePathOrMsg = "当前用户没有启用的角色";
                return false;
            }
        }
        /// <summary>
        /// 注销，清除浏览器的cookie和服务器中缓存的用户信息
        /// </summary>
        /// <returns></returns>
        public bool SignOut()
        {
            var cuser = this.GetCurrentUser();
            if (cuser == null)
            {
                return false;
            }
            else
            {
                CookieMgr.Remove(LoginCookieName);
                CacheMaker.RedisCache.Remove("sidkey" + cuser.UserID);
                return true;
            }
        }

        #endregion
        public CurrentUser GetCurrentUser()
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
                string cacheLoginInfo = CacheMaker.RedisCache.Get<string>("sidkey" + cuser.UserID);
                if (!string.IsNullOrEmpty(cacheLoginInfo))
                {
                    if (cookieLoginInfo.Equals(cacheLoginInfo))
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
