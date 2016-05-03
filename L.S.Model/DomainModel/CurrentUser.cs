using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DomainModel
{
    public class CurrentUser
    {
        public string UserID { get; set; }
        public string LoginName { get; set; }
        public string DisplayName { get; set; }
        public string DepName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public string RolesID { get; set; }
        public string RolesName { get; set; }
        public string RightIDs { get; set; }
        public string HomePath { get; set; }
        
    }

}
