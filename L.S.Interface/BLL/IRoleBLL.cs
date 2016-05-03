using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Interface.BLL
{
    public interface IRoleBLL
    {
        bool SetRoleRights(string roleID, string SysRightsID, out string msg);
        bool SetUserRoles(string userID, string roleIDs, out string msg);
    }
}
