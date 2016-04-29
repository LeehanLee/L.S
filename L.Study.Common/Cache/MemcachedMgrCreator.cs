using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Cache
{
    public class MemcachedMgrCreator : ICacheCreator
    {
        public ICacheMgr CreateMgr(object param = null)
        {
            ICacheMgr cachemgr = null;
            if (param != null)
            {
                var p = param as string[];
                cachemgr = new MemcachedMgr(p);
            }
            else
            {
                cachemgr = new MemcachedMgr();
            }
            return cachemgr;
        }
        public static ICacheMgr test()
        {
            return new MemcachedMgr();
        }
    }
}
