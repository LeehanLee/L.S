using L.S.Model.DatabaseModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Interface
{
    public interface IRightService: IBaseService<SysRight>
    {
        int RightsDelete(string ids, out string msg);
    }
}
