using Model.Comoon;

namespace DXAGVClient.FormSystem
{
    partial class FrmSysAut
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSysAut));
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.newCategory = new DevExpress.XtraBars.BarButtonItem();
            this.delCategory = new DevExpress.XtraBars.BarButtonItem();
            this.newUser = new DevExpress.XtraBars.BarButtonItem();
            this.delUser = new DevExpress.XtraBars.BarButtonItem();
            this.btnOK = new DevExpress.XtraBars.BarButtonItem();
            this.Cancel = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gcCategory = new DevExpress.XtraGrid.GridControl();
            this.bsCategory = new System.Windows.Forms.BindingSource();
            this.gvCategory = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.CategoryCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CategoryName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcOperButton = new DevExpress.XtraGrid.GridControl();
            this.cmSelect = new System.Windows.Forms.ContextMenuStrip();
            this.AllSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.CancelSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.bsSysOperButton = new System.Windows.Forms.BindingSource();
            this.gvOperButton = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ButtonChoose = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ButtonType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ButtonCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUser = new DevExpress.XtraGrid.GridControl();
            this.bsUser = new System.Windows.Forms.BindingSource();
            this.gvUser = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.UserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PassWord = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtUerPassWord = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.UserName = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection48)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcOperButton)).BeginInit();
            this.cmSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSysOperButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOperButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUerPassWord)).BeginInit();
            this.SuspendLayout();
            // 
            // imageCollection24
            // 
            this.imageCollection24.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection24.ImageStream")));
            this.imageCollection24.Images.SetKeyName(0, "-.png");
            this.imageCollection24.Images.SetKeyName(1, "+.png");
            this.imageCollection24.Images.SetKeyName(2, "add-favorite.png");
            this.imageCollection24.Images.SetKeyName(3, "add-printer.png");
            this.imageCollection24.Images.SetKeyName(4, "alarm.png");
            this.imageCollection24.Images.SetKeyName(5, "alert.png");
            this.imageCollection24.Images.SetKeyName(6, "announce.png");
            this.imageCollection24.Images.SetKeyName(7, "back.png");
            this.imageCollection24.Images.SetKeyName(8, "back2.png");
            this.imageCollection24.Images.SetKeyName(9, "backward.png");
            this.imageCollection24.Images.SetKeyName(10, "basket.png");
            this.imageCollection24.Images.SetKeyName(11, "binoculars.png");
            this.imageCollection24.Images.SetKeyName(12, "burn.png");
            this.imageCollection24.Images.SetKeyName(13, "cal.png");
            this.imageCollection24.Images.SetKeyName(14, "calculator.png");
            this.imageCollection24.Images.SetKeyName(15, "cash-register.png");
            this.imageCollection24.Images.SetKeyName(16, "cc.png");
            this.imageCollection24.Images.SetKeyName(17, "cd.png");
            this.imageCollection24.Images.SetKeyName(18, "config.png");
            this.imageCollection24.Images.SetKeyName(19, "connect.png");
            this.imageCollection24.Images.SetKeyName(20, "construction.png");
            this.imageCollection24.Images.SetKeyName(21, "contacts.png");
            this.imageCollection24.Images.SetKeyName(22, "copy.png");
            this.imageCollection24.Images.SetKeyName(23, "cut.png");
            this.imageCollection24.Images.SetKeyName(24, "database.png");
            this.imageCollection24.Images.SetKeyName(25, "delete.png");
            this.imageCollection24.Images.SetKeyName(26, "delete-folder.png");
            this.imageCollection24.Images.SetKeyName(27, "down3.png");
            this.imageCollection24.Images.SetKeyName(28, "download.png");
            this.imageCollection24.Images.SetKeyName(29, "edit.png");
            this.imageCollection24.Images.SetKeyName(30, "email.png");
            this.imageCollection24.Images.SetKeyName(31, "email2.png");
            this.imageCollection24.Images.SetKeyName(32, "export.png");
            this.imageCollection24.Images.SetKeyName(33, "export1.png");
            this.imageCollection24.Images.SetKeyName(34, "faq.png");
            this.imageCollection24.Images.SetKeyName(35, "favorite.png");
            this.imageCollection24.Images.SetKeyName(36, "file.png");
            this.imageCollection24.Images.SetKeyName(37, "folder.png");
            this.imageCollection24.Images.SetKeyName(38, "forward.png");
            this.imageCollection24.Images.SetKeyName(39, "front.png");
            this.imageCollection24.Images.SetKeyName(40, "front1.png");
            this.imageCollection24.Images.SetKeyName(41, "fulltrash.png");
            this.imageCollection24.Images.SetKeyName(42, "hd.png");
            this.imageCollection24.Images.SetKeyName(43, "hd1.png");
            this.imageCollection24.Images.SetKeyName(44, "help.png");
            this.imageCollection24.Images.SetKeyName(45, "home.png");
            this.imageCollection24.Images.SetKeyName(46, "image.png");
            this.imageCollection24.Images.SetKeyName(47, "import.png");
            this.imageCollection24.Images.SetKeyName(48, "import2.png");
            this.imageCollection24.Images.SetKeyName(49, "info.png");
            this.imageCollection24.Images.SetKeyName(50, "install.png");
            this.imageCollection24.Images.SetKeyName(51, "locked.png");
            this.imageCollection24.Images.SetKeyName(52, "music.png");
            this.imageCollection24.Images.SetKeyName(53, "network.png");
            this.imageCollection24.Images.SetKeyName(54, "new-folder.png");
            this.imageCollection24.Images.SetKeyName(55, "opened.png");
            this.imageCollection24.Images.SetKeyName(56, "open-folder.png");
            this.imageCollection24.Images.SetKeyName(57, "paste.png");
            this.imageCollection24.Images.SetKeyName(58, "photo.png");
            this.imageCollection24.Images.SetKeyName(59, "police.png");
            this.imageCollection24.Images.SetKeyName(60, "printer.png");
            this.imageCollection24.Images.SetKeyName(61, "public.png");
            this.imageCollection24.Images.SetKeyName(62, "radar.png");
            this.imageCollection24.Images.SetKeyName(63, "save.png");
            this.imageCollection24.Images.SetKeyName(64, "save-as.png");
            this.imageCollection24.Images.SetKeyName(65, "screen-capture.png");
            this.imageCollection24.Images.SetKeyName(66, "search.png");
            this.imageCollection24.Images.SetKeyName(67, "send.png");
            this.imageCollection24.Images.SetKeyName(68, "software 2.png");
            this.imageCollection24.Images.SetKeyName(69, "software update.png");
            this.imageCollection24.Images.SetKeyName(70, "software.png");
            this.imageCollection24.Images.SetKeyName(71, "sound.png");
            this.imageCollection24.Images.SetKeyName(72, "statics.png");
            this.imageCollection24.Images.SetKeyName(73, "statics-1.png");
            this.imageCollection24.Images.SetKeyName(74, "statics-2.png");
            this.imageCollection24.Images.SetKeyName(75, "stop.png");
            this.imageCollection24.Images.SetKeyName(76, "support.png");
            this.imageCollection24.Images.SetKeyName(77, "switcher.png");
            this.imageCollection24.Images.SetKeyName(78, "trash.png");
            this.imageCollection24.Images.SetKeyName(79, "truck.png");
            this.imageCollection24.Images.SetKeyName(80, "up4.png");
            this.imageCollection24.Images.SetKeyName(81, "update.png");
            this.imageCollection24.Images.SetKeyName(82, "upload.png");
            this.imageCollection24.Images.SetKeyName(83, "user.png");
            this.imageCollection24.Images.SetKeyName(84, "uses.png");
            this.imageCollection24.Images.SetKeyName(85, "viewi-pr.png");
            this.imageCollection24.Images.SetKeyName(86, "web.png");
            this.imageCollection24.Images.SetKeyName(87, "window.png");
            this.imageCollection24.Images.SetKeyName(88, "window-2.png");
            this.imageCollection24.Images.SetKeyName(89, "zoom-.png");
            this.imageCollection24.Images.SetKeyName(90, "zoom+.png");
            this.imageCollection24.Images.SetKeyName(91, "alarm.png");
            this.imageCollection24.Images.SetKeyName(92, "bcir1.jpg");
            this.imageCollection24.Images.SetKeyName(93, "bcir2.jpg");
            this.imageCollection24.Images.SetKeyName(94, "break.jpg");
            this.imageCollection24.Images.SetKeyName(95, "cir1.jpg");
            this.imageCollection24.Images.SetKeyName(96, "cir12.jpg");
            this.imageCollection24.Images.SetKeyName(97, "cir123.jpg");
            this.imageCollection24.Images.SetKeyName(98, "cir132.jpg");
            this.imageCollection24.Images.SetKeyName(99, "copy.png");
            this.imageCollection24.Images.SetKeyName(100, "database.png");
            this.imageCollection24.Images.SetKeyName(101, "delete.png");
            this.imageCollection24.Images.SetKeyName(102, "edit.png");
            this.imageCollection24.Images.SetKeyName(103, "expand.jpg");
            this.imageCollection24.Images.SetKeyName(104, "hand1.jpg");
            this.imageCollection24.Images.SetKeyName(105, "hand2.jpg");
            this.imageCollection24.Images.SetKeyName(106, "landmark.jpg");
            this.imageCollection24.Images.SetKeyName(107, "line.jpg");
            this.imageCollection24.Images.SetKeyName(108, "Login_Exit.png");
            this.imageCollection24.Images.SetKeyName(109, "move.jpg");
            this.imageCollection24.Images.SetKeyName(110, "radar.png");
            this.imageCollection24.Images.SetKeyName(111, "redo.jpg");
            this.imageCollection24.Images.SetKeyName(112, "save.png");
            this.imageCollection24.Images.SetKeyName(113, "select.jpg");
            this.imageCollection24.Images.SetKeyName(114, "support.png");
            this.imageCollection24.Images.SetKeyName(115, "truck.png");
            this.imageCollection24.Images.SetKeyName(116, "undo.jpg");
            this.imageCollection24.Images.SetKeyName(117, "user.png");
            // 
            // imageCollection32
            // 
            this.imageCollection32.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection32.ImageStream")));
            this.imageCollection32.Images.SetKeyName(0, "-.png");
            this.imageCollection32.Images.SetKeyName(1, "+.png");
            this.imageCollection32.Images.SetKeyName(2, "add-favorite.png");
            this.imageCollection32.Images.SetKeyName(3, "add-printer.png");
            this.imageCollection32.Images.SetKeyName(4, "alarm.png");
            this.imageCollection32.Images.SetKeyName(5, "alert.png");
            this.imageCollection32.Images.SetKeyName(6, "announce.png");
            this.imageCollection32.Images.SetKeyName(7, "back.png");
            this.imageCollection32.Images.SetKeyName(8, "back2.png");
            this.imageCollection32.Images.SetKeyName(9, "backward.png");
            this.imageCollection32.Images.SetKeyName(10, "basket.png");
            this.imageCollection32.Images.SetKeyName(11, "binoculars.png");
            this.imageCollection32.Images.SetKeyName(12, "burn.png");
            this.imageCollection32.Images.SetKeyName(13, "cal.png");
            this.imageCollection32.Images.SetKeyName(14, "calculator.png");
            this.imageCollection32.Images.SetKeyName(15, "cash-register.png");
            this.imageCollection32.Images.SetKeyName(16, "cc.png");
            this.imageCollection32.Images.SetKeyName(17, "cd.png");
            this.imageCollection32.Images.SetKeyName(18, "config.png");
            this.imageCollection32.Images.SetKeyName(19, "connect.png");
            this.imageCollection32.Images.SetKeyName(20, "construction.png");
            this.imageCollection32.Images.SetKeyName(21, "contacts.png");
            this.imageCollection32.Images.SetKeyName(22, "copy.png");
            this.imageCollection32.Images.SetKeyName(23, "cut.png");
            this.imageCollection32.Images.SetKeyName(24, "database.png");
            this.imageCollection32.Images.SetKeyName(25, "delete.png");
            this.imageCollection32.Images.SetKeyName(26, "delete-folder.png");
            this.imageCollection32.Images.SetKeyName(27, "down3.png");
            this.imageCollection32.Images.SetKeyName(28, "download.png");
            this.imageCollection32.Images.SetKeyName(29, "edit.png");
            this.imageCollection32.Images.SetKeyName(30, "email.png");
            this.imageCollection32.Images.SetKeyName(31, "email2.png");
            this.imageCollection32.Images.SetKeyName(32, "export.png");
            this.imageCollection32.Images.SetKeyName(33, "export1.png");
            this.imageCollection32.Images.SetKeyName(34, "faq.png");
            this.imageCollection32.Images.SetKeyName(35, "favorite.png");
            this.imageCollection32.Images.SetKeyName(36, "file.png");
            this.imageCollection32.Images.SetKeyName(37, "folder.png");
            this.imageCollection32.Images.SetKeyName(38, "forward.png");
            this.imageCollection32.Images.SetKeyName(39, "front.png");
            this.imageCollection32.Images.SetKeyName(40, "front1.png");
            this.imageCollection32.Images.SetKeyName(41, "fulltrash.png");
            this.imageCollection32.Images.SetKeyName(42, "hd.png");
            this.imageCollection32.Images.SetKeyName(43, "hd1.png");
            this.imageCollection32.Images.SetKeyName(44, "help.png");
            this.imageCollection32.Images.SetKeyName(45, "home.png");
            this.imageCollection32.Images.SetKeyName(46, "image.png");
            this.imageCollection32.Images.SetKeyName(47, "import.png");
            this.imageCollection32.Images.SetKeyName(48, "import2.png");
            this.imageCollection32.Images.SetKeyName(49, "info.png");
            this.imageCollection32.Images.SetKeyName(50, "install.png");
            this.imageCollection32.Images.SetKeyName(51, "locked.png");
            this.imageCollection32.Images.SetKeyName(52, "music.png");
            this.imageCollection32.Images.SetKeyName(53, "network.png");
            this.imageCollection32.Images.SetKeyName(54, "new-folder.png");
            this.imageCollection32.Images.SetKeyName(55, "opened.png");
            this.imageCollection32.Images.SetKeyName(56, "open-folder.png");
            this.imageCollection32.Images.SetKeyName(57, "paste.png");
            this.imageCollection32.Images.SetKeyName(58, "photo.png");
            this.imageCollection32.Images.SetKeyName(59, "police.png");
            this.imageCollection32.Images.SetKeyName(60, "printer.png");
            this.imageCollection32.Images.SetKeyName(61, "public.png");
            this.imageCollection32.Images.SetKeyName(62, "radar.png");
            this.imageCollection32.Images.SetKeyName(63, "save.png");
            this.imageCollection32.Images.SetKeyName(64, "save-as.png");
            this.imageCollection32.Images.SetKeyName(65, "screen-capture.png");
            this.imageCollection32.Images.SetKeyName(66, "search.png");
            this.imageCollection32.Images.SetKeyName(67, "send.png");
            this.imageCollection32.Images.SetKeyName(68, "software 2.png");
            this.imageCollection32.Images.SetKeyName(69, "software update.png");
            this.imageCollection32.Images.SetKeyName(70, "software.png");
            this.imageCollection32.Images.SetKeyName(71, "sound.png");
            this.imageCollection32.Images.SetKeyName(72, "statics.png");
            this.imageCollection32.Images.SetKeyName(73, "statics-1.png");
            this.imageCollection32.Images.SetKeyName(74, "statics-2.png");
            this.imageCollection32.Images.SetKeyName(75, "stop.png");
            this.imageCollection32.Images.SetKeyName(76, "support.png");
            this.imageCollection32.Images.SetKeyName(77, "switcher.png");
            this.imageCollection32.Images.SetKeyName(78, "trash.png");
            this.imageCollection32.Images.SetKeyName(79, "truck.png");
            this.imageCollection32.Images.SetKeyName(80, "up4.png");
            this.imageCollection32.Images.SetKeyName(81, "update.png");
            this.imageCollection32.Images.SetKeyName(82, "upload.png");
            this.imageCollection32.Images.SetKeyName(83, "user.png");
            this.imageCollection32.Images.SetKeyName(84, "uses.png");
            this.imageCollection32.Images.SetKeyName(85, "viewi-pr.png");
            this.imageCollection32.Images.SetKeyName(86, "web.png");
            this.imageCollection32.Images.SetKeyName(87, "window.png");
            this.imageCollection32.Images.SetKeyName(88, "window-2.png");
            this.imageCollection32.Images.SetKeyName(89, "zoom-.png");
            this.imageCollection32.Images.SetKeyName(90, "zoom+.png");
            this.imageCollection32.Images.SetKeyName(91, "alarm.png");
            this.imageCollection32.Images.SetKeyName(92, "bcir1.jpg");
            this.imageCollection32.Images.SetKeyName(93, "bcir2.jpg");
            this.imageCollection32.Images.SetKeyName(94, "break.jpg");
            this.imageCollection32.Images.SetKeyName(95, "cir1.jpg");
            this.imageCollection32.Images.SetKeyName(96, "cir12.jpg");
            this.imageCollection32.Images.SetKeyName(97, "cir123.jpg");
            this.imageCollection32.Images.SetKeyName(98, "cir132.jpg");
            this.imageCollection32.Images.SetKeyName(99, "copy.png");
            this.imageCollection32.Images.SetKeyName(100, "database.png");
            this.imageCollection32.Images.SetKeyName(101, "delete.png");
            this.imageCollection32.Images.SetKeyName(102, "edit.png");
            this.imageCollection32.Images.SetKeyName(103, "expand.jpg");
            this.imageCollection32.Images.SetKeyName(104, "hand1.jpg");
            this.imageCollection32.Images.SetKeyName(105, "hand2.jpg");
            this.imageCollection32.Images.SetKeyName(106, "landmark.jpg");
            this.imageCollection32.Images.SetKeyName(107, "line.jpg");
            this.imageCollection32.Images.SetKeyName(108, "Login_Exit.png");
            this.imageCollection32.Images.SetKeyName(109, "move.jpg");
            this.imageCollection32.Images.SetKeyName(110, "radar.png");
            this.imageCollection32.Images.SetKeyName(111, "redo.jpg");
            this.imageCollection32.Images.SetKeyName(112, "save.png");
            this.imageCollection32.Images.SetKeyName(113, "select.jpg");
            this.imageCollection32.Images.SetKeyName(114, "support.png");
            this.imageCollection32.Images.SetKeyName(115, "truck.png");
            this.imageCollection32.Images.SetKeyName(116, "undo.jpg");
            this.imageCollection32.Images.SetKeyName(117, "user.png");
            // 
            // imageCollection48
            // 
            this.imageCollection48.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection48.ImageStream")));
            this.imageCollection48.Images.SetKeyName(0, "-.png");
            this.imageCollection48.Images.SetKeyName(1, "+.png");
            this.imageCollection48.Images.SetKeyName(2, "add-favorite.png");
            this.imageCollection48.Images.SetKeyName(3, "add-printer.png");
            this.imageCollection48.Images.SetKeyName(4, "alarm.png");
            this.imageCollection48.Images.SetKeyName(5, "alert.png");
            this.imageCollection48.Images.SetKeyName(6, "announce.png");
            this.imageCollection48.Images.SetKeyName(7, "back.png");
            this.imageCollection48.Images.SetKeyName(8, "back2.png");
            this.imageCollection48.Images.SetKeyName(9, "backward.png");
            this.imageCollection48.Images.SetKeyName(10, "basket.png");
            this.imageCollection48.Images.SetKeyName(11, "binoculars.png");
            this.imageCollection48.Images.SetKeyName(12, "burn.png");
            this.imageCollection48.Images.SetKeyName(13, "cal.png");
            this.imageCollection48.Images.SetKeyName(14, "calculator.png");
            this.imageCollection48.Images.SetKeyName(15, "cash-register.png");
            this.imageCollection48.Images.SetKeyName(16, "cc.png");
            this.imageCollection48.Images.SetKeyName(17, "cd.png");
            this.imageCollection48.Images.SetKeyName(18, "config.png");
            this.imageCollection48.Images.SetKeyName(19, "connect.png");
            this.imageCollection48.Images.SetKeyName(20, "construction.png");
            this.imageCollection48.Images.SetKeyName(21, "contacts.png");
            this.imageCollection48.Images.SetKeyName(22, "copy.png");
            this.imageCollection48.Images.SetKeyName(23, "cut.png");
            this.imageCollection48.Images.SetKeyName(24, "database.png");
            this.imageCollection48.Images.SetKeyName(25, "delete.png");
            this.imageCollection48.Images.SetKeyName(26, "delete-folder.png");
            this.imageCollection48.Images.SetKeyName(27, "down3.png");
            this.imageCollection48.Images.SetKeyName(28, "download.png");
            this.imageCollection48.Images.SetKeyName(29, "edit.png");
            this.imageCollection48.Images.SetKeyName(30, "email.png");
            this.imageCollection48.Images.SetKeyName(31, "email2.png");
            this.imageCollection48.Images.SetKeyName(32, "export.png");
            this.imageCollection48.Images.SetKeyName(33, "export1.png");
            this.imageCollection48.Images.SetKeyName(34, "faq.png");
            this.imageCollection48.Images.SetKeyName(35, "favorite.png");
            this.imageCollection48.Images.SetKeyName(36, "file.png");
            this.imageCollection48.Images.SetKeyName(37, "folder.png");
            this.imageCollection48.Images.SetKeyName(38, "forward.png");
            this.imageCollection48.Images.SetKeyName(39, "front.png");
            this.imageCollection48.Images.SetKeyName(40, "front1.png");
            this.imageCollection48.Images.SetKeyName(41, "fulltrash.png");
            this.imageCollection48.Images.SetKeyName(42, "hd.png");
            this.imageCollection48.Images.SetKeyName(43, "hd1.png");
            this.imageCollection48.Images.SetKeyName(44, "help.png");
            this.imageCollection48.Images.SetKeyName(45, "home.png");
            this.imageCollection48.Images.SetKeyName(46, "image.png");
            this.imageCollection48.Images.SetKeyName(47, "import.png");
            this.imageCollection48.Images.SetKeyName(48, "import2.png");
            this.imageCollection48.Images.SetKeyName(49, "info.png");
            this.imageCollection48.Images.SetKeyName(50, "install.png");
            this.imageCollection48.Images.SetKeyName(51, "locked.png");
            this.imageCollection48.Images.SetKeyName(52, "music.png");
            this.imageCollection48.Images.SetKeyName(53, "network.png");
            this.imageCollection48.Images.SetKeyName(54, "new-folder.png");
            this.imageCollection48.Images.SetKeyName(55, "opened.png");
            this.imageCollection48.Images.SetKeyName(56, "open-folder.png");
            this.imageCollection48.Images.SetKeyName(57, "paste.png");
            this.imageCollection48.Images.SetKeyName(58, "photo.png");
            this.imageCollection48.Images.SetKeyName(59, "police.png");
            this.imageCollection48.Images.SetKeyName(60, "printer.png");
            this.imageCollection48.Images.SetKeyName(61, "public.png");
            this.imageCollection48.Images.SetKeyName(62, "radar.png");
            this.imageCollection48.Images.SetKeyName(63, "save.png");
            this.imageCollection48.Images.SetKeyName(64, "save-as.png");
            this.imageCollection48.Images.SetKeyName(65, "screen-capture.png");
            this.imageCollection48.Images.SetKeyName(66, "search.png");
            this.imageCollection48.Images.SetKeyName(67, "send.png");
            this.imageCollection48.Images.SetKeyName(68, "software 2.png");
            this.imageCollection48.Images.SetKeyName(69, "software update.png");
            this.imageCollection48.Images.SetKeyName(70, "software.png");
            this.imageCollection48.Images.SetKeyName(71, "sound.png");
            this.imageCollection48.Images.SetKeyName(72, "statics.png");
            this.imageCollection48.Images.SetKeyName(73, "statics-1.png");
            this.imageCollection48.Images.SetKeyName(74, "statics-2.png");
            this.imageCollection48.Images.SetKeyName(75, "stop.png");
            this.imageCollection48.Images.SetKeyName(76, "support.png");
            this.imageCollection48.Images.SetKeyName(77, "switcher.png");
            this.imageCollection48.Images.SetKeyName(78, "trash.png");
            this.imageCollection48.Images.SetKeyName(79, "truck.png");
            this.imageCollection48.Images.SetKeyName(80, "up4.png");
            this.imageCollection48.Images.SetKeyName(81, "update.png");
            this.imageCollection48.Images.SetKeyName(82, "upload.png");
            this.imageCollection48.Images.SetKeyName(83, "user.png");
            this.imageCollection48.Images.SetKeyName(84, "uses.png");
            this.imageCollection48.Images.SetKeyName(85, "viewi-pr.png");
            this.imageCollection48.Images.SetKeyName(86, "web.png");
            this.imageCollection48.Images.SetKeyName(87, "window.png");
            this.imageCollection48.Images.SetKeyName(88, "window-2.png");
            this.imageCollection48.Images.SetKeyName(89, "zoom-.png");
            this.imageCollection48.Images.SetKeyName(90, "zoom+.png");
            this.imageCollection48.Images.SetKeyName(91, "alarm.png");
            this.imageCollection48.Images.SetKeyName(92, "bcir1.jpg");
            this.imageCollection48.Images.SetKeyName(93, "bcir2.jpg");
            this.imageCollection48.Images.SetKeyName(94, "break.jpg");
            this.imageCollection48.Images.SetKeyName(95, "cir1.jpg");
            this.imageCollection48.Images.SetKeyName(96, "cir12.jpg");
            this.imageCollection48.Images.SetKeyName(97, "cir123.jpg");
            this.imageCollection48.Images.SetKeyName(98, "cir132.jpg");
            this.imageCollection48.Images.SetKeyName(99, "copy.png");
            this.imageCollection48.Images.SetKeyName(100, "database.png");
            this.imageCollection48.Images.SetKeyName(101, "delete.png");
            this.imageCollection48.Images.SetKeyName(102, "edit.png");
            this.imageCollection48.Images.SetKeyName(103, "expand.jpg");
            this.imageCollection48.Images.SetKeyName(104, "hand1.jpg");
            this.imageCollection48.Images.SetKeyName(105, "hand2.jpg");
            this.imageCollection48.Images.SetKeyName(106, "landmark.jpg");
            this.imageCollection48.Images.SetKeyName(107, "line.jpg");
            this.imageCollection48.Images.SetKeyName(108, "Login_Exit.png");
            this.imageCollection48.Images.SetKeyName(109, "move.jpg");
            this.imageCollection48.Images.SetKeyName(110, "radar.png");
            this.imageCollection48.Images.SetKeyName(111, "redo.jpg");
            this.imageCollection48.Images.SetKeyName(112, "save.png");
            this.imageCollection48.Images.SetKeyName(113, "select.jpg");
            this.imageCollection48.Images.SetKeyName(114, "support.png");
            this.imageCollection48.Images.SetKeyName(115, "truck.png");
            this.imageCollection48.Images.SetKeyName(116, "undo.jpg");
            this.imageCollection48.Images.SetKeyName(117, "user.png");
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageCollection24;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.newCategory,
            this.newUser,
            this.btnOK,
            this.Cancel,
            this.delCategory,
            this.delUser});
            this.barManager1.MaxItemId = 6;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.newCategory, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.delCategory, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.newUser, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.delUser, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOK, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.Cancel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // newCategory
            // 
            this.newCategory.Caption = "新建分类";
            this.newCategory.Id = 0;
            this.newCategory.ImageIndex = 1;
            this.newCategory.Name = "newCategory";
            this.newCategory.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.newCategory_ItemClick);
            // 
            // delCategory
            // 
            this.delCategory.Caption = "删除分类";
            this.delCategory.Id = 4;
            this.delCategory.ImageIndex = 0;
            this.delCategory.Name = "delCategory";
            this.delCategory.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.delCategory_ItemClick);
            // 
            // newUser
            // 
            this.newUser.Caption = "新建用户";
            this.newUser.Id = 1;
            this.newUser.ImageIndex = 1;
            this.newUser.Name = "newUser";
            this.newUser.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.newUser_ItemClick);
            // 
            // delUser
            // 
            this.delUser.Caption = "删除用户";
            this.delUser.Id = 5;
            this.delUser.ImageIndex = 0;
            this.delUser.Name = "delUser";
            this.delUser.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.delUser_ItemClick);
            // 
            // btnOK
            // 
            this.btnOK.Caption = "保存";
            this.btnOK.Id = 2;
            this.btnOK.ImageIndex = 14;
            this.btnOK.Name = "btnOK";
            this.btnOK.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOK_ItemClick);
            // 
            // Cancel
            // 
            this.Cancel.Caption = "退出";
            this.Cancel.Id = 3;
            this.Cancel.ImageIndex = 25;
            this.Cancel.Name = "Cancel";
            this.Cancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Cancel_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(852, 39);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 448);
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlBottom.Size = new System.Drawing.Size(852, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 409);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(852, 39);
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 409);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gcCategory);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 39);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(266, 409);
            this.panelControl1.TabIndex = 4;
            // 
            // gcCategory
            // 
            this.gcCategory.DataSource = this.bsCategory;
            this.gcCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCategory.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcCategory.Location = new System.Drawing.Point(2, 2);
            this.gcCategory.MainView = this.gvCategory;
            this.gcCategory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcCategory.MenuManager = this.barManager1;
            this.gcCategory.Name = "gcCategory";
            this.gcCategory.Size = new System.Drawing.Size(262, 405);
            this.gcCategory.TabIndex = 0;
            this.gcCategory.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCategory});
            this.gcCategory.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // bsCategory
            // 
            this.bsCategory.DataSource = typeof(UserCategory);
            // 
            // gvCategory
            // 
            this.gvCategory.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gvCategory.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.CategoryCode,
            this.CategoryName});
            this.gvCategory.GridControl = this.gcCategory;
            this.gvCategory.Name = "gvCategory";
            this.gvCategory.OptionsBehavior.Editable = false;
            this.gvCategory.OptionsView.ShowDetailButtons = false;
            this.gvCategory.OptionsView.ShowGroupPanel = false;
            this.gvCategory.OptionsView.ShowIndicator = false;
            this.gvCategory.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvCategory_FoucsedRowChanged);
            this.gvCategory.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvCategory_BeforeLeaveRow);
            this.gvCategory.Click += new System.EventHandler(this.gvCategory_Click);
            this.gvCategory.DoubleClick += new System.EventHandler(this.gvCategory_DoubleClick);
            // 
            // CategoryCode
            // 
            this.CategoryCode.Caption = "分类编码";
            this.CategoryCode.FieldName = "CategoryCode";
            this.CategoryCode.Name = "CategoryCode";
            this.CategoryCode.OptionsColumn.ReadOnly = true;
            this.CategoryCode.Visible = true;
            this.CategoryCode.VisibleIndex = 0;
            // 
            // CategoryName
            // 
            this.CategoryName.Caption = "分类名称";
            this.CategoryName.FieldName = "CategoryName";
            this.CategoryName.Name = "CategoryName";
            this.CategoryName.OptionsColumn.ReadOnly = true;
            this.CategoryName.Visible = true;
            this.CategoryName.VisibleIndex = 1;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcOperButton);
            this.panelControl2.Controls.Add(this.gcUser);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(266, 39);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(586, 409);
            this.panelControl2.TabIndex = 5;
            // 
            // gcOperButton
            // 
            this.gcOperButton.ContextMenuStrip = this.cmSelect;
            this.gcOperButton.DataSource = this.bsSysOperButton;
            this.gcOperButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcOperButton.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcOperButton.Location = new System.Drawing.Point(2, 170);
            this.gcOperButton.MainView = this.gvOperButton;
            this.gcOperButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcOperButton.MenuManager = this.barManager1;
            this.gcOperButton.Name = "gcOperButton";
            this.gcOperButton.Size = new System.Drawing.Size(582, 237);
            this.gcOperButton.TabIndex = 0;
            this.gcOperButton.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOperButton});
            this.gcOperButton.Click += new System.EventHandler(this.gcOperButton_Click);
            // 
            // cmSelect
            // 
            this.cmSelect.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllSelect,
            this.CancelSelect});
            this.cmSelect.Name = "cmSelect";
            this.cmSelect.Size = new System.Drawing.Size(125, 48);
            // 
            // AllSelect
            // 
            this.AllSelect.Name = "AllSelect";
            this.AllSelect.Size = new System.Drawing.Size(124, 22);
            this.AllSelect.Text = "全部选中";
            this.AllSelect.Click += new System.EventHandler(this.AllSelect_Click);
            // 
            // CancelSelect
            // 
            this.CancelSelect.Name = "CancelSelect";
            this.CancelSelect.Size = new System.Drawing.Size(124, 22);
            this.CancelSelect.Text = "取消全选";
            this.CancelSelect.Click += new System.EventHandler(this.CancelSelect_Click);
            // 
            // bsSysOperButton
            // 
            this.bsSysOperButton.DataSource = typeof(SysOperButton);
            // 
            // gvOperButton
            // 
            this.gvOperButton.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gvOperButton.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ButtonChoose,
            this.ButtonType,
            this.ButtonCaption});
            this.gvOperButton.GridControl = this.gcOperButton;
            this.gvOperButton.Name = "gvOperButton";
            this.gvOperButton.OptionsView.ShowDetailButtons = false;
            this.gvOperButton.OptionsView.ShowGroupPanel = false;
            this.gvOperButton.OptionsView.ShowIndicator = false;
            // 
            // ButtonChoose
            // 
            this.ButtonChoose.Caption = "选择";
            this.ButtonChoose.FieldName = "IsSelect";
            this.ButtonChoose.Name = "ButtonChoose";
            this.ButtonChoose.Visible = true;
            this.ButtonChoose.VisibleIndex = 0;
            // 
            // ButtonType
            // 
            this.ButtonType.Caption = "权限类型";
            this.ButtonType.FieldName = "ButtonTypeStr";
            this.ButtonType.Name = "ButtonType";
            this.ButtonType.OptionsColumn.AllowEdit = false;
            this.ButtonType.OptionsColumn.ReadOnly = true;
            this.ButtonType.Visible = true;
            this.ButtonType.VisibleIndex = 1;
            // 
            // ButtonCaption
            // 
            this.ButtonCaption.Caption = "权限名称";
            this.ButtonCaption.FieldName = "ButtonCaption";
            this.ButtonCaption.Name = "ButtonCaption";
            this.ButtonCaption.OptionsColumn.AllowEdit = false;
            this.ButtonCaption.OptionsColumn.ReadOnly = true;
            this.ButtonCaption.Visible = true;
            this.ButtonCaption.VisibleIndex = 2;
            // 
            // gcUser
            // 
            this.gcUser.DataSource = this.bsUser;
            this.gcUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcUser.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcUser.Location = new System.Drawing.Point(2, 2);
            this.gcUser.MainView = this.gvUser;
            this.gcUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcUser.MenuManager = this.barManager1;
            this.gcUser.Name = "gcUser";
            this.gcUser.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.txtUerPassWord});
            this.gcUser.Size = new System.Drawing.Size(582, 168);
            this.gcUser.TabIndex = 0;
            this.gcUser.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvUser});
            this.gcUser.Click += new System.EventHandler(this.gcUser_Click);
            // 
            // bsUser
            // 
            this.bsUser.DataSource = typeof(UserInfo);
            // 
            // gvUser
            // 
            this.gvUser.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gvUser.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.UserID,
            this.PassWord,
            this.UserName});
            this.gvUser.GridControl = this.gcUser;
            this.gvUser.Name = "gvUser";
            this.gvUser.OptionsBehavior.Editable = false;
            this.gvUser.OptionsView.ShowDetailButtons = false;
            this.gvUser.OptionsView.ShowGroupPanel = false;
            this.gvUser.OptionsView.ShowIndicator = false;
            this.gvUser.DoubleClick += new System.EventHandler(this.gvUser_DoubleClick);
            // 
            // UserID
            // 
            this.UserID.Caption = "用户账号";
            this.UserID.FieldName = "UserID";
            this.UserID.Name = "UserID";
            this.UserID.OptionsColumn.ReadOnly = true;
            this.UserID.Visible = true;
            this.UserID.VisibleIndex = 0;
            // 
            // PassWord
            // 
            this.PassWord.Caption = "用户密码";
            this.PassWord.ColumnEdit = this.txtUerPassWord;
            this.PassWord.FieldName = "PassWord";
            this.PassWord.Name = "PassWord";
            this.PassWord.OptionsColumn.ReadOnly = true;
            this.PassWord.Visible = true;
            this.PassWord.VisibleIndex = 1;
            // 
            // txtUerPassWord
            // 
            this.txtUerPassWord.AutoHeight = false;
            this.txtUerPassWord.Name = "txtUerPassWord";
            this.txtUerPassWord.PasswordChar = '*';
            // 
            // UserName
            // 
            this.UserName.Caption = "用户名称";
            this.UserName.FieldName = "UserName";
            this.UserName.Name = "UserName";
            this.UserName.OptionsColumn.ReadOnly = true;
            this.UserName.Visible = true;
            this.UserName.VisibleIndex = 2;
            // 
            // FrmSysAut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 448);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmSysAut";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "权限控制";
            this.Shown += new System.EventHandler(this.FrmSysAut_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection48)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcOperButton)).EndInit();
            this.cmSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsSysOperButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOperButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUerPassWord)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem newCategory;
        private DevExpress.XtraBars.BarButtonItem newUser;
        private DevExpress.XtraBars.BarButtonItem btnOK;
        private DevExpress.XtraBars.BarButtonItem Cancel;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcUser;
        private DevExpress.XtraGrid.Views.Grid.GridView gvUser;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gcCategory;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCategory;
        private DevExpress.XtraGrid.Columns.GridColumn CategoryCode;
        private DevExpress.XtraGrid.Columns.GridColumn CategoryName;
        private DevExpress.XtraGrid.GridControl gcOperButton;
        private DevExpress.XtraGrid.Views.Grid.GridView gvOperButton;
        private DevExpress.XtraGrid.Columns.GridColumn ButtonChoose;
        private DevExpress.XtraGrid.Columns.GridColumn ButtonType;
        private DevExpress.XtraGrid.Columns.GridColumn ButtonCaption;
        private DevExpress.XtraGrid.Columns.GridColumn UserID;
        private DevExpress.XtraGrid.Columns.GridColumn PassWord;
        private System.Windows.Forms.BindingSource bsSysOperButton;
        private System.Windows.Forms.BindingSource bsUser;
        private System.Windows.Forms.BindingSource bsCategory;
        private DevExpress.XtraGrid.Columns.GridColumn UserName;
        private DevExpress.XtraBars.BarButtonItem delCategory;
        private DevExpress.XtraBars.BarButtonItem delUser;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtUerPassWord;
        private System.Windows.Forms.ContextMenuStrip cmSelect;
        private System.Windows.Forms.ToolStripMenuItem AllSelect;
        private System.Windows.Forms.ToolStripMenuItem CancelSelect;
    }
}