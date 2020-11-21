using Model.Comoon;
using RCSEditTool.FrmCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCSEditTool.FrmSystem
{
    public partial class FrmAssemblyConfig : BaseForm
    {
        public FrmAssemblyConfig()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtAssemblyAGVClassName.Text.Trim()) && string.IsNullOrEmpty(txtAssemblyChargeClassName.Text.Trim()))
                {
                    MsgBox.ShowWarn("请配置程序集！");
                    return;
                }
                IList<DispatchAssembly> assemblys = new List<DispatchAssembly>();
                if (!string.IsNullOrEmpty(txtAssemblyAGVClassName.Text.Trim())){
                    DispatchAssembly agvAssembly = new DispatchAssembly()
                    {
                        AssemblyType = 0,
                        ClassName = txtAssemblyAGVClassName.Text.Trim(),
                        Discript = txtAssemblyAGVDescript.Text.Trim()
                    };
                    assemblys.Add(agvAssembly);
                }
                if (!string.IsNullOrEmpty(txtAssemblyChargeClassName.Text.Trim())) {
                    DispatchAssembly chargeAssembly = new DispatchAssembly()
                    {
                        AssemblyType = 1,
                        ClassName = txtAssemblyChargeClassName.Text.Trim(),
                        Discript = txtAssemblyChargeDescript.Text.Trim()
                    };
                    assemblys.Add(chargeAssembly);
                }
                OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.SaveAssemblySet(assemblys);
                MsgBox.Show(opr);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                return;
            }
            catch (Exception x)
            { MsgBox.ShowError(x.Message); }
        }

        private void FrmAssemblyConfig_Shown(object sender, EventArgs e)
        {
            try
            {
                IList<DispatchAssembly> assemblys = AGVDAccess.AGVClientDAccess.LoadAssemblys();
                if (assemblys != null && assemblys.Count > 0)
                {
                    foreach(DispatchAssembly assembly in assemblys)
                    {
                        if (assembly.AssemblyType == 0)
                        {
                            txtAssemblyAGVClassName.Text = assembly.ClassName;
                            txtAssemblyAGVDescript.Text = assembly.Discript;
                        }
                        if(assembly.AssemblyType == 1)
                        {
                            txtAssemblyChargeClassName.Text = assembly.ClassName;
                            txtAssemblyChargeDescript.Text = assembly.Discript;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
    }
}
