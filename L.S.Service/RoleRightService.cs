using L.S.Model.DatabaseModel.Entity;
using L.S.Interface;
using System.Data.Entity;

namespace L.S.Service
{
    public class RoleRightService:BaseService<SysRoleRight>, IRoleRightService
    {
        public RoleRightService(DbContext context) : base(context) { }
    }
}
