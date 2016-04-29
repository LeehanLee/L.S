using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Service
{
    public class DepService : BaseService<SysDep>, IDepService
    {
        public int DepsDelete(string ids,out string msg)
        {
            msg = "";
            int readyCount = 0;
            var idArray = ids.Split(',');
            int totalCount = idArray.Length;
            foreach (var id in idArray)
            {
                var d=Find(id.Trim());
                if (d != null)
                {
                    if (d.Children.Count == 0)
                    {
                        if (d.AddBy != "init")
                        {
                            Remove(d);
                            readyCount++;
                        }
                        else
                        {
                            msg += "组织["+d.Name+"]为系统内置数据不允许删除<br />";
                        }
                    }
                    else
                    {
                        msg += "组织["+d.Name+ "]存在下级组织，不允许删除<br />";
                    }
                }
                else
                {
                    msg += "未找到ID为" + id+ "的组织<br />";
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
