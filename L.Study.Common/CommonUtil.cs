using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace L.S.Common
{
    public static class CommonUtil
    {
        /// <summary>
        /// 返回绝对路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static string GetMapPath(string path, string absolutePath = "")
        {
            if (HttpContext.Current == null)
            {

                path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                if (path.IndexOf(Path.VolumeSeparatorChar) > -1)
                {
                    return path;
                }
                return (string.IsNullOrEmpty(absolutePath) ? AppDomain.CurrentDomain.BaseDirectory : absolutePath).TrimEnd('\\') + "\\" + path.TrimStart('\\');
            }
            else
            {
                try
                {
                    return HttpContext.Current.Server.MapPath(path);
                }
                catch
                {
                }
            }
            return path;
        }

        #region 字符串处理公用方法
        /// <summary>
        /// 去掉HTML代码
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string ToNoHtml(this string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return string.Empty;
            }

            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(ndash);", "–", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(mdash);", "—", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(rdquo);", "”", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(ldquo);", "“", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpUtility.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        public static string ToMaxString(this string text, int l)
        {
            string r = text;
            if (!string.IsNullOrEmpty(text) && text.Length > l)
            {
                if (l > 3) { l = l - 3; }
                r = text.Substring(0, l) + "...";
            }
            return r;
        }
        #endregion

        #region 文件上传处理方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hfc"></param>
        /// <param name="fileName">保存文件的文件夹名</param>
        /// <param name="extFormat"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static List<string> SaveUploadFiles(HttpFileCollectionBase hfc, string fileName = "", List<string> extFormat = null, long maxLength = -1)
        {
            List<string> list = new List<string>();
            if (hfc != null && hfc.Count > 0)
            {
                //临时文件目录
                string fpath = "/Upload/" + (string.IsNullOrEmpty(fileName) ? "temp" : fileName) + "/" + DateTime.Now.ToString("yyyyMMdd") + "/"; ;
                string path = CommonUtil.GetMapPath(fpath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string ext = string.Empty;
                string fname = string.Empty;
                foreach (string fn in hfc)
                {
                    try
                    {
                        var f = hfc[fn];
                        ext = f.FileName.Substring(f.FileName.LastIndexOf("."), f.FileName.Length - f.FileName.LastIndexOf("."));
                        //不为指定格式或超过大小的数据不做处理
                        if ((extFormat == null || (extFormat.Count > 0 && extFormat.Contains(ext))) && (maxLength == -1 || ((maxLength > 0 && f.ContentLength <= maxLength))))
                        {
                            fname = f.FileName;
                            if (fname.LastIndexOf("/") > -1)
                            {
                                fname = fname.Substring(fname.LastIndexOf("/") + 1);
                            }
                            fname = "[" + fname + "]" + IdentityCreator.NextIdentity + ext;
                            f.SaveAs(path + fname);
                            list.Add(fpath + fname);
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
            return list;
        }

        /// <summary>
        /// 删除需要删除的图片文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="successCount"></param>
        /// <param name="notExistCount"></param>
        /// <param name="msg"></param>
        public static void RemoveFiles(string data, out int successCount, out int notExistCount, out string msg)
        {
            msg = "";
            successCount = 0;
            notExistCount = 0;
            try
            {
                var paths = data.Split('|');
                foreach (string rpath in paths)
                {
                    if (!string.IsNullOrEmpty(rpath))
                    {
                        string path = GetMapPath(rpath);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                            successCount++;
                        }
                        else
                        {
                            notExistCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg += ex.Message;
            }
        }
        #endregion
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
