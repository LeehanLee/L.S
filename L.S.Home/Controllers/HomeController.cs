using jsTree3.Models;
using L.S.Home.Models;
using L.S.Interface;
using L.S.Interface.BLL;
using L.S.Model.DatabaseModel.Entity;
using L.Study.Common.Cache;
using L.Study.Common.Config;
using L.Study.Common.Cookie;
using L.Study.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Controllers
{
    public class HomeController : Controller
    {
        /*
        #region private
        private IUserService userService;
        private IRightService rightService;
        private IDepService depService;
        private IRoleService roleService;
        private IUserBLL userBLL;
        #endregion

        public IndexController(IUserService _userService, IRightService _rightService, IDepService _depService,
            IRoleService _roleService, IUserBLL _userBLL)
        {
            userService = _userService;
            rightService = _rightService;
            depService = _depService;
            roleService = _roleService;
            userBLL = _userBLL;
        }

        #region 页面与页面中可访问的接口

        #region 页面与方法
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(SysUser model, string returnurl = "")
        {
            SysUser verifiedUser;
            if (VerifyUser(model, out verifiedUser))
            {
                var homePath = "";
                if (userBLL.SignIn(verifiedUser,out homePath))
                {
                    string url = Url.Action("index","sysuser","admin");//再次取一个硬代码规定的的主页
                    if (!string.IsNullOrEmpty(homePath)) { url = homePath; }//其次取数据库内为角色设置的默认主页
                    if (!string.IsNullOrEmpty(returnurl)) { url = returnurl; }//最dmu优先取访问时带来的返回地址
                    return Json(new AjaxResult() { success = true, msg = "登录成功", url = url });
                }
                else
                {
                    return Json(new AjaxResult() { success = false, msg = "登录失败，请联系管理员", moremsg = "管理员请检查缓存服务是否启动" });
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
            return RedirectToAction("Login");
        }
        public ActionResult NoPermission()
        {
            return View();
        } 
        #endregion

        #region 异步请求方法

        #region 获取权限树
        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <param name="selectNodeID"></param>
        /// <returns></returns>
        public ActionResult GetRightTree(string selectNodeID,string userid="",string thisid="")
        {
            List<JsTree3Node> rightTreeNodes = new List<JsTree3Node>();
            var rightList = rightService.GetList(r => r.IsAvailable && !r.IsDel);
            rightList.Where(r => r.ParentID == null).ToList().ForEach(r =>
            {
                bool disabled = r.RightIDPath.Split('/').Any(pid => pid == thisid);
                bool select = !string.IsNullOrEmpty(selectNodeID) && selectNodeID.Contains(r.ID);
                var node = new JsTree3Node
                {
                    id = r.ID,
                    text = r.Name,
                    state = new State(false, disabled, select),
                    children = new List<JsTree3Node>()
                };
                GenerateTree(node, rightList, selectNodeID,disabled,thisid);
                rightTreeNodes.Add(node);
            });

            return Json(rightTreeNodes, JsonRequestBehavior.AllowGet);
        }
        private void GenerateTree(JsTree3Node node, List<SysRight> list, string selectNodeID,bool parentDisabled,string thisid)
        {
            var childs = list.Where(d => d.ParentID == node.id);

            foreach (var c in childs)
            {
                bool disabled = parentDisabled|| c.RightIDPath.Split('/').Any(pid => pid == thisid);
                bool select = !string.IsNullOrEmpty(selectNodeID) && selectNodeID.Contains(c.ID);
                var child = new JsTree3Node() 
                {
                    id = c.ID,
                    text = c.Name,
                    state = new State(false, disabled, select),
                    children = new List<JsTree3Node>()
                };
                GenerateTree(child, list, selectNodeID, disabled, thisid);
                node.children.Add(child);
            }
        } 
        #endregion

        #region 获取部门树
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="thisid">此ID对应的部门不包含在返回的树节点里（场景：部门编辑时可以选择上级部门，而这个选择范围不应该包含它自己）</param>
        /// <param name="selectNodeID">当前已选中的部门ID</param>
        /// <returns></returns>
        public ActionResult GetDepTree(string id = "root", string thisid = "", string selectNodeID = "")
        {
            id = string.IsNullOrEmpty(id) || id == "#" ? "root" : id;
            Expression<Func<SysDep, bool>> exp = d => d.DepFullIDPath.Contains(id);
            var deps = depService.GetList(exp);
            var level1 = deps.FirstOrDefault(d => d.ID == id);
            var disabled = !string.IsNullOrEmpty(thisid) && level1.DepFullIDPath.Contains(thisid);
            bool select = selectNodeID == level1.ID;
            var root = new JsTree3Node() // Create our root node and ensure it is opened
            {
                id = level1.ID,
                text = level1.Name,
                state = new State(true, disabled, select),
                children = new List<JsTree3Node>()
            };
            GenerateTree(root, deps, selectNodeID, disabled, thisid);
            return Json(root, JsonRequestBehavior.AllowGet);
        }
        private void GenerateTree(JsTree3Node node, List<SysDep> list, string selectNodeID,bool parentDisabled,string thisid)
        {
            var childs = list.Where(d => d.ParentID == node.id);

            foreach (var c in childs)
            {
                var disabled = !string.IsNullOrEmpty(thisid)&&(parentDisabled || c.DepFullIDPath.Contains(thisid));
                bool select = selectNodeID == c.ID;
                var child = new JsTree3Node() 
                {
                    id = c.ID,
                    text = c.Name,
                    state = new State(false, disabled, select),
                    children = new List<JsTree3Node>()
                };
                GenerateTree(child, list, selectNodeID, disabled,thisid);
                node.children.Add(child);
            }
        }
        #endregion

        #region 获取角色树
        public ActionResult GetRoleTree(string selectNodeID ="",string thisid="")
        {
            var cuser = userBLL.GetCurrentUser();
            Expression<Func<SysRole, bool>> exp = sr => !sr.IsDel && sr.IsAvailable ;
            var roles = roleService.GetList(exp);
            var level1 = roles.FirstOrDefault(d => d.Parent == null);            
            bool disabled =level1.RoleIDPath.Split('/').Any(rid=>rid==thisid)|| !cuser.RolesID.Contains(level1.ID);
            var selectedNodeIDs = selectNodeID.Split(',');
            bool selected = selectedNodeIDs.Any(nodeid=>nodeid==level1.ID);
            var root = new JsTree3Node() 
            {
                id = level1.ID,
                text = level1.Name,
                state = new State(true, disabled, selected),
                children = new List<JsTree3Node>()
            };
            GenerateRoleTree(root, roles, cuser.RolesID, disabled, selectedNodeIDs, thisid);
            return Json(root, JsonRequestBehavior.AllowGet);
        }
        private void GenerateRoleTree(JsTree3Node node, List<SysRole> list, string userRolesID,bool parentDisabled ,string[] selectedNodeIDs, string thisid)
        {
            var childs = list.Where(d => d.ParentID == node.id);

            foreach (var c in childs)
            {
                bool disabled = c.RoleIDPath.Split('/').Any(rid => rid == thisid) || (parentDisabled && !userRolesID.Contains(c.ID));
                var child = new JsTree3Node()
                {
                    id = c.ID,
                    text = c.Name,
                    state = new State(false, disabled, selectedNodeIDs.Any(nodeid => nodeid == c.ID)),
                    children = new List<JsTree3Node>()
                };
                GenerateRoleTree(child, list, userRolesID, disabled, selectedNodeIDs, thisid);
                node.children.Add(child);
            }
        }
        #endregion

        #endregion

        #endregion

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
            else {
                verifiedUser = null;
                return false;
            }
        } 
        #endregion
        */

        public ActionResult Index()
        {
            return View();
        }
    }
}