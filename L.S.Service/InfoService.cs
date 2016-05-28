using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Service
{
    public class InfoService: BaseService<Info>, IInfoService
    {
        public int InfoesDelete(string ids, out string msg)
        {
            msg = "";
            int readyCount = 0;
            var idArray = ids.Split(',');
            int totalCount = idArray.Length;
            foreach (var id in idArray)
            {
                var d = Find(id.Trim());
                if (d != null)
                {
                    Remove(d);
                    readyCount++;
                }
                else
                {
                    msg += "未找到ID为" + id + "的资讯<br />";
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
