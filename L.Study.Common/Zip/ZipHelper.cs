using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Zip
{
    class ZipHelper
    {
        /// <summary>
        /// zip压缩文件
        /// </summary>
        /// <param name="pathToPack">将要被压缩的文件夹（绝对路径）</param>
        /// <param name="packedToPath">压缩包放到哪个路径下，如路径不存在会自动创建</param>
        /// <param name="filename">压缩包的名字，要有后缀.zip</param>
        /// <param name="filter">排除文件</param>
        /// <returns></returns>
        public static bool Zip(string pathToPack, string packedToPath, string filename, string filter = "")
        {
            try
            {
                FastZip fz = new FastZip();
                fz.CreateEmptyDirectories = true;
                if (!Directory.Exists(packedToPath))
                {
                    Directory.CreateDirectory(packedToPath);
                }
                string filePath = Path.Combine(packedToPath, filename);
                fz.CreateZip(filePath, pathToPack, true, filter);
                fz = null;
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        
        /// <summary>
        /// zip解压文件
        /// </summary>
        /// <param name="unPackToPath">解压到哪个绝对路径下</param>
        /// <param name="packFilePath">要解压的文件的路径</param>
        /// <param name="packFileName">要解压的文件名</param>
        /// <returns></returns>
        public static bool UnZip(string unPackToPath, string packFilePath, string packFileName)
        {
            try
            {
                if (!Directory.Exists(unPackToPath))
                {
                    Directory.CreateDirectory(unPackToPath);
                }
                string filePath = Path.Combine(packFilePath, packFileName);
                ZipInputStream zstream = new ZipInputStream(File.OpenRead(filePath));
                ZipEntry entry;
                while ((entry = zstream.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(entry.Name))
                    {
                        string upath = Path.Combine(unPackToPath, entry.Name);
                        if (entry.IsDirectory && !Directory.Exists(upath))
                        {
                            Directory.CreateDirectory(upath);
                        }
                        else if (entry.Name.Contains("\\"))
                        {
                            string tmp_path = unPackToPath;
                            string[] sss = entry.Name.Split('\\');

                            int count = sss.Length;
                            for (int i = 0; i < count - 1; i++)
                            {
                                tmp_path = Path.Combine(tmp_path, sss[i]);
                                if (!Directory.Exists(tmp_path))
                                {
                                    Directory.CreateDirectory(tmp_path);
                                }
                            }
                            if (entry.CompressedSize > 0)
                            {
                                GenerateFile(zstream, upath);
                            }
                        }
                        else if (entry.CompressedSize > 0)
                        {
                            GenerateFile(zstream, upath);
                        }
                    }
                }
                zstream.Dispose();
                zstream.Close();
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 抽取出来的方法，解压时生成文件用
        /// </summary>
        /// <param name="zstream">输入流，不解释</param>
        /// <param name="upath">解压后文件的绝对路径与名字，如  D:\lihan\kejian\unpresstest\test2\resource\Asset\Image\7\yd2.png    D:\lihan\kejian\unpresstest\test2\index.html</param>
        private static void GenerateFile(ZipInputStream zstream, string upath)
        {
            FileStream fs = File.Create(upath);
            int size = 2048;
            byte[] data = new byte[size];
            while (true)
            {
                size = zstream.Read(data, 0, data.Length);
                if (size > 0)
                {
                    fs.Write(data, 0, size);
                }
                else
                {
                    break;
                }
            }
            fs.Dispose();
            fs.Close();
        }
    }
}
