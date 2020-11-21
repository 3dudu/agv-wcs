using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using RCSEditTool.FrmCommon;
using Model.MSM;

namespace RCSEditTool.FrmExternal
{
    public partial class FormSelectIOAction : BaseForm
    {
        public FormSelectIOAction()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }
        #region 窗体属性
        //当前选择的所有的IO动作
        public IList<IOActionInfo> SelectIOActions = new List<IOActionInfo>();
        #endregion

        #region 窗体事件
        private void FormSelectIOAction_Shown(object sender, EventArgs e)
        {
            try
            {
                IList<IOActionInfo> AllIOActions = AGVDAccess.AGVClientDAccess.LoadAllIOAction();
                if (AllIOActions != null)
                {
                    this.bsIOAcion.DataSource = AllIOActions;
                    bsIOAcion.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnSelectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                foreach (IOActionInfo item in bsIOAcion)
                { item.IsSelect = true; }
                bsIOAcion.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnSelectNone_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                foreach (IOActionInfo item in bsIOAcion)
                { item.IsSelect = false; }
                bsIOAcion.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnReflectSelect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                foreach (IOActionInfo item in bsIOAcion)
                { item.IsSelect = !item.IsSelect; }
                bsIOAcion.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                SelectIOActions = this.bsIOAcion.List.Cast<IOActionInfo>().Where(p => p.IsSelect).ToList();
                DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
        #endregion


    }//endForm
}