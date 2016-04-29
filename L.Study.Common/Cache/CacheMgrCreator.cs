using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Cache
{
    public class SimpleCacheMgrCreator
    {
        /// <summary>
        /// 简单工厂
        /// </summary>
        /// <param name="cacheName">使用什么类型的缓存，如redis，memcached，不区分大小写</param>
        /// <param name="param">可传递 ip+:+端口，如：127.0.0.1:11011</param>
        /// <returns></returns>
        public static ICacheMgr CreateCacheMgr(string cacheName,object param=null)
        {
            ICacheMgr cachemgr=null;
            switch (cacheName.ToLower())
            {
                case "memcached":
                    if (param != null)
                    #region memcached
		{
                        var p1 = param as string[];
                        cachemgr = new MemcachedMgr(p1);
                    }
                    else
                    {
                        cachemgr = new MemcachedMgr();
                    } 
	#endregion
                    break;
                case "redis":
                    #region redis
		            if (param != null)
                    {
                        var p2 = param as string;
                        cachemgr=new RedisMgr(p2);
                    }
                    else
                    {
                        cachemgr = new RedisMgr();
                    } 
	                #endregion
                    break;
                default:break;
            }
            return cachemgr;
        }
    }
}
