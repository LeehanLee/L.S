using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Cache
{
    public abstract class ACachemgr : ICacheMgr
    {
        public abstract bool Add<T>(string key, T value);
        public abstract bool Add<T>(string key, T value, int expireByMinutes = 0);
        public abstract bool Exist(string key);
        public abstract T Get<T>(string key) where T : class;
        public abstract bool Remove(string key);
        public abstract bool RemoveAll();
        public abstract bool RemoveAll(string filter);
        public abstract bool Set<T>(string key, T value);
        public abstract bool Set<T>(string key, T value, int expireByMinutes=0);
        public virtual T GetOrSetThenGet<T>(string key, Func<T> getFunc, int expireByMinutes = 0) where T : class
        {
            var value = Get<T>(key);
            if (value != null)
            {
                return value as T;
            }
            else
            {
                T result = getFunc();
                Set<T>(key, result,expireByMinutes);
                return result as T;
            }
        }        
    }
}
