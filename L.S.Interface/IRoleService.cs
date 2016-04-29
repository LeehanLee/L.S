using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Interface
{
    using L.S.Model.DatabaseModel.Entity;
    public interface IRoleService: IBaseService<SysRole>
    {
        int RoleDelete(string ids, out string msg);
    }
}
