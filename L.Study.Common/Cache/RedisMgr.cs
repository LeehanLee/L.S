using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Cache
{
    public class RedisMgr:ACachemgr, ICacheMgr
    {
        RedisClient redis;
        public RedisMgr(string server="")
        {
            if (string.IsNullOrEmpty(server))
            {
                server = "127.0.0.1:6379";
            }
            redis = new RedisClient(server);
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
            return redis.Add<T>(key, value);
        }
        public override bool Add<T>(string key, T value, int expireByMinutes)
        {
            return redis.Add<T>(key, value,DateTime.UtcNow.AddMinutes(expireByMinutes));
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
            return redis.Set<T>(key, value);
        }
        public override bool Set<T>(string key, T value, int expireByMinutes)
        {
            return redis.Set<T>(key, value,DateTime.UtcNow.AddMinutes(expireByMinutes));
        }
        public override bool Exist(string key)
        {
            var result= redis.Exists(key);
            if (result > 0) { return true; }
            else { return false; }
        }
        public override T Get<T>(string key)
        {
            return redis.Get<T>(key);
        }
        public override bool Remove(string key)
        {
            return redis.Remove(key);
        }
        public override bool RemoveAll()
        {            
            try { redis.FlushAll(); return true; }
            catch { return false; }
        }
        public override bool RemoveAll(string filter)
        {
            try
            {
                var keys = redis.GetKeysByPattern(filter);
                redis.RemoveAll(keys); return true;
            }
            catch { return false; }
        }
    }
}
