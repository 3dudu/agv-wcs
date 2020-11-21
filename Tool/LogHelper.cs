using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class LogHelper
    {
        private static Object lockerrobj = new Object();

        private static Object lockobj = new Object();
        private static Object locktrafficobj = new Object();

        private static Object lockreceiveobj = new Object();
        private static Object locksendobj = new Object();

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteErrorLog(Exception ex)
        {
            Task.Run(() =>
            {
                lock (lockerrobj)
                {
                    if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                    {
                        File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "ErrorLog" + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + ex.Message + ex.StackTrace + Environment.NewLine);
                    }

                }
            });
        }

        /// <summary>
        /// 记录运行日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(string msg)
        {
            Task.Run(() =>
            {
                lock (lockobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "Log" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// 记录交通管制信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteTrafficLog(string msg)
        {
            Task.Run(() =>
            {
                lock (locktrafficobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "Traffic" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// 记录接受AGV指令信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteReciveAGVMessLog(string msg)
        {
            Task.Run(() =>
            {
                lock (lockreceiveobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "ReciveAGVMess" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// 记录发送AGV信息日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteSendAGVMessLog(string msg)
        {
            Task.Run(() =>
            {
                lock (lockobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "SendAGVMess" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// 记录接受充电桩命令
        /// </summary>
        /// <param name="msg"></param>
        public static /*async*/ void WriteReciveChargeMessLog(string msg)
        {
            Task.Run(() =>
            {
                lock (lockobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "ReciveChargeMess" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }


        /// <summary>
        /// 记录发送充电桩命令
        /// </summary>
        /// <param name="msg"></param>
        public static /*async*/ void WriteSendChargeMessLog(string msg)
        {
            Task.Run(() =>
            {
                lock (locksendobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "SendChargeMess" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// 记录掉线日志
        /// </summary>
        /// <param name="msg"></param>
        public static /*async*/ void WriteOffLineLog(string msg)
        {
            Task.Run(() =>
            {
                lock (lockobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "OffLineLog" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// 记录充电日志
        /// </summary>
        /// <param name="msg"></param>
        public static /*async*/ void WriteAGVChargeLog(string msg)
        {
            //await Task.Factory.StartNew(() =>
            //{
            Task.Run(() =>
            {
                lock (lockobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "ChargeLog" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //});
            });
        }

        /// <summary>
        /// 记录所有AGV小车的报警信息
        /// </summary>
        /// <param name="msg"></param>
        public static /*async*/ void WriteAGVWarnMessLog(string msg)
        {
            Task.Run(() =>
            {
                lock (lockobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "AGVWarnMessLog" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// 记录任务生成日志
        /// </summary>
        public static /*async*/ void WriteCreatTaskLog(string msg)
        {
            Task.Run(() =>
            {
                lock (lockobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "TaskLog" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + ": " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// 记录按钮盒操作日志
        /// </summary>
        /// <param name="msg"></param>
        public static /*async*/ void WriteCallBoxLog(string msg)
        {
            Task.Run(() =>
            {
                lock (lockobj)
                {
                    try
                    {
                        if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\1.txt"))
                        {
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "CallBoxOperLog" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + ": " + msg + Environment.NewLine);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// 删除五天前的日志
        /// </summary>
        public static void DeleteLogFile()
        {
            try
            {
                string strFolderPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\";
                DirectoryInfo dyInfo = new DirectoryInfo(strFolderPath);
                //获取文件夹下所有的文件
                foreach (FileInfo feInfo in dyInfo.GetFiles())
                {
                    //判断文件日期是否小于五天前,是则删除
                    if (feInfo.CreationTime < DateTime.Today.AddDays(-2) && feInfo.Name.ToLower() != "1.txt")
                    {
                        try
                        {
                            feInfo.Delete();
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
