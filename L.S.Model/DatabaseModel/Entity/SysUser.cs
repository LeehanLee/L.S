using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace L.S.Model.DatabaseModel.Entity
{
    public class SysUser:BaseEntity
    {
        public SysUser()
        {
            SysUserRoles = new HashSet<SysUserRole>();
        }
        [Required]
        [MaxLength(36)]
        public string LoginName { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        
        [MaxLength(11)]
        public string Mobile { get; set; }
        [EmailAddress]
        [MaxLength(20)]
        public string Email { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        
        public Nullable<DateTime> Birthday { get; set; }
        [MaxLength(100)]
        public string Remark { get; set; }
        [MaxLength(36)]
        public string SysDepID { get; set; }
        [MaxLength(36)]
        public string SysDepName { get; set; }
        [ForeignKey("SysDepID")]
        public virtual SysDep SysDep { get; set; }
        public virtual ICollection<SysUserRole> SysUserRoles { get; set; }
    }
}
