using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Service
{
    //public class CommonStr
    //{
        
    //}
    public enum RightPositionType
    {
        /// <summary>
        /// 列表页顶部
        /// </summary>
        ListTop = 0,
        /// <summary>
        /// 列表页右边
        /// </summary>
        ListRight
    }
    public enum RightActionType
    {
        View=0,
        Delete,
        Available,
        UnAvailable,
    }
    public class StaticResourse
    {
        public static readonly Dictionary<string, string> RoleTypeDict = new Dictionary<string, string> {
            { RightPositionType.ListTop.ToString(),"列表页顶部"},
            { RightPositionType.ListRight.ToString(), "列表页右边"},
        };
        public static readonly Dictionary<string, string> RightActionTypeDict = new Dictionary<string, string> {
            { RightActionType.View.ToString(),"跳转页面"},
            { RightActionType.Delete.ToString(), "删除"},
        };
    }
}
