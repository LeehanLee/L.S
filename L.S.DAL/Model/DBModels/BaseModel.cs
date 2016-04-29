using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.DAL.Model.DBModels
{
    abstract class BaseModel
    {
        public string ID { get; set; }
        public string AddBy { get; set; }
        public DateTime AddTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDel { get; set; }
    }
}
