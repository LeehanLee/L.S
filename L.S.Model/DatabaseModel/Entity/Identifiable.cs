using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DatabaseModel.Entity
{
    public abstract class AIdentifiable
    {
        [Required]
        [MaxLength(36)]
        public string ID { get; set; }
    }
}
