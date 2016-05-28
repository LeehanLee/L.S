using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DatabaseModel.Entity
{
    public class SysRole:BaseEntity
    {
        public SysRole()
        {
            SysUserRoles = new HashSet<SysUserRole>();
            SysRoleRights = new HashSet<SysRoleRight>();
            Children = new HashSet<SysRole>();
        }
        [MaxLength(10)]
        public string ParentName { get; set; }
        [MaxLength(36)]
        public string ParentID { get; set; }
        [MaxLength(360)]
        public string RoleIDPath { get; set; }
        [MaxLength(100)]
        public string DefaultHomePath { get; set; }
        public int Level { get; set; }
        [MaxLength(360)]
        public string RoleNamePath { get; set; }
        [ForeignKey("ParentID")]
        public virtual SysRole Parent { get; set; }
        public virtual ICollection<SysUserRole> SysUserRoles { get; set; }
        public virtual ICollection<SysRoleRight> SysRoleRights { get; set; }
        public virtual ICollection <SysRole> Children { get; set; }
        public int SortNo { get; set; }
    }
}
