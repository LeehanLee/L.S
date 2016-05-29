using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DatabaseModel.Entity
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Children = new HashSet<Category>();
        }
        public string CateTypeID { get; set; }        

        [MaxLength(36)]
        public string ParentID { get; set; }
        
        public string CategoryFullIDPath { get; set; }
        public string CategoryFullNamePath { get; set; }
        public int SortNo { get; set; }
        public int Level { get; set; }


        [ForeignKey("ParentID")]
        public virtual Category Parent { get; set; }

        [ForeignKey("CateTypeID")]
        public virtual CategoryType CategoryType { get; set; }

        public virtual ICollection<Category> Children { get; set; }
    }
}
