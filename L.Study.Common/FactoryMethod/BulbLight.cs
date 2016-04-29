using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.FactoryMethod
{
    public class BulbLight : Light 
    { 
        public  void TurnOn() 
        { 
            Console.WriteLine("Bulb Light is Turned on"); 
        }
        public  void TurnOff()
        {
            Console.WriteLine("Bulb Light is Turned off");
        }
    }
}
