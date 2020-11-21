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
    public partial class FrmPad : BaseForm
    {
        public FrmPad()
        {
            InitializeComponent();
        }
        #region 事件
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPad_Shown(object sender, EventArgs e)
        {
            try
            {
                this.CenterToParent();

                this.bsPad.DataSource = AGVDAccess.AGVClientDAccess.LoadAllPad();
                this.bsPad.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                PdaInfo pda = new PdaInfo();
                int PdaID = bsPad.Count + 1;
                pda.PadID = PdaID;
                pda.IsNew = true;
                using (FrmPdaOperSet frm = new FrmPdaOperSet(pda))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.bsPad.DataSource = AGVDAccess.AGVClientDAccess.LoadAllPad();
                        this.bsPad.ResetBindings(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                PdaInfo pda = this.bsPad.Current as PdaInfo;
                if (pda != null)
                {
                    IList<PdaOperSetInfo> pdaOperSetlist = AGVDAccess.AGVClientDAccess.LoadAllPdaOperSet();
                    pda.PdaOperSetList = pdaOperSetlist.Where(p => p.PdaID == pda.PadID).ToList();
                    using (FrmPdaOperSet frm = new FrmPdaOperSet(pda))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.bsPad.DataSource = AGVDAccess.AGVClientDAccess.LoadAllPad();
                            this.bsPad.ResetBindings(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (MsgBox.ShowQuestion("是否确认删除当前行信息") == DialogResult.Yes)
                {
                    PdaInfo pda = this.bsPad.Current as PdaInfo;
                    if (pda != null)
                    {
                        OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.DeletePad(pda.PadID);
                        MsgBox.Show(opr);
                        if (opr.ReturnCode == OperateCodeEnum.Success)
                        {
                            this.bsPad.DataSource = AGVDAccess.AGVClientDAccess.LoadAllPad();
                            this.bsPad.ResetBindings(false);
                        }
                    }
                }
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
                return;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 行双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPad_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                btnEdit_ItemClick(null, null);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        #endregion


    }
}