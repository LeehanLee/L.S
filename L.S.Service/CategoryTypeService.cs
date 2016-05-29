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
    public class CategoryTypeService : BaseService<CategoryType>, ICategoryTypeService
    {
        public int CategoryTypesDelete(string ids, out string msg)
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
                    if (model.CategoryList.Count <= 0)
                    {
                        if (model.AddBy == "init")
                        {
                            msg += "分类类型[" + model.Name + "]为系统内置数据不允许删除<br />";
                        }
                        else
                        {
                            //msg += "暂未实现分类类型删除方法<br />";
                            //CacheMaker.RedisCache.Remove(u.ID);
                            Remove(model);
                            //SqlParameter param = new SqlParameter("@UserID", model.ID);
                            //ExecuteSql("DELETE FROM dbo.SysUserRole WHERE UserID=@UserID;", out msg, param);
                            readyCount++;
                        }
                    }
                    else
                    {
                        msg += "分类类型[" + model.Name + "]有下属分类不允许删除<br />";
                    }
                }
                else
                {
                    msg += "未找到ID为" + id + "的分类类型<br />";
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
