using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DatabaseModel.Entity
{
    /// <summary>
    /// 类型的种类，比如资讯分类，其下会有很多资讯的分类
    /// </summary>
    public class CategoryType: BaseEntity
    {
        public CategoryType()
        {
            CategoryList = new HashSet<Category>();            
        }
        public int SortNo { get; set; }

        public virtual ICollection<Category> CategoryList { get; set; }
    }
}
