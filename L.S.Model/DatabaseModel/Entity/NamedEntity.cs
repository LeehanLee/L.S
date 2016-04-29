using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace L.S.Model.DatabaseModel.Entity
{
    public abstract class ANamedEntity : AIdentifiable
    {
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
    }
}
