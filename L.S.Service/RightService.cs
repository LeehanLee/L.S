using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using L.Study.Common.Cache;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Service
{
    /// <summary>
    /// 权限的服务类
    /// </summary>
    public class RightService: BaseService<SysRight>, IRightService
    {
        //public RightService(DbContext context) : base(context) { }
        public int RightsDelete(string ids, out string msg)
        {
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
                            msg += "权限[" + r.Name + "]为系统内置数据不允许删除<br />";
                        }
                        else {
                            Remove(r);
                            SqlParameter param = new SqlParameter("@RightID", r.ID);
                            ExecuteSql("DELETE FROM dbo.SysRoleRight WHERE RightID=@RightID;", out msg, param);
                            readyCount++;                            
                        }
                    }
                    else
                    {
                        var existRightNames = string.Join(",", r.Children.Select(ch => ch.Name).ToArray());
                        msg += "权限[" + r.Name + "]存在下级权限["+ existRightNames + "]，不允许删除<br />";
                    }
                }
                else
                {
                    msg += "未找到ID为" + id + "的权限<br />";
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

        public IList<SysRight> GetAllSysRights()
        {
            //var list = CacheMaker.IISCache.GetOrSetThenGet("all_sys_right", () =>
            //{                
                var result = GetList(r => r.IsAvailable && !r.IsDel);                
                return result;
            //});
            //return list;
        }

    }
}
