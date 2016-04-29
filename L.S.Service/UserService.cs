using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using System.Data.Entity;
using System.Data.SqlClient;
using L.Study.Common.Cache;

namespace L.S.Service
{
    public class UserService : BaseService<SysUser>,IUserService
    {
        public int UsersDelete(string ids, out string msg)
        {
            msg = "";
            int readyCount = 0;
            var idArray = ids.Split(',');
            int totalCount = idArray.Length;
            foreach (var id in idArray)
            {
                var u = Find(id.Trim());
                if (u != null)
                {
                    if (u.AddBy == "init")
                    {
                        msg += "用户[" + u.Name + "]为系统内置数据不允许删除<br />";
                    }
                    else
                    {
                        CacheMaker.RedisCache.Remove(u.ID);
                        Remove(u);                        
                        SqlParameter param = new SqlParameter("@UserID", u.ID);
                        ExecuteSql("DELETE FROM dbo.SysUserRole WHERE UserID=@UserID;", out msg, param);
                        readyCount++;
                    }                    
                }
                else
                {
                    msg += "未找到ID为" + id + "的用户<br />";
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
