using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Models
{
    using Interface;
    using L.Study.Common.Config;
    using Model.DatabaseModel.Entity;
    using Newtonsoft.Json;
    using Service;
    using Study.Common.Cache;
    public class LsBaseController : Controller
    {
        public CurrentUser cuser { get; set; }
        public int page = 1;
        public int pageSize = 10;
        public int totalCount = 0;
        public string msg = string.Empty;
        /// <summary>
        /// 错误消息是否返回给浏览器
        /// </summary>
        public bool errorOutPutToPage = false;
        /// <summary>
        /// 可选的一页数据条数
        /// </summary>
        public List<int> optionsForPerPageDataCount;

        public string insertSuccess = "添加成功";
        public string insertFailure = "添加失败";
        public string updateSuccess = "更新成功";
        public string updateFailure = "更新失败";
        public string deleteSuccess = "删除成功";
        public string deleteFailure = "删除失败";
        public string AvailableSuccess = "启用成功";
        public string AvailableFailure = "启用失败";
        public string UnAvailableSuccess = "禁用成功";
        public string UnAvailableFailure = "禁用失败";
        /// <summary>
        /// 请选择要删除的数据
        /// </summary>
        public string didnotchoosedata = "请选择要操作的数据";

        public LsBaseController()
        {
            errorOutPutToPage = ConfigMgr.GetAppSettingBool("IsErrorOutPutToPage");
            string[] optionsStringForPerPageDataCount = ConfigMgr.GetAppSettingString("OptionsForPerPageDataCount").Split(',');
            optionsForPerPageDataCount = new List<int>();
            foreach (var option in optionsStringForPerPageDataCount)
            {
                int result = 0;
                if (int.TryParse(option, out result))
                {
                    optionsForPerPageDataCount.Add(result);
                }
            }
            optionsForPerPageDataCount.Reverse();
            ViewBag.optionsForPerPageDataCount = optionsForPerPageDataCount;
        }

        /// <summary>
        /// 获取当前用户本身的角色以及它的下级角色生成一个IEnumerable<SelectListItem>
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userRoleID">当前用户的角色ID</param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetRoleListAsEnumerable(IRoleService service, string userRoleID)
        {            
            var SysRoles= 
                //CacheMaker.Cache.GetOrSetThenGet("SysRoles", (key) => {
                //var sysrolelist = 
                    service.GetList(r => r.IsAvailable && !r.IsDel);
            //    for(int i=0;i< sysrolelist.Count;i++)
            //    {
            //        var r = sysrolelist[i];
            //        r.Parent = null;
            //        r.ParentID = null;
            //    }
            //    return (sysrolelist);//坑爹的redis不能保存树级对象SysRolesStr
            //});
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "", Text = "--请选择--" });
            list.Add(new SelectListItem {Value= userRoleID,Text=SysRoles.FirstOrDefault(r => r.ID == userRoleID).Name });
            GenerateRoleListEnumerable(SysRoles, list, userRoleID);
            return list;
        }
        private void GenerateRoleListEnumerable(List<SysRole> SysRoles,List<SelectListItem> list, string parentRoleID)
        {
            var childs = SysRoles.Where(r => r.ParentID == parentRoleID);
            foreach (var c in childs)
            {
                list.Add(new SelectListItem { Value = c.ID, Text = c.Name });
                GenerateRoleListEnumerable(SysRoles,list, c.ID);
            }
        }
    }
}
