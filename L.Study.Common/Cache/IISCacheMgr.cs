using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace L.Study.Common.Cache
{
    public class IISCacheMgr : ACachemgr, ICacheMgr
    {
        System.Web.Caching.Cache cache;
        public IISCacheMgr()
        {
            if (HttpContext.Current != null)
            {
                cache = HttpContext.Current.Cache;
            }
            else
            {
                cache = null;
            }
        }
        public override bool Add<T>(string key, T value)
        {
            if (cache == null)
            {
                return false;
            }
            else
            {
                //Insert方法中，如果添加的键已存在，将会进行替换
                if (cache.Get(key) == null)
                {
                    cache.Insert(key, value);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override bool Add<T>(string key, T value, int expireByMinutes)
        {
            if (cache == null)
            {
                return false;
            }
            else
            {
                //Insert方法中，如果添加的键已存在，将会进行替换
                if (cache.Get(key) == null)
                {
                    cache.Insert(key, value, null,
                    System.DateTime.UtcNow.AddMinutes(expireByMinutes),
                    System.Web.Caching.Cache.NoSlidingExpiration);
                    return true;
                }
                else
                {
                    return false;
                }                
            }
        }

        public override bool Exist(string key)
        {
            if (cache == null)
            {
                return false;
            }
            else
            {
                var value = cache.Get(key);
                return value == null;
            }
        }

        public override T Get<T>(string key)
        {
            if (cache == null)
            {
                return null;
            }
            else
            {
                var value = cache.Get(key);
                var result = value as T;
                return result;
            }
        }

        public override bool Remove(string key)
        {
            if (cache == null)
            {
                return false;
            }
            else
            {
                var value = cache.Remove(key);
                return true;
            }
        }

        public override bool RemoveAll()
        {
            if (cache == null)
            {
                return false;
            }
            else
            {                
                IDictionaryEnumerator dicts = cache.GetEnumerator();
                ArrayList alRemove = new ArrayList();
                while (dicts.MoveNext())
                {
                    alRemove.Add(dicts.Key);
                }
                foreach (string key in alRemove)
                {
                    cache.Remove(key);
                }
                return true;
            }
        }

        public override bool RemoveAll(string filter)
        {
            if (cache == null)
            {
                return false;
            }
            else
            {                
                IDictionaryEnumerator dicts = cache.GetEnumerator();
                ArrayList alRemove = new ArrayList();
                while (dicts.MoveNext())
                {
                    if (string.IsNullOrEmpty(filter) || dicts.Key.ToString().IndexOf(filter) > -1)
                    {
                        alRemove.Add(dicts.Key);
                    }
                }
                foreach (string key in alRemove)
                {
                    cache.Remove(key);
                }
                return true;
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
            if (cache == null)
            {
                return false;
            }
            else
            {
                //Insert方法中，如果添加的键已存在，将会进行替换
                cache.Insert(key, value);
                return true;
            }
        }

        /// <summary>
        /// 键存在时值覆盖，键不存在时值添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Set<T>(string key, T value, int expireByMinutes)
        {
            if (cache == null)
            {
                return false;
            }
            else
            {
                //Insert方法中，如果添加的键已存在，将会进行替换
                cache.Insert(key, value, null,
                    DateTime.UtcNow.AddMinutes(expireByMinutes),
                    System.Web.Caching.Cache.NoSlidingExpiration);
                return true;
            }
        }
    }
}
