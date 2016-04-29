using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace L.Study.Console
{
    class AsyncTest
    {
        public const string url = "http://www.l.s.cn/test/GetResultAfterSleep10";
        public static async Task<string> GetStringAsync()
        {
            //System.Console.WriteLine(Thread.CurrentThread.Name + "  是否池化线程：" + Thread.CurrentThread.IsThreadPoolThread);
            System.Console.WriteLine("GetStringAsync is start");
            var client = new WebClient();
            var result = await client.DownloadStringTaskAsync(new Uri(url));
            System.Console.WriteLine("GetStringAsync is end");
            return result;
        }
        public static string GetString()
        {
            //Thread.CurrentThread.Name = "GetString所在线程";
            //System.Console.WriteLine(Thread.CurrentThread.Name+"  是否池化线程："+ Thread.CurrentThread.IsThreadPoolThread);
            var client = new WebClient();
            var result = client.DownloadString(url);
            return result;
        }
    }
}
