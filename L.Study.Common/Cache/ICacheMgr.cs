using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Cache
{
    public interface ICacheMgr
    {
        bool Add<T>(string key, T value);
        bool Set<T>(string key, T value);
        bool Add<T>(string key, T value, int expireByMinutes);
        bool Set<T>(string key, T value, int expireByMinutes);
        bool Exist(string key);
        T Get<T>(string key) where T:class;
        bool Remove(string key);
        bool RemoveAll();
        bool RemoveAll(string filter);
        T GetOrSetThenGet<T>(string key, Func<T> getFunc, int expireByMinutes=0) where T:class;
    }
}
