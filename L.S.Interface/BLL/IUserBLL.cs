using L.S.Model.DatabaseModel.Entity;
using L.S.Model.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Interface.BLL
{
    public interface IUserBLL
    {
        bool SignIn(SysUser Model, out string homePathOrMsg);
        bool SignOut();
        CurrentUser GetCurrentUser();
    }
}
