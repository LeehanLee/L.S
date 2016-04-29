using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Cache
{
    public class RedisMgrCreator:ICacheCreator
    {
        public ICacheMgr CreateMgr(object param = null)
        {
            ICacheMgr cachemgr = null;                   
            if (param != null)
            {
                var p2 = param as string;
                cachemgr = new RedisMgr(p2);
            }
            else
            {
                cachemgr = new RedisMgr();
            }            
            return cachemgr;
        }
    }
}
