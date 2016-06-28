using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L.S.Model.DatabaseModel.Entity;

namespace L.S.Model.DomainModel
{
    [NotMapped]
    public class InfoViewModel: DatabaseModel.Entity.Info
    {
        public string AddByName { get; set; }
        public string UpdateByName { get; set; }
        public string CategoryName { get; set; }
    }
}
