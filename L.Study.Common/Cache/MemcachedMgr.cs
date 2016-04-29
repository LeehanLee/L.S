using Memcached.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Cache
{
    public class MemcachedMgr:ACachemgr, ICacheMgr
    {
        MemcachedClient cache;
        public MemcachedMgr()
        {
            string[] serverList1 = { "127.0.0.1:11011" };
            Init(serverList1);
        }
        public MemcachedMgr(params string[] serverList)
        {
            if (serverList == null || serverList.Length == 0)
            {
                string[] serverList1 = { "127.0.0.1:11011" };
                serverList = serverList1;
            }
            Init(serverList);
        }

        private void Init(string[] serverList)
        {
            SockIOPool pool = SockIOPool.GetInstance("Memcached");
            pool.SetServers(serverList);
            pool.Initialize();
            cache = new MemcachedClient();
            cache.PoolName = "Memcached";
            cache.EnableCompression = false;
        }

        /// <summary>
        /// 键存在时不添加也不覆盖，键不存在时添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Add<T>(string key, T value)
        {
            var result= cache.Add(key, value);
            return result;
        }
        public override bool Add<T>(string key, T value,int expireByMinutes)
        {
            return cache.Add(key, value,DateTime.UtcNow.AddMinutes(expireByMinutes));
        }
        /// <summary>
        /// 键存在时值覆盖，键不存在时值添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Set<T>(string key, T value)
        {
            return cache.Set(key, value);
        }
        public override bool Set<T>(string key, T value, int expireByMinutes)
        {
            return cache.Set(key, value,DateTime.UtcNow.AddMinutes(expireByMinutes));
        }
        public override bool Exist(string key)
        {
            return cache.KeyExists(key);
        }
        public override T Get<T>(string key)
        {
            var value=cache.Get(key);
            var result = value as T;
            return result;
        }
        public override bool Remove(string key)
        {
            return cache.Delete(key);
        }
        public override bool RemoveAll()
        {
            return cache.FlushAll();
        }

        public override bool RemoveAll(string filter)
        {
            throw new NotImplementedException();
        }
    }
}
