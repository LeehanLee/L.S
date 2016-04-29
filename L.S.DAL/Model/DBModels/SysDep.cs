using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.DAL.Model.DBModels
{
    public class SysDep : BaseModel, DbEnitty
    {
        public SysDep()
        {
            SysUsers = new HashSet<SysUser>();
        }
        public virtual ICollection<SysUser> SysUsers { get; set; }
    }
}
