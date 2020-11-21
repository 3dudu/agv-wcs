using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.XtraEditors.Repository;
using RCSEditTool.FrmCommon;
using Model.MSM;
using Model.Comoon;

namespace RCSEditTool.FrmSystem
{
    public partial class FrmSystemConfig : BaseForm
    {
        public FrmSystemConfig()
        {
            InitializeComponent();
            this.Icon = base.Icon;
        }

        IList<ParameterMode> ParameterModes = new List<ParameterMode>();

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                IList<SysParameter> systems = bsDetail.List as IList<SysParameter>;
                if (systems == null) { return; }
                if (systems.Count <= 0) { return; }
                OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.SaveParameter(systems);
                MsgBox.Show(opr);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
            return;
        }

        private void FrmSystemConfig_Shown(object sender, EventArgs e)
        {
            try
            {
                ParameterModes = AGVDAccess.AGVClientDAccess.LoadAllParameterMode();
                if (ParameterModes == null) { return; }
                if (ParameterModes.Count <= 0) { return; }
                IList<SysParameter> systemParameter = new List<SysParameter>();
                foreach (ParameterMode item in ParameterModes)
                {
                    SysParameter syspara = new SysParameter();
                    syspara.ParameterCode = item.ParameterCode;
                    syspara.ParameterName = item.ParameterName;
                    SysParameter GetPara = AGVDAccess.AGVClientDAccess.GetParameterByCode(item.ParameterCode);
                    if (GetPara != null && !string.IsNullOrEmpty(GetPara.ParameterValue))
                    { syspara.ParameterValue = GetPara.ParameterValue; }
                    else
                    { syspara.ParameterValue = item.DefaultValue; }
                    systemParameter.Add(syspara);
                }
                this.bsDetail.DataSource = systemParameter;
                bsDetail.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvDetail_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                SysParameter Curr_Sys = bsDetail.Current as SysParameter;
                if (Curr_Sys == null) { return; }
                ParameterMode Curr_paraMode = ParameterModes.Where(p => p.ParameterCode.Equals(Curr_Sys.ParameterCode)).FirstOrDefault();
                if (Curr_paraMode == null) { return; }
                colParameterValue.ColumnEdit = null;
                if (Curr_paraMode.ExitType == 2)
                {
                    string[] Items = Curr_paraMode.ChooseValues.Split(';');
                    RepositoryItemComboBox cbxvalue = new RepositoryItemComboBox();
                    cbxvalue.Appearance.Options.UseTextOptions = true;
                    cbxvalue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    cbxvalue.AppearanceDropDown.Options.UseTextOptions = true;
                    cbxvalue.AppearanceDropDown.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    cbxvalue.AutoHeight = false;
                    cbxvalue.Items.AddRange(Items);
                    cbxvalue.Name = "cbxCarType";
                    cbxvalue.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    colParameterValue.ColumnEdit = cbxvalue;
                }
                else if (Curr_paraMode.ExitType == 3)
                {
                    RepositoryItemTimeEdit datevalue = new RepositoryItemTimeEdit();
                    datevalue.TimeEditStyle = DevExpress.XtraEditors.Repository.TimeEditStyle.TouchUI;
                    datevalue.Appearance.Options.UseTextOptions = true;
                    datevalue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    datevalue.AppearanceDropDown.Options.UseTextOptions = true;
                    datevalue.AutoHeight = false;
                    datevalue.Name = "deTime";
                    colParameterValue.ColumnEdit = datevalue;
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}
