using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common
{
    class CommonUtil
    {
    }
    public sealed class IdentityCreator
    {
        /*
         * var guid = Guid.NewGuid();         
         * guid.ToString("D")   10244798-9a34-4245-b1ef-9143f9b1e68a
         * guid.ToString("N")   102447989a344245b1ef9143f9b1e68a
         * guid.ToString("B")   {10244798-9a34-4245-b1ef-9143f9b1e68a}
         * guid.ToString("P")   (10244798-9a34-4245-b1ef-9143f9b1e68a)
         * 不区另大小写
         */
        public static string NextIdentity
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Guid.NewGuid().ToString("N").Substring(new Random().Next(0, 17), 15);
            }
        }
        public static int timestamp
        {
            get
            {
                return ((Int32)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            }
        }
    }
}
