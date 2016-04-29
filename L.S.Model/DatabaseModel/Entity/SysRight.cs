using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DatabaseModel.Entity
{
    public class SysRight : BaseEntity
    {
        public SysRight()
        {
            Children = new HashSet<SysRight>();
            SysRoleRights = new HashSet<SysRoleRight>();
        }
        [MaxLength(360)]
        public string RightIDPath { get; set; }
        [MaxLength(36)]
        public string ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual SysRight Parent { get; set; }
        public int RightLevel { get; set; }
        /// <summary>
        /// 动作，同步还是异步
        /// </summary>
        [MaxLength(20)]
        public string ActionType { get; set; }
        /// <summary>
        /// 位置，放在列表页上面还是放在列表页里面
        /// </summary>
        [MaxLength(20)]
        public string Position { get; set; }
        public int SortNo { get; set; }
        [MaxLength(100)]
        public string MenuUrl { get; set; }
        public virtual ICollection<SysRight> Children { get; set; }
        public virtual ICollection<SysRoleRight> SysRoleRights { get; set; }
    }
}
