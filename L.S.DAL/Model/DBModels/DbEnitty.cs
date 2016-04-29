using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.DAL.Model.DBModels
{
    interface DbEnitty
    {
        string ID { get; set; }
        Nullable<DateTime> AddTime { get; set; }
        Nullable<DateTime> UpdateTime { get; set; }
    }
}
