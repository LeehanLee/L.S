using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.DAL.Model.DBModels
{
    public class SysRole
    {
        public SysRole()
        {
            SysUser = new HashSet<SysUser>();
        }
        public virtual ICollection<SysUser> SysUser { get; set; }
    }
}
