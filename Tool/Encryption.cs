using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tools
{
    /// <summary>
    /// 加密解密类
    /// </summary>
   public class Encryption
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Pass"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string EncryPW(string Pass, string Key)
        {
            return DesEncrypt(Pass, Key);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strPass"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string DisEncryPW(string strPass, string Key)
        {
            return DesDecrypt(strPass, Key);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        private static string DesEncrypt(string encryptString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        private static string DesDecrypt(string decryptString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
    }
}
