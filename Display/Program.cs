using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.IO;

namespace Display
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
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");
            string DefaultSkinName = "";
            string RemeberSkinFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\SkinFile.txt";
            if (File.Exists(RemeberSkinFilePath))
            { DefaultSkinName = File.ReadAllText(RemeberSkinFilePath); }
            if (!string.IsNullOrEmpty(DefaultSkinName))
            { UserLookAndFeel.Default.SetSkinStyle(DefaultSkinName); }
            Application.Run(new FrmSecondDMain());
        }
    }
}
