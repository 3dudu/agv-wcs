using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class OperatIniFile
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool WritePrivateProfileString(
 string lpAppName, string lpKeyName, string lpString, string lpFileName);
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetPrivateProfileString(
        string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString,
        int nSize, string lpFileName);

        public static void WriteIni(string Assesion, string Key,string Context,string FilePath)
        {
            try
            {
                WritePrivateProfileString(Assesion, Key, Context, FilePath);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static string ReadIni(string Assesion, string Key, string FilePath)
        {
            try
            {
                StringBuilder temp = new StringBuilder();
                GetPrivateProfileString(Assesion, Key, "", temp, 255, FilePath);
                return temp.ToString();
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
