using L.S.Model.DatabaseModel.Context;
using L.S.Model.DatabaseModel.Entity;
using L.Study.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace L.S.BLL.SysManage
{
    public class SysRightBLL
    {
        public IList<SysRight> GetAllSysRights()
        {
            var list = CacheMaker.IISCache.GetOrSetThenGet("all_sys_right", () =>
            {
                using(LSContext context=new LSContext())
                {
                    var result = context.SysRights.Where(r => r.IsAvailable && !r.IsDel).ToList();
                    return result;
                }                
            });
            return list;
        }
    }
}
