using Canvas;
using Canvas.DrawTools;
using Model.CarInfoExtend;
using Model.MDM;
using SQLServerOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowPDAClient
{
    public partial class FrmMain : Form, ICanvasOwner, IEditToolOwner
    {
        public FrmMain()
        {
            InitializeComponent();
            using (FrmWaitingBox dialog = new FrmWaitingBox("正在加载地图,请稍后..."))
            {
                InitCanvas("", false);
            }
        }


        #region 窗体属性
        DataModel m_data;
        CanvasCtrl m_canvas;
        string m_filename = string.Empty;
        private Thread thread_DataUpdate;
        IList<StorageInfo> Storeges = new List<StorageInfo>();
        #endregion

        #region 窗体事件
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            try
            {
                InitData();
                
            }
            catch (Exception ex)
            { }
        }

        private void btnInStore_Click(object sender, EventArgs e)
        {
            try
            {
                using (FrmPDACall frm = new FrmPDACall())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        //根据选择的物料类型进行入库
                        int CallMaterialType = frm.ChoosedMaterialType;
                        
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnOutStore_Click(object sender, EventArgs e)
        {
            try
            {
                using (FrmPDACall frm = new FrmPDACall())
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnMoveStore_Click(object sender, EventArgs e)
        {

        }

        private void btnSpray_Click(object sender, EventArgs e)
        {
            try
            {
                using (FrmConfig frm = new FrmConfig())
                { frm.ShowDialog(); }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowQuestion("确认退出系统?") == DialogResult.Yes)
            {
                this.Close();
                return;
            }
        }
        #endregion

        #region 窗体自定义函数、方法
        /// <summary>
        /// 初始化画布函数
        /// </summary>
        private void InitCanvas(string filename, bool IsNew)
        {
            if (!IsNew)
            {
                try
                {
                    AGVDAccess.AGVClientDAccess.GetPlanSet();
                    string tempFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\temSet.agv";
                    if (File.Exists(tempFile))
                    { filename = tempFile; }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
            try
            {
                m_data = new DataModel();
                if (filename.Length > 0 && File.Exists(filename) && m_data.Load(filename,null))
                { m_filename = filename; }
                m_canvas = new CanvasCtrl(this, m_data);
                m_canvas.m_model.Zoom = 0.067f;
                m_canvas.Dock = DockStyle.Fill;
                pnlMain.Controls.Add(m_canvas);
                m_canvas.KeyDown += new KeyEventHandler(OnCanvasKeyDown);
                m_canvas.MouseUp += new MouseEventHandler(OnCanvasMouseUp);
                m_canvas.MouseDoubleClick += new MouseEventHandler(OnCanvasMouseDoubleClick);
                //AGVModel.LandmarkInfo landmark = Content.AllPoint.FirstOrDefault(p => p.LandmarkCode.Equals("442"));
                //if (landmark != null)
                //    m_canvas.SetCenter(new UnitPoint(landmark.LandX, landmark.LandY));
                thread_DataUpdate = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(1 * 1000);
                        UpdateStock();
                    }
                })
                { IsBackground = true };
                thread_DataUpdate.Start();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            try
            {
                //加载车辆信息
                IList<CarInfo> AllCars = AGVDAccess.AGVClientDAccess.LoadCarInfoAchive();
                this.bsCar.DataSource = AllCars;
                this.bsCar.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        /// <summary>
        /// 画布键盘事件
        /// </summary>
        private void OnCanvasKeyDown(object sender, KeyEventArgs e)
        { }

        /// <summary>
        /// 画布鼠标弹出时间
        /// </summary>
        private void OnCanvasMouseUp(object sender, MouseEventArgs e)
        {
            try
            {

                //if (m_data.SelectedObjects.Count() > 0)
                //{
                //    StorageTool curr = m_data.SelectedObjects.FirstOrDefault(p => p.Id.Equals("StorageTool")) as StorageTool;

                //}
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void OnCanvasMouseDoubleClick(object sender, MouseEventArgs e)
        {}


        public void SetPositionInfo(UnitPoint unitpos)
        { }

        public void SetSnapInfo(ISnapPoint snap)
        { }

        public void SetHint(string text)
        { }

        /// <summary>
        /// 更新储位状态信息
        /// </summary>
        private void UpdateStock()
        {
            if (m_canvas.InvokeRequired)
            {
                m_canvas.Invoke(new MethodInvoker(() => { UpdateStock(); }));
            }
            else
            {
                try
                {
                    //加载所有的储位信息
                    Storeges = AGVDAccess.AGVClientDAccess.LoadStorages();
                    if (Storeges != null)
                    {
                        foreach (StorageInfo item in Storeges)
                        {
                            StorageTool storage = m_data.ActiveLayer.Objects.Where(p => p.Id == "StorageTool" && (p as StorageTool).StcokID == item.ID).FirstOrDefault() as StorageTool;
                            if (storage != null)
                            {
                                storage.OwnArea = item.OwnArea;
                                storage.SubOwnArea = item.SubOwnArea;
                                storage.matterType = item.matterType;
                                storage.StorageState = item.StorageState;
                                storage.LockState = item.LockState;
                                storage.MaterielType = item.MaterielType;
                                storage.StorageName = item.StorageName;
                            }
                        }
                        m_canvas.DoInvalidate(true);
                    }
                }
                catch (Exception ex)
                { }
            }
        }



        #endregion

       
    }//endForm
}
