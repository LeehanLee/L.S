using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace L.S.Model.DatabaseModel.Entity
{
    public abstract class BaseEntity : ANamedEntity
    {
        [MaxLength(36)]
        public string AddBy { get; set; }
        [MaxLength(20)]
        public string AddByName { get; set; }
        public DateTime AddDate { get; set; }
        [MaxLength(36)]
        public string UpdateBy { get; set; }
        [MaxLength(20)]
        public string UpdateByName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDel { get; set; }
    }
}
