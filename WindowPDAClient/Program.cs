using SQLServerOperator;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowPDAClient
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
            try
            {
                ConnectConfigTool.setDBase();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message + "请检查...");
                Application.Exit();
                return;
            }
            Application.Run(new FrmMain());
        }
    }
}
