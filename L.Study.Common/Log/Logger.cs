using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Log
{
    public class Logger
    {
        private static Object thislock = new Object();
        /// <summary>
        /// 添加日志 copy by lihan,2015-12-28 21:48:39
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="filename">日志文件前缀名</param>
        /// <param name="directory">日志目录</param>
        /// <returns></returns>
        public static bool AddLog(string message, string filename = "", string logDirectory = "")
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (logDirectory != "")
            {
                directory += "\\" + logDirectory;
            }
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = Path.Combine(directory, filename == null ? string.Empty : filename + DateTime.Now.Date.ToString("yyyyMMdd") + ".txt");

            if (!System.IO.File.Exists(path))
            {

                FileStream fs1 = new FileStream(path, FileMode.Create, FileAccess.Write);//创建写入文件
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine("message: \r\n" + message + "\r\n" + System.DateTime.Now.ToString() + "\r\n");//开始写入值
                sw.Close();
                fs1.Close();
            }
            else
            {
                lock (thislock)
                {                    
                    StreamWriter sr = System.IO.File.AppendText(path);
                    sr.WriteLine("message: \r\n" + message + "\r\n" + System.DateTime.Now.ToString() + "\r\n");//开始写入值
                    sr.Close();
                }
            }
            return true;

        }
    }
}
