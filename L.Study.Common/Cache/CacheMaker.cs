using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Cache
{
    public class CacheMaker
    {
        public static ICacheMgr RedisCache { get; set; }
        public static ICacheMgr IISCache { get; set; }

        static CacheMaker()
        {
            RedisCache = new RedisMgr();
            IISCache = new IISCacheMgr();
        }
    }
}
