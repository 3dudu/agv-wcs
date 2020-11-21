using AGVDAccess;
using Canvas;
using Canvas.DrawTools;
using Model.MDM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowPDAClient
{
    public partial class FrmPDACall : Form, ICanvasOwner, IEditToolOwner
    {
        public FrmPDACall()
        {
            InitializeComponent();
            using (FrmWaitingBox dialog = new FrmWaitingBox("正在加载地图,请稍后..."))
            {
                InitCanvas("", false);
            }
        }


        #region 窗体属性
        private Thread thread_DataUpdate;
        private DataModel m_data;
        private CanvasCtrl m_canvas;
        private string m_filename = string.Empty;
        private IList<StorageInfo> Storeges = new List<StorageInfo>();
        public int ChoosedMaterialType = 0;
        #endregion

        #region 窗体事件
        private void FrmPDACall_Shown(object sender, EventArgs e)
        {
            try
            {
                CreatBtnByMaterials();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
        #endregion

        #region 窗体自定义函数
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
                IList<string> ContainElement = new List<string>();
                ContainElement.Add("StorageTool");
                if (filename.Length > 0 && File.Exists(filename) && m_data.Load(filename, ContainElement))
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

        public void SetPositionInfo(UnitPoint unitpos)
        { }

        public void SetSnapInfo(ISnapPoint snap)
        { }

        public void SetHint(string text)
        { }

        /// <summary>
        /// 画布键盘事件
        /// </summary>
        private void OnCanvasKeyDown(object sender, KeyEventArgs e)
        { }

        /// <summary>
        /// 画布鼠标弹出时间
        /// </summary>
        private void OnCanvasMouseUp(object sender, MouseEventArgs e)
        { }

        private void OnCanvasMouseDoubleClick(object sender, MouseEventArgs e)
        { }

        /// <summary>
        /// 加载所有的物料类型
        /// </summary>
        private void CreatBtnByMaterials()
        {
            try
            {
                IList<MaterialInfo> AllMaterials = AGVClientDAccess.LoadAllMaterial();
                if (AllMaterials != null)
                {
                    int TotalWidth = this.Width;
                    int RowAcount = TotalWidth / (150 + 5);
                    int Amount = 0;
                    int HasWidth = 0;
                    int RowCount = 1;
                    foreach (MaterialInfo MaterialInfo in AllMaterials)
                    {
                        Amount++;
                        Button btn = new Button();
                        btn.Tag = MaterialInfo.MaterialType;
                        btn.Anchor = AnchorStyles.Left;
                        btn.Text = MaterialInfo.MaterialName;
                        btn.Click += Btn_Click;
                        btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 0;
                        btn.BackColor = Color.DarkRed;
                        btn.ForeColor = Color.White;
                        btn.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        btn.Size = new System.Drawing.Size(150, 60);
                        if (Amount == 1)
                        {
                            btn.Location = new Point(5, 5);
                            HasWidth = btn.Width + 5;
                        }
                        else if (Amount <= RowAcount)
                        {
                            btn.Location = new Point(HasWidth + 5, ((RowCount - 1) < 0 ? 0 : RowCount - 1) * btn.Height + (5 * RowCount));
                            HasWidth += btn.Width + 5;
                        }
                        else
                        {
                            HasWidth = 0;
                            RowCount += 1;
                            btn.Location = new Point(HasWidth + 5, ((RowCount - 1) < 0 ? 0 : RowCount - 1) * btn.Height + (5 * RowCount));
                            HasWidth += btn.Width + 5;
                            Amount = 1;
                        }
                        this.pnlBtn.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }


        private void Btn_Click(object sender, EventArgs e)
        {
            try
            {
                int MaterialType = -1;
                if (sender is Button)
                {
                    string toolid = ((Button)sender).Tag.ToString();
                    if (!string.IsNullOrEmpty(toolid))
                    {
                        try
                        {
                            MaterialType = Convert.ToInt16(toolid);
                        }
                        catch (Exception ex)
                        {
                            MsgBox.ShowWarn(ex.Message);
                            return;
                        }

                        if (MsgBox.ShowQuestion("确认选择[" + ((Button)sender).Text + "]工件?") == DialogResult.Yes)
                        {
                            this.ChoosedMaterialType = MaterialType;
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }


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


    }
}
