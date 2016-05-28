using L.S.Model.DatabaseModel.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DomainModel.Sys
{
    [NotMapped]
    public class SysRoleViewModel : SysRole
    {
        public List<string> SysRightsID { get; set; }
        public List<string> SysRightsName { get; set; }
    }
}
