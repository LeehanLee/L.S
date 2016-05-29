using L.S.Interface;
using L.S.Model.DatabaseModel.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Service
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public int CategorysDelete(string ids, out string msg)
        {
            msg = "";
            int readyCount = 0;
            var idArray = ids.Split(',');
            int totalCount = idArray.Length;
            foreach (var id in idArray)
            {
                var model = Find(id.Trim());
                if (model != null)
                {
                    if (model.Children.Count <= 0)
                    {
                        if (model.AddBy == "init")
                        {
                            msg += "分类[" + model.Name + "]为系统内置数据不允许删除<br />";
                        }
                        else
                        {                            
                            //CacheMaker.RedisCache.Remove(u.ID);
                            Remove(model);
                            //SqlParameter param = new SqlParameter("@UserID", model.ID);
                            //ExecuteSql("DELETE FROM dbo.SysUserRole WHERE UserID=@UserID;", out msg, param);
                            readyCount++;
                        }
                    }
                    else
                    {
                        msg += "分类[" + model.Name + "]存在下级分类不允许删除<br />";
                    }
                    
                }
                else
                {
                    msg += "未找到ID为" + id + "的分类<br />";
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
