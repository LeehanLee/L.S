using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DatabaseModel.Entity
{
    public class SysDep : BaseEntity
    {
        public SysDep()
        {
            SysUsers = new HashSet<SysUser>();
            Children = new HashSet<SysDep>();
        }
        [MaxLength(20)]
        public string ParentName { get; set; }
        [MaxLength(36)]
        public string ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual SysDep Parent { get; set; }
        public string DepFullIDPath { get; set; }
        public string DepFullNamePath { get; set; }
        public virtual ICollection<SysUser> SysUsers { get; set; }
        public virtual ICollection<SysDep> Children { get; set; }
        public int SortNo { get; set; }
    }
}
