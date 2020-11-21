using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowPDAClient
{
    public static class MsgBox
    {
        public static DialogResult ShowError(string Mes)
        {
            using (FrmMsgBox frm = new FrmMsgBox("错误异常", Mes))
            { return frm.ShowDialog(); }
        }

        public static DialogResult ShowWarn(string Msg)
        {
            using (FrmMsgBox frm = new FrmMsgBox("提示信息", Msg))
            { return frm.ShowDialog(); }
        }

        public static DialogResult ShowQuestion(string Msg)
        {
            using (FrmMsgBox frm = new FrmMsgBox("询问信息", Msg,MsgTypeEnum.Question,MsgBtnTypeEnum.YesNo))
            { return frm.ShowDialog(); }
        }
    }//end
}
