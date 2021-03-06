﻿using jsTree3.Models;
using L.S.Common;
using L.S.Interface;
using L.S.Interface.BLL;
using L.S.Model.DatabaseModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Controllers
{
    public class AsyncController : Controller
    {
        private IRightService rightService;
        private IDepService depService;
        private IUserBLL userBLL;
        private IRoleService roleService;
        private ICategoryService categoryService;

        public AsyncController(IRightService _rightService, IDepService _depService, IRoleService _roleService, IUserBLL _userBLL, ICategoryService _cateService)
        {
            rightService = _rightService;
            depService = _depService;
            userBLL = _userBLL;
            roleService = _roleService;
            categoryService = _cateService;
        }

        #region 异步请求方法

        #region 获取权限树
        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <param name="selectNodeID"></param>
        /// <returns></returns>
        public ActionResult GetRightTree(string selectNodeID, string userid = "", string thisid = "", bool ismanage = false)
        {
            List<JsTree3Node> rightTreeNodes = new List<JsTree3Node>();
            var rightQueryable = rightService.GetQueryable(r => !r.IsDel);
            if (!ismanage)
            {
                rightQueryable = rightQueryable.Where(r => r.IsAvailable);
            }
            var rightList = rightQueryable.OrderBy(d => d.SortNo).ThenByDescending(d => d.AddDate).ToList();
            rightList.Where(r => r.ParentID == null).ToList().ForEach(r =>
            {
                bool disabled = r.RightIDPath.Trim('/').Split('/').Any(pid => pid == thisid);
                bool select = !string.IsNullOrEmpty(selectNodeID) && selectNodeID.Contains(r.ID);
                var node = new JsTree3Node
                {
                    id = r.ID,
                    text = r.Name,
                    state = new State(false, disabled, select),
                    children = new List<JsTree3Node>()
                };
                GenerateTree(node, rightList, selectNodeID, disabled, thisid);
                rightTreeNodes.Add(node);
            });

            return Json(rightTreeNodes, JsonRequestBehavior.AllowGet);
        }
        private void GenerateTree(JsTree3Node node, List<SysRight> list, string selectNodeID, bool parentDisabled, string thisid)
        {
            var childs = list.Where(d => d.ParentID == node.id).OrderBy(d => d.SortNo).ThenByDescending(d => d.AddDate).ToList();

            foreach (var c in childs)
            {
                bool disabled = parentDisabled || c.RightIDPath.Trim('/').Split('/').Any(pid => pid == thisid);
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
            var deps = depService.GetList(exp).OrderBy(d => d.SortNo).ThenByDescending(d => d.AddDate).ToList();
            var level1 = deps.FirstOrDefault(d => d.ID == id);
            var level1_disabled = !string.IsNullOrEmpty(thisid) && level1.DepFullIDPath.Contains("/" + thisid +"/");
            bool select = selectNodeID == level1.ID;
            var root = new JsTree3Node()
            {
                id = level1.ID,
                text = level1.Name,
                state = new State(true, level1_disabled, select),
                children = new List<JsTree3Node>()
            };
            GenerateTree(root, deps, selectNodeID, level1_disabled, thisid);
            return Json(root, JsonRequestBehavior.AllowGet);
        }
        private void GenerateTree(JsTree3Node node, List<SysDep> list, string selectNodeID, bool parentDisabled, string thisid)
        {
            var childs = list.Where(d => d.ParentID == node.id).OrderBy(d => d.SortNo).ThenByDescending(d => d.AddDate).ToList();

            foreach (var c in childs)
            {
                var disabled = !string.IsNullOrEmpty(thisid) && (parentDisabled || c.DepFullIDPath.Contains("/" + thisid + "/"));
                bool select = selectNodeID == c.ID;
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

        #region 获取角色树
        public ActionResult GetRoleTree(string selectNodeID = "", string thisid = "", bool ismanage = false)
        {
            var cuser = userBLL.GetCurrentUser();
            //Expression<Func<SysRole, bool>> exp = sr => !sr.IsDel && sr.IsAvailable /*&& sr.RoleIDPath.Contains("/" + cuser.RoleID)*/;
            //var rightQueryable = rightService.GetQueryable(r => !r.IsDel);
            //if (!ismanage)
            //{
            //    rightQueryable = rightQueryable.Where(r => r.IsAvailable);
            //}
            var roleQueryable = roleService.GetQueryable(r => !r.IsDel);
            if (!ismanage)
            {
                roleQueryable = roleQueryable.Where(r => r.IsAvailable);
            }
            var roles = roleQueryable.OrderBy(d => d.SortNo).ThenByDescending(d => d.AddDate).ToList();
            var level1 = roles.FirstOrDefault(d => d.Parent == null);
            bool disabled = level1.RoleIDPath.Trim('/').Split('/').Any(rid => rid == thisid) || !cuser.RolesID.Contains(level1.ID);
            var selectedNodeIDs = selectNodeID.Split(',');
            bool selected = selectedNodeIDs.Any(nodeid => nodeid == level1.ID);
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
        private void GenerateRoleTree(JsTree3Node node, List<SysRole> list, string userRolesID, bool parentDisabled, string[] selectedNodeIDs, string thisid)
        {
            var childs = list.Where(d => d.ParentID == node.id).OrderBy(d => d.SortNo).ThenByDescending(d => d.AddDate).ToList();

            foreach (var c in childs)
            {
                bool disabled = c.RoleIDPath.Trim('/').Split('/').Any(rid => rid == thisid) || (parentDisabled && !userRolesID.Contains(c.ID));
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

        #region 获取分类树
        public ActionResult GetCategoryTree(string selectNodeID = "", string thisid = "", bool ismanage = false, string cateTypeID = "")
        {
            List<JsTree3Node> root = new List<JsTree3Node>();
            var cuser = userBLL.GetCurrentUser();
            var categoryQueryable = categoryService.GetQueryable(r => !r.IsDel && r.CateTypeID == cateTypeID);
            if (!ismanage)
            {
                categoryQueryable = categoryQueryable.Where(r => r.IsAvailable);
            }
            var categorys = categoryQueryable.OrderBy(d => d.SortNo).ThenByDescending(d => d.AddDate).ToList();
            var level1 = categorys.Where(d => d.Parent == null).ToList();
            if (level1.Count>0)
            {
                level1.ForEach(c => 
                {
                    bool disabled = c.CategoryFullIDPath.Trim('/').Split('/').Any(rid => rid == thisid);
                    var selectedNodeIDs = selectNodeID.Split(',');
                    bool selected = selectedNodeIDs.Any(nodeid => nodeid == c.ID);
                    var node = new JsTree3Node()
                    {
                        id = c.ID,
                        text = c.Name,
                        state = new State(true, disabled, selected),
                        children = new List<JsTree3Node>()
                    };
                    GenerateCategoryTree(node, categorys, disabled, selectedNodeIDs, thisid);
                    root.Add(node);
                });
                
                return Json(root, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("未找到树的顶级节点");
            }
        }
        private void GenerateCategoryTree(JsTree3Node node, List<Category> list, bool parentDisabled, string[] selectedNodeIDs, string thisid)
        {
            var childs = list.Where(d => d.ParentID == node.id).OrderBy(d => d.SortNo).ThenByDescending(d => d.AddDate).ToList();

            foreach (var c in childs)
            {
                bool disabled = c.CategoryFullIDPath.Trim('/').Split('/').Any(rid => rid == thisid) || (parentDisabled);
                var child = new JsTree3Node()
                {
                    id = c.ID,
                    text = c.Name,
                    state = new State(false, disabled, selectedNodeIDs.Any(nodeid => nodeid == c.ID)),
                    children = new List<JsTree3Node>()
                };
                GenerateCategoryTree(child, list, disabled, selectedNodeIDs, thisid);
                node.children.Add(child);
            }
        }
        #endregion

        #endregion

        public ActionResult UploadFile(string fileType = "")
        {
            List<string> list = CommonUtil.SaveUploadFiles(Request.Files, fileType);
            if (list.Count > 0)
            {
                return Content(list.FirstOrDefault());
            }
            return Json(new { success = false, msg = "无文件上传." });
        }
        //[HttpPost]
        //public ActionResult RemoveFile(string data)
        //{
        //    int successCount = 0, notExistCount = 0;
        //    string msg = "";
        //    CommonUtil.RemoveFiles(data, out successCount, out notExistCount, out msg);
        //    if (successCount > 0)
        //    {
        //        return Json(new { success = true, msg = "[成功" + successCount + "个，不存在" + notExistCount + "个]" + msg });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, msg = "[成功" + successCount + "个，不存在" + notExistCount + "个]" + msg });
        //    }
        //}
    }
}