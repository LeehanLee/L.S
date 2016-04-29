using L.S.Model.DatabaseModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace L.S.Home.Models
{
    public class TreeOptions
    {
        public string ContainerID { get; set; }
    }
    public static class HtmlHelper
    {
        public static MvcHtmlString SysRightTree(this System.Web.Mvc.HtmlHelper html,
                                                 IList<SysRight> list, TreeOptions option=null)
        {

            var div = new TagBuilder("div");
            div.GenerateId(option == null ? "sysrighttreecontainer" : string.IsNullOrEmpty(option.ContainerID) ? "sysrighttreecontainer" : option.ContainerID);

            var ul = new TagBuilder("ul");
            foreach (var item in list)
            {
                
            }
            div.InnerHtml += ul.ToString();
            return new MvcHtmlString(div.ToString());            
        }
    }
}