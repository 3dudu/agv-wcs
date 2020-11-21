using Model.Comoon;
using SocketClient;
using SQLServerOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace Display
{
    public partial class FrmSysConnSet : BaseForm
    {
        public FrmSysConnSet()
        {
            InitializeComponent();
            this.Icon = base.Icon;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        #region 窗体属性
        DataBaseInfo TestdataBaseInfo = null;
        string txtPath = Application.StartupPath + @"\DataSource.txt";//通信设置信息保存的文件
        TcpClientSever clientserver = new TcpClientSever();
        ClientConfig serverconfig = new ClientConfig();



        #endregion


        #region 窗体事件
        private void FrmSysConnSet_Shown(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ConnectConfigTool.TxtToDT();
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.txtIP.Text = dt.Rows[0]["DBIP"].ToString();
                    this.txtDBName.Text = dt.Rows[0]["DBName"].ToString();
                    this.txtPass.Text = dt.Rows[0]["DBPass"].ToString();
                    this.txtUser.Text = dt.Rows[0]["DBUser"].ToString();
                    this.txtServeIP.Text = dt.Rows[0]["ServerIP"].ToString();
                    this.txtServePort.Text = dt.Rows[0]["ServerPort"].ToString();
                }
                this.label1.Focus();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                this.label1.Focus();
                if (Valide())
                {
                    if (TestdataBaseInfo == null)
                    { TestdataBaseInfo = new DataBaseInfo(); }
                    TestdataBaseInfo.DataBaseName = this.txtDBName.Text.Trim();
                    TestdataBaseInfo.DbSource = this.txtIP.Text.Trim();
                    TestdataBaseInfo.Pwd = this.txtPass.Text.Trim();
                    TestdataBaseInfo.Uid = this.txtUser.Text.Trim();

                    IDbOperator dbTesthqOperator = CreateDbOperator.DbOperatorInstance(TestdataBaseInfo);
                    try
                    {
                        //测试数据库
                        if (dbTesthqOperator.ServerIsThrough())
                        { }
                        else
                        {
                            MsgBox.ShowError("数据库地址不正确!");
                            return;
                        }
                        serverconfig = new ClientConfig() { ServerIP = txtServeIP.Text.Trim(), Port = System.Convert.ToInt32(txtServePort.Text.Trim()), TimeOut = 5 * 1000, ReceiveBufferSize = 128 };
                        if (!clientserver.TestConnect(serverconfig))
                        { MsgBox.ShowWarn("连接上位机失败!"); return; }
                        MsgBox.ShowWarn("测试成功!");
                    }
                    catch (Exception ex)
                    { MsgBox.ShowError(ex.Message); }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.label1.Focus();
                if (Valide())
                {
                    using (DevExpress.Utils.WaitDialogForm dialog = new DevExpress.Utils.WaitDialogForm("正在测试数据库连接,请稍后...", "提示"))
                    {
                        if (TestdataBaseInfo == null)
                        { TestdataBaseInfo = new DataBaseInfo(); }
                        TestdataBaseInfo.DataBaseName = this.txtDBName.Text.Trim();
                        TestdataBaseInfo.DbSource = this.txtIP.Text.Trim();
                        TestdataBaseInfo.Pwd = this.txtPass.Text.Trim();
                        TestdataBaseInfo.Uid = this.txtUser.Text.Trim();

                        IDbOperator dbTesthqOperator = CreateDbOperator.DbOperatorInstance(TestdataBaseInfo);
                        //测试数据库
                        if (dbTesthqOperator.ServerIsThrough())
                        { }
                        else
                        {
                            MsgBox.ShowError("数据库地址不正确!");
                            return;
                        }
                    }
                    DataTable dt = ConnectConfigTool.GetDataTableStruct();
                    DataRow dr = dt.NewRow();
                    dr["DBIP"] = this.txtIP.Text.Trim();
                    dr["DBName"] = this.txtDBName.Text.Trim();
                    dr["DBUser"] = this.txtUser.Text.Trim();
                    dr["DBPass"] = this.txtPass.Text.Trim();
                    dr["ServerIP"] = this.txtServeIP.Text.Trim();
                    dr["ServerPort"] = this.txtServePort.Text.Trim();
                    dt.Rows.Add(dr);
                    DtToTxt(dt);
                    ConnectConfigTool.setDBase();
                    MsgBox.ShowWarn("保存成功!");
                    if (MsgBox.ShowQuestion("是否重新加载地图?") == DialogResult.Yes)
                        this.DialogResult = DialogResult.OK;
                    else
                        this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(txtPath)) { return; }
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmSysConnSet_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                { btnOK_Click(null, null); }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        #endregion

        #region 窗体函数
        private bool Valide()
        {
            if (string.IsNullOrEmpty(this.txtDBName.Text))
            {
                MsgBox.ShowWarn("请输入数据库名称!");
                return false;
            }
            if (string.IsNullOrEmpty(this.txtIP.Text))
            {
                MsgBox.ShowWarn("请输入数据库地址!");
                return false;
            }
            if (string.IsNullOrEmpty(this.txtPass.Text))
            {
                MsgBox.ShowWarn("请输入数据库口令密码!");
                return false;
            }
            if (string.IsNullOrEmpty(this.txtUser.Text))
            {
                MsgBox.ShowWarn("请输入数据库用户名!");
                return false;
            }
            if (string.IsNullOrEmpty(txtServeIP.Text))
            {
                MsgBox.ShowWarn("请输入上位机地址!");
                return false;
            }
            if (string.IsNullOrEmpty(this.txtServePort.Text))
            {
                MsgBox.ShowWarn("请输入上位机端口!");
                return false;
            }
            return true;
        }


        private void DtToTxt(DataTable dt)
        {
            try
            {
                string Content = "";
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn cl in dt.Columns)
                    {
                        Content += cl.ColumnName + ":" + dr[cl.ColumnName].ToString() + ",";
                    }
                }
                if (!string.IsNullOrEmpty(Content))
                { Content = Content.Remove(Content.LastIndexOf(',')); }
                File.Delete(txtPath);
                File.AppendAllText(txtPath, Content, Encoding.UTF8);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmSysConnSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!File.Exists(txtPath)) { e.Cancel = true; return; }
                //this.DialogResult = DialogResult.Cancel;
                //this.Close();
                //return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
        #endregion


    }//end
}
