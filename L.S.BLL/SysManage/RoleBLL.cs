using L.S.Interface;
using L.S.Interface.BLL;
using L.S.Model.DatabaseModel.Entity;
using L.S.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.BLL.SysManage
{
    public class RoleBLL : IRoleBLL
    {
        public IRoleRightService roleRightService;
        public IUserRoleService userRoleService;
        public RoleBLL(IRoleRightService _roleRightService, IUserRoleService _userRoleService)
        {
            roleRightService = _roleRightService;
            userRoleService = _userRoleService;
        }
        public bool SetRoleRights(string roleID, string SysRightsID, out string msg)
        {
            SqlParameter param = new SqlParameter("@RoleID", roleID);
            roleRightService.ExecuteSql("DELETE FROM dbo.SysRoleRight WHERE RoleID=@RoleID", out msg, param);
            if (!string.IsNullOrEmpty(SysRightsID))
            {
                SysRightsID.Split(',').ToList().ForEach(rightid =>
                {
                    if (!string.IsNullOrEmpty(rightid))
                    {
                        var roleRight = new SysRoleRight()
                        {
                            ID = IdentityCreator.NextIdentity,
                            RightID = rightid,
                            RoleID = roleID,
                        };
                        roleRightService.Add(roleRight);
                    }
                });
                return roleRightService.SaveChanges(out msg) > 0;
            }
            else
            {
                return true;
            }
        }
        public bool SetUserRoles(string userID, string roleIDs, out string msg)
        {
            SqlParameter param = new SqlParameter("@UserID", userID);
            userRoleService.ExecuteSql("DELETE FROM dbo.SysUserRole WHERE UserID=@UserID", out msg, param);
            if (!string.IsNullOrEmpty(roleIDs))
            {
                roleIDs.Split(',').ToList().ForEach(roileid =>
                {
                    if (!string.IsNullOrEmpty(roileid))
                    {
                        var userRole = new SysUserRole()
                        {
                            ID = IdentityCreator.NextIdentity,
                            UserID = userID,
                            RoleID = roileid,
                        };
                        userRoleService.Add(userRole);
                    }
                });
                return userRoleService.SaveChanges(out msg) > 0;
            }
            else
            {
                return true;
            }
        }
    }
}
