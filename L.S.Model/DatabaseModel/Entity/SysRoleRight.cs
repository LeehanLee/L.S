using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DatabaseModel.Entity
{
    public class SysRoleRight
    {
        [Required]
        [MaxLength(36)]
        public string ID { get; set; }        
        [Required]
        [MaxLength(36)]
        public string RoleID { get; set; }
        [Required]
        [MaxLength(36)]
        public string RightID { get; set; }
        [ForeignKey("RoleID")]
        public virtual SysRole SysRole { get; set; }
        [ForeignKey("RightID")]
        public virtual SysRight SysRight { get; set; }
    }
}
