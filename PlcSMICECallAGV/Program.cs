using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PlcSMICECallAGV
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            // Application.Run(new FrmMain()/*new FrmAGVRevSIMZECallServer()*/);
            SetSelfStarting();
            Application.Run(new FrmAGVRevSIMZECallServer());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            File.AppendAllText("ExceptionLog.txt",
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ")
                                + "发生了 Application_ThreadException " + Environment.NewLine
                                + e.Exception.Message + Environment.NewLine
                                + e.Exception.StackTrace + Environment.NewLine
                                + e.Exception.Source + Environment.NewLine
                               );
            HandleException(e.Exception);
        }


        private static void HandleException(Exception exception)
        {

            EventLog log = null;
            try
            {

                string source = Application.ProductName;
                log = new EventLog("PLC");
                //  首先应判断日志来源是否存在，一个日志来源只能同时与一个事件绑定s
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, "PLC");
                log.Source = source;
                log.WriteEntry(exception.ToString(), EventLogEntryType.Error);

            }
            catch
            {

            }
        }


        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception;
            exception = e.ExceptionObject as Exception;
            File.AppendAllText("ExceptionLog.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ")
                               + "发生了 CurrentDomain_UnhandledException " + Environment.NewLine
                               + exception.Message + Environment.NewLine
                               + exception.StackTrace + Environment.NewLine
                               + exception.Source + Environment.NewLine
                              );
            Console.ReadLine();
            if (exception == null)
            {
                // 这里是非托管异常，无法处理
                return;
            }
        }
        /// <summary>
        /// 设置开机自启动__写入注册表
        /// </summary>
        /// <returns></returns>
        public static void SetSelfStarting()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                RegistryKey reg = null;
                try
                {
                    string ShortFileName = Application.ProductName;
                    string strAssName = Application.StartupPath + @"\" + Application.ProductName + @".exe";
                    if (IntPtr.Size == 4)
                    { reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true); }
                    else
                    { reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Run", true); }
                    if (reg == null)
                    {
                        if (IntPtr.Size == 4)
                        { reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"); }
                        else
                        { reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Run"); }
                    }
                    if (reg.GetValue(ShortFileName) != null)
                    { reg.DeleteValue(ShortFileName); }
                    reg.SetValue(ShortFileName, strAssName);
                }
                catch (Exception ex)
                {
                    if (reg != null)
                        reg.Close();
                    System.Environment.Exit(System.Environment.ExitCode);
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    if (reg != null)
                        reg.Close();
                }
            }
        }

    }
}
