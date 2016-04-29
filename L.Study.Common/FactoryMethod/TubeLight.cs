using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.FactoryMethod
{
    public class TubeLight : Light 
    { 
        public  void TurnOn() 
        { 
            Console.WriteLine("Tube Light is Turned on"); 
        }
        public  void TurnOff()
        {
            Console.WriteLine("Tube Light is Turned off");
        }
    }
}
