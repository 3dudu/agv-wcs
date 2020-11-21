using AGVDAccess;
using Model.Comoon;
using Model.MDM;
using RCSEditTool;
using RCSEditTool.FrmCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCSEditTool.FrmExternal
{
    public partial class FrmPdaOperSet : BaseForm
    {

        PdaInfo Pda = new PdaInfo();
        public FrmPdaOperSet()
        {
            InitializeComponent();
        }

        public FrmPdaOperSet(PdaInfo pda)
        {
            InitializeComponent();
            Pda = pda;
        }

        #region 事件
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPdaOperSet_Shown(object sender, EventArgs e)
        {
            try
            {
                this.CenterToParent();
                if (Pda.IsNew)
                {
                    this.txtPadID.Properties.ReadOnly = false;
                }
                else
                {
                    this.txtPadID.Properties.ReadOnly = true;
                }

                DataTable AreaInfos = AGVDAccess.AGVClientDAccess.LoadAread();
                PadAreaInfo.EditValue = "OwnArea";
                PadAreaInfo.Properties.ValueMember = "OwnArea";
                PadAreaInfo.Properties.DisplayMember = "AreaName";
                PadAreaInfo.Properties.DataSource = AreaInfos;
                //显示第一个值
                PadAreaInfo.ItemIndex = 0;

                PadAreaInfo.Properties.PopulateColumns();
                //隐藏第一个列
                PadAreaInfo.Properties.Columns[0].Visible = false;
                PadAreaInfo.Properties.Columns[1].Caption = "区域";

                this.txtPadID.Text = Pda.PadID.ToString();
                this.txtDesc.Text = Pda.Discripetion;
                //this.cbxArea.SelectedItem = ;
                this.PadAreaInfo.EditValue = Pda.Area;
                this.cbxIsViewArea.SelectedIndex = Pda.IsViewArea;
                this.cbxPadType.SelectedIndex = Pda.PadType;
                this.bsPadset.DataSource = Pda.PdaOperSetList;
                this.bsPadset.ResetBindings(false);
                cbxArea_SelectedIndexChanged(null, null);

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 增行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.gvPadset.BeforeLeaveRow -= this.gvPadset_BeforeLeaveRow;
                this.labelControl1.Focus();
                this.gvPadset.CloseEditor();
                PdaOperSetInfo currrow = this.bsPadset.Current as PdaOperSetInfo;
                if (currrow == null || Vaild(currrow))
                {
                    PdaOperSetInfo newrow = new PdaOperSetInfo();
                    newrow.BtnID = bsPadset.Count + 1;
                    this.bsPadset.Add(newrow);
                    this.bsPadset.ResetBindings(false);
                    this.bsPadset.MoveLast();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            finally
            {
                this.gvPadset.BeforeLeaveRow += this.gvPadset_BeforeLeaveRow;
            }
        }

        /// <summary>
        /// 删行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.gvPadset.BeforeLeaveRow -= this.gvPadset_BeforeLeaveRow;
                this.bsPadset.RemoveCurrent();
                bsPadset.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            finally
            {
                this.gvPadset.BeforeLeaveRow += this.gvPadset_BeforeLeaveRow;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //MessageBox.Show(PadAreaInfo.Text);

                //MessageBox.Show(PadAreaInfo.EditValue.ToString());
                ////MessageBox.Show(PadAreaInfo);

                //return;

                this.labelControl1.Focus();
                //验证当前行
                PdaOperSetInfo currrow = this.bsPadset.Current as PdaOperSetInfo;
                if (!Vaild(currrow)) return;
                if (string.IsNullOrEmpty(txtPadID.Text))
                {
                    MsgBox.ShowWarn("平板ID不能为空");
                    return;
                }
                if (this.bsPadset.List == null || this.bsPadset.Count == 0)
                {
                    MsgBox.ShowWarn("按钮列表不能为空");
                    return;
                }
                Pda.PadID = Convert.ToInt32(this.txtPadID.Text);
                Pda.Area = Convert.ToInt32(this.PadAreaInfo.EditValue);
                Pda.PadType = this.cbxPadType.SelectedIndex;
                Pda.IsViewArea = this.cbxIsViewArea.SelectedIndex;
                Pda.Discripetion = this.txtDesc.Text;
                IList<PdaOperSetInfo> PdaOperSetList = this.bsPadset.List as IList<PdaOperSetInfo>;
                Pda.PdaOperSetList = PdaOperSetList;
                foreach (PdaOperSetInfo ps in PdaOperSetList)
                {
                    IList<PdaOperSetInfo> allPdaOperSet = AGVDAccess.AGVClientDAccess.LoadallPdaOperSet(ps.PdaID);
                    if (ps != null && allPdaOperSet != null && allPdaOperSet.Count > 0)
                    {
                        if (allPdaOperSet.Where(p => p.BtnSendValue == ps.BtnSendValue && p.BtnID != ps.BtnID).Count() > 0)
                        {
                            MsgBox.ShowWarn("发送值ID已存在!");
                            return;
                        }
                    }
                }
                OperateReturnInfo opt = AGVDAccess.AGVClientDAccess.SavePad(Pda);
                MsgBox.Show(opt);
                if (opt.ReturnCode == OperateCodeEnum.Success)
                { this.DialogResult = DialogResult.OK; }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 离开行时判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPadset_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                this.labelControl1.Focus();
                this.gvPadset.CloseEditor();
                PdaOperSetInfo currrow = this.bsPadset.Current as PdaOperSetInfo;
                if (currrow != null)
                {
                    e.Allow = Vaild(currrow);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public bool Vaild(PdaOperSetInfo row)
        {
            if (row != null)
            {
                if (row.BtnID == 0)
                {
                    MsgBox.ShowWarn("按钮ID不能为空");
                    return false;
                }
                if (string.IsNullOrEmpty(row.BtnSendValue))
                {
                    MsgBox.ShowWarn("发送值不能为空");
                    return false;
                }

            }
            return true;
        }
        #endregion

        private void cbxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (cbxArea.SelectedIndex == 6)
            //    {this.colAreaStr.Visible = true;}
            //    else
            //    { this.colAreaStr.Visible = false; }
            //}
            //catch (Exception ex)
            //{ MsgBox.ShowError(ex.Message); }

        }

        private void cbxPadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxPadType.SelectedIndex == 1)
                { this.colOperType.Visible = false; }
                else
                { this.colOperType.Visible = true; }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}