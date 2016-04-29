using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Cache
{
    /// <summary>
    /// 工厂方法模式中的工厂接口
    /// </summary>
    public interface ICacheCreator
    {
        ICacheMgr CreateMgr(object param = null);
    }
}
