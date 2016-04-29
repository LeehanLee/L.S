
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Service
{
    using L.S.Interface;
    using Model.DatabaseModel.Entity;
    using System.Data.SqlClient;
    public class RoleService: BaseService<SysRole>, IRoleService
    {
        public int RoleDelete(string ids,out string msg)
        {
            msg = "";
            msg = "";
            int readyCount = 0;
            var idArray = ids.Split(',');
            int totalCount = idArray.Length;
            foreach (var id in idArray)
            {
                var r = Find(id.Trim());
                if (r != null)
                {
                    if (r.Children.Count == 0)
                    {
                        if (r.AddBy == "init")
                        {
                            msg += "角色[" + r.Name+ "]为系统内置数据不允许删除<br />";
                        }
                        else
                        {
                            Remove(r);
                            SqlParameter param = new SqlParameter("@RoleID", r.ID);
                            ExecuteSql("DELETE FROM dbo.SysUserRole WHERE RoleID=@RoleID;DELETE FROM dbo.SysRoleRight WHERE RoleID=@RoleID;", out msg, param);
                            readyCount++;                            
                        }
                    }
                    else
                    {
                        var existRoleNames=string.Join(",", r.Children.Select(ch => ch.Name).ToArray());
                        msg += "角色[" + r.Name + "]存在下级角色["+ existRoleNames + "]，不允许删除<br />";
                    }
                }
                else
                {
                    msg += "未找到ID为" + id + "的角色<br />";
                }
            }
            if (readyCount == totalCount)
            {
                return SaveChanges(out msg);
            }
            else
            {
                return 0;
            }
        }
    }
}
