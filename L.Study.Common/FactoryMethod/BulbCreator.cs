using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.FactoryMethod
{
    public class BulbCreator : Creator 
    { 
        public Light factory() 
        { 
            return new BulbLight(); 
        } 
    }
}
