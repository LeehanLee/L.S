using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Cache
{
    public class RedisMgr : ACachemgr, ICacheMgr
    {
        string server = "";
        public RedisMgr(string _server = "")
        {
            if (string.IsNullOrEmpty(_server))
            {
                server = "127.0.0.1:6379";
            }
            else
            {
                server = _server;
            }

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
            using (RedisClient redis = new RedisClient(server))
            {
                return redis.Add<T>(key, value);
            }
        }
        public override bool Add<T>(string key, T value, int expireByMinutes)
        {
            using (RedisClient redis = new RedisClient(server))
            {
                return redis.Add<T>(key, value, DateTime.UtcNow.AddMinutes(expireByMinutes));
            }
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
            using (RedisClient redis = new RedisClient(server))
            {
                return redis.Set<T>(key, value);
            }
        }
        public override bool Set<T>(string key, T value, int expireByMinutes)
        {
            using (RedisClient redis = new RedisClient(server))
            {
                return redis.Set<T>(key, value, DateTime.UtcNow.AddMinutes(expireByMinutes));
            }
        }
        public override bool Exist(string key)
        {
            using (RedisClient redis = new RedisClient(server))
            {
                var result = redis.Exists(key);
                if (result > 0) { return true; }
                else { return false; }
            }
        }
        public override T Get<T>(string key)
        {
            using (RedisClient redis = new RedisClient(server))
            {
                return redis.Get<T>(key);
            }
        }
        public override bool Remove(string key)
        {
            using (RedisClient redis = new RedisClient(server))
            {
                return redis.Remove(key);
            }
        }
        public override bool RemoveAll()
        {
            using (RedisClient redis = new RedisClient(server))
            {
                try { redis.FlushAll(); return true; }
                catch { return false; }
            }
        }
        public override bool RemoveAll(string filter)
        {
            using (RedisClient redis = new RedisClient(server))
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
}
