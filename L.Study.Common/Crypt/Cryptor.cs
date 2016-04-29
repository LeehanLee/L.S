using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace L.Study.Common.Crypt
{
    public class Cryptor
    {
        #region SHA1加密
        /// <summary>
        /// SHA1加密
        /// </summary>
        public static string SHA1Encrypt(string input, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            SHA1 sha1 = SHA1.Create();
            byte[] data = sha1.ComputeHash(encoding.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public static bool SHA1Verify(string input,string hash,Encoding encoding = null)
        {
            string hashOfInput = SHA1Encrypt(input, encoding);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);            
        }
        #endregion

        #region MD5加密、验证
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">需要加密的内容</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string MD5Encrypt(string input, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(encoding.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// MD5验证传入的密文是否经由传入的明文加密所得
        /// </summary>
        /// <param name="input">明文</param>
        /// <param name="hash">密文</param>
        /// <returns>密文是否由明文加密所得</returns>
        public static bool MD5Verify(string input, string hash, Encoding encoding = null)
        {            
            string hashOfInput = MD5Encrypt(input, encoding); // Create a StringComparer and compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);            
        }
        #endregion

        #region DES 加密解密
        /// <summary>
        /// DES 加密 注意:密钥必须为８位
        /// </summary>
        /// <param name="inputString">待加密字符串</param>
        /// <param name="encryptKey">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string DesEncrypt(string inputString, string encryptKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(inputString);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// DES 解密 注意:密钥必须为８位
        /// </summary>
        /// <param name="inputString">待解密字符串</param>
        /// <param name="decryptKey">密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string DesDecrypt(string inputString, string decryptKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = new Byte[inputString.Length];
            byKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(inputString);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
                
        }
        #endregion

    }
}
