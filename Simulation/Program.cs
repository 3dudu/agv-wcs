using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using Simulation.SimulationSysForm;
using System.IO;
using ShilinetSoftVerify;
using DevExpress.XtraEditors;
using Simulation.Simulation3D;

namespace Simulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            string RemeberSkinFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\SkinFile.txt";
            string DefaultSkinName = "DevExpress Dark Style";
            if (File.Exists(RemeberSkinFilePath))
            { DefaultSkinName = File.ReadAllText(RemeberSkinFilePath); }
            if (!string.IsNullOrEmpty(DefaultSkinName))
            { UserLookAndFeel.Default.SetSkinStyle(DefaultSkinName); }
            //校验序列号
            //ReturnInfo opr = Verify.VerifyCarAmount(2);
            //if (opr.Result)
            //{
                Application.Run(new FrmMain());
            //}
            //else
            //{
            //    XtraMessageBox.Show(opr.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    Application.Exit();
            //}
        }
    }
}
