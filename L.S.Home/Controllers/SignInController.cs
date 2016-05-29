using L.S.Home.Models;
using L.S.Interface;
using L.S.Interface.BLL;
using L.S.Model.DatabaseModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Controllers
{
    public class SignInController : Controller
    {
        #region private
        private IUserService userService;
        private IRightService rightService;
        private IDepService depService;
        private IRoleService roleService;
        private IUserBLL userBLL;
        #endregion

        public SignInController(IUserService _userService, IRightService _rightService, IDepService _depService,
            IRoleService _roleService, IUserBLL _userBLL)
        {
            userService = _userService;
            rightService = _rightService;
            depService = _depService;
            roleService = _roleService;
            userBLL = _userBLL;
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(SysUser model, string returnurl = "")
        {
            SysUser verifiedUser;
            if (VerifyUser(model, out verifiedUser))
            {
                var homePathOrMsg = "";
                if (userBLL.SignIn(verifiedUser, out homePathOrMsg))
                {
                    string url = Url.Action("index", "sysuser", "admin");//再次取一个硬代码规定的的主页
                    if (!string.IsNullOrEmpty(homePathOrMsg)) { url = homePathOrMsg; }//其次取数据库内为角色设置的默认主页
                    if (!string.IsNullOrEmpty(returnurl)) { url = returnurl; }//最优先取访问时带来的返回地址
                    return Json(new AjaxResult() { success = true, msg = "登录成功", url = url });
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = "登录失败，请联系管理员", moremsg = homePathOrMsg });
                }
            }
            else
            {
                return Json(new AjaxResult() { success = false, msg = "用户名或密码有误" });
            }
        }
        public ActionResult SignOut()
        {
            userBLL.SignOut();
            return RedirectToAction("index");
        }
        public ActionResult NoPermission()
        {
            return View();
        }

        #region 私有方法
        /// <summary>
        /// 只进行数据库内用户的账号密码校验
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool VerifyUser(SysUser model, out SysUser verifiedUser)
        {
            SysUser user = userService.Find(u => u.LoginName == model.LoginName && u.Password == model.Password && !u.IsDel && u.IsAvailable);
            if (user != null)
            {
                verifiedUser = user;
                return true;
            }
            else
            {
                verifiedUser = null;
                return false;
            }
        }
        #endregion
    }
}