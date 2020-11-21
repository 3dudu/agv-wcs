using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class MD5Encrypt
    {
        public static string MD5Encrypt64(string txt)
        {
            string cl = txt;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }
    }
}
