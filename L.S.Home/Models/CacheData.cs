using Autofac;
using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using L.Study.Common.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L.S.Home.Models
{
    public class CacheData
    {
        /// <summary>
        /// 使用IIS应用程序池缓存起来的当前系统内的所有权限
        /// </summary>
        public static IList<SysRight> AllSysRights
        {
            get
            {
                var list = CacheMaker.IISCache.GetOrSetThenGet("all_sys_right", () => {
                    var rightService = IocConfig.Container.Resolve<IRightService>();
                    var result = rightService.GetList(r => r.IsAvailable && !r.IsDel);
                    return result;
                });
                return list;
            }
        }
    }
}