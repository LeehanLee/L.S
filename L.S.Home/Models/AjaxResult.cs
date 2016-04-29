using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L.S.Home.Models
{
    public class AjaxResult
    {
        public bool success { get; set; }
        public string type { get; set; }
        public string msg { get; set; }
        public object moremsg { get; set; }
        public string url { get; set; }
    }
}