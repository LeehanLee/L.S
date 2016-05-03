using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.FTP
{
    public class Ftp
    {
        public string ftpUserID { get; set; }
        public string ftpPassword { get; set; }
        public string ftpServerIP { get; set; }

        public Ftp()
        {
            ftpUserID = System.Configuration.ConfigurationManager.AppSettings["ftp_account"];
            ftpPassword = System.Configuration.ConfigurationManager.AppSettings["ftp_password"];
            ftpServerIP = System.Configuration.ConfigurationManager.AppSettings["ftp_services"];
        }

        public Ftp(string _ftpServerIP, string _ftpUserID, string _ftpPassword)
        {
            ftpUserID = _ftpUserID;
            ftpPassword = _ftpPassword;
            ftpServerIP = _ftpServerIP;
        }

        /// <summary>
        /// 上传文件至ftp服务器
        /// </summary>
        /// <param name="ftpPath">ftp服务器中的路径 eg:  /dir1/dir2/ </param>
        /// <param name="localFile">本地文件绝对路径</param>
        /// <returns></returns>
        public Boolean FtpUpload(string ftpPath, string localFile)
        {
            //检查目录是否存在，不存在创建  
            FtpCheckDirectoryExist(ftpPath);
            FileInfo fi = new FileInfo(localFile);
            FileStream fs = fi.OpenRead();
            long length = fs.Length;
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://" + ftpServerIP + ftpPath + fi.Name);
            req.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.ContentLength = length;
            req.Timeout = 10 * 1000;
            try
            {
                Stream stream = req.GetRequestStream();
                int BufferLength = 2048; //2K    
                byte[] b = new byte[BufferLength];
                int i;
                while ((i = fs.Read(b, 0, BufferLength)) > 0)
                {
                    stream.Write(b, 0, i);
                }
                stream.Close();
                stream.Dispose();
            }
            catch
            {
                return false;
            }
            finally
            {
                fs.Close();
                req.Abort();
            }
            req.Abort();
            return true;
        }
        #region MyRegion
        //判断文件的目录是否存,不存则创建  
        private void FtpCheckDirectoryExist(string destFilePath)
        {
            string fullDir = FtpParseDirectory(destFilePath);
            string[] dirs = fullDir.Split('/');
            string curDir = "/";
            for (int i = 0; i < dirs.Length; i++)
            {
                string dir = dirs[i];
                //如果是以/开始的路径,第一个为空    
                if (dir != null && dir.Length > 0)
                {
                    try
                    {
                        curDir += dir + "/";
                        FtpMakeDir(curDir);
                    }
                    catch (Exception)
                    { }
                }
            }
        }
        private string FtpParseDirectory(string destFilePath)
        {
            return destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
        }
        //创建目录  
        private Boolean FtpMakeDir(string localFile)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://" + ftpServerIP + localFile);
            req.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }
        #endregion
    }
}
