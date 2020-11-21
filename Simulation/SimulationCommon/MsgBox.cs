using DevExpress.XtraEditors;
using Model.Comoon;
using Model.MSM;
using System.Windows.Forms;

namespace Simulation.SimulationCommon
{
    /// <summary>
    /// 弹窗提示类
    /// </summary>
    public class MsgBox
    {
        public static DialogResult ShowError(string Msg)
        {
            return XtraMessageBox.Show(Msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        public static DialogResult ShowWarn(string Msg)
        {
            return XtraMessageBox.Show(Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowQuestion(string Msg)
        {
            return XtraMessageBox.Show(Msg, "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult Show(OperateReturnInfo Msg)
        {
            if (Msg.ReturnCode == OperateCodeEnum.Success)
            { return XtraMessageBox.Show("操作成功!"); }
            else
            { return XtraMessageBox.Show(Msg.ReturnInfo.ToString()); }
        }
    }
}
