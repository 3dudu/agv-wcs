using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
//using AGVDAccess;
//using AGVModel;
using DevExpress.Utils;
using System.IO;
using CommonTools;
using DXAGVClient;
//using DXAGVClient.FormSystem;

namespace RCSEditTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CommonTools.Tracing.AddId(Tracing.TracePaint);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            string RemeberSkinFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\SkinFile.txt";
            string DefaultSkinName = "VS2010";
            if (File.Exists(RemeberSkinFilePath))
            { DefaultSkinName = File.ReadAllText(RemeberSkinFilePath); }
            if (!string.IsNullOrEmpty(DefaultSkinName))
            { UserLookAndFeel.Default.SetSkinStyle(DefaultSkinName); }
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "view.txt"))
            {
                Application.Run(new FrmMainView());
            }
            else
            {
                using (FrmLogin frm = new FrmLogin())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    { Application.Run(new FrmMain()); }
                }
            }
            
            CommonTools.Tracing.Terminate();
        }
    }
}
