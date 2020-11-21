using DevExpress.Utils;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tools;

namespace PlcSMICECallAGV
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        SiemensS7Net melsecMc = null;

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            try
            {
                this.btnConnect.Enabled = true;
                this.btnOff.Enabled = false;
                this.btnRead.Enabled = false;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string IP = txtIP.Text.Trim();
                if (string.IsNullOrEmpty(IP))
                {
                    MessageBox.Show("请输入IP地址!");
                    return;
                }
                string Port = txtPort.Text.Trim();
                if (string.IsNullOrEmpty(Port))
                {
                    MessageBox.Show("请输入端口号!");
                    return;
                }
                OperateResult connect;
                using (WaitDialogForm dialog = new WaitDialogForm("正在连接,请稍后...", "提示"))
                {
                    int port = Convert.ToInt16(Port);
                    melsecMc = new SiemensS7Net(SiemensPLCS.S1200) { IpAddress = IP, Port = port, ConnectTimeOut = 5000, ReceiveTimeOut = 5000 };
                    melsecMc.ConnectClose();
                    connect = melsecMc.ConnectServer();
                }
                if (!connect.IsSuccess)
                {
                    MessageBox.Show("连接失败:" + connect.Message);
                    return;
                }
                else
                {
                    MessageBox.Show("连接成功");
                    btnConnect.Enabled = false;
                    btnRead.Enabled = true;
                    btnOff.Enabled = true;
                    return;
                }

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                string readaddr = txtReadAddr.Text.Trim();
                if (string.IsNullOrEmpty(readaddr))
                {
                    MessageBox.Show("请输入正确的读取地址");
                    return;
                }

                OperateResult<short[]> opr = melsecMc.ReadInt16(readaddr,15);
                if (opr.IsSuccess)
                {
                    string codes = "";
                    foreach (short ts in opr.Content.Skip(3))
                    {
                        codes += ts.ToString() + ",";
                    }

                    //物料编码
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[6-3])[1]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[6-3])[0]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[7-3])[1]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[7-3])[0]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[8-3])[1]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[8-3])[0]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[9-3])[1]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[9-3])[0]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[10-3])[1]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[10-3])[0]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[11-3])[1]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[11-3])[0]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[12-3])[1]));
                    sb1.Append(Chr(BitConverter.GetBytes(opr.Content[12 - 3])[0]));
                   var s = sb1.ToString();
                    StringBuilder sb2 = new StringBuilder();  
                    sb2.Append(Chr(BitConverter.GetBytes(opr.Content[13-3])[1]));
                    sb2.Append(Chr(BitConverter.GetBytes(opr.Content[13-3])[0]));
                    sb2.Append(Chr(BitConverter.GetBytes(opr.Content[14-3])[1]));
                    sb2.Append(Chr(BitConverter.GetBytes(opr.Content[14-3])[0]));
                    sb2.Append(Chr(BitConverter.GetBytes(opr.Content[15-3])[1]));
                    sb2.Append(Chr(BitConverter.GetBytes(opr.Content[15-3])[0]));
                    sb2.Append(Chr(BitConverter.GetBytes(opr.Content[16-3])[1]));
                    sb2.Append(Chr(BitConverter.GetBytes(opr.Content[16-3])[0]));
                    sb2.Append(Chr(BitConverter.GetBytes(opr.Content[17-3])[1]));
                    sb2.Append(Chr(BitConverter.GetBytes(opr.Content[17-3])[0]));
                    s = sb2.ToString();
                }

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (melsecMc != null)
                {
                    OperateResult connect = melsecMc.ConnectClose();
                    if (!connect.IsSuccess)
                    {
                        MessageBox.Show("关闭连接失败:" + connect.Message);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("关闭连接成功!");
                        this.btnConnect.Enabled = true;
                        this.btnOff.Enabled = false;
                        this.btnRead.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {

                //EB 90 D1 5 17 14 0 0 0     FF FF FA E0     FF FF DF 3D 0 4 1E B0 0 1 0 5D 0 25 
                this.txtRecv.Text = "";
                byte[] bytes = new byte[4] { 0x65, 0xDF, 0xFF,0xFF};
                float X = BitConverter.ToInt32(bytes, 0) / 1000.00F;  // -1.312
                byte[]  dd = BitConverter.GetBytes((int)(-1.29999995*1000));
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        public static string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("无效的ASCII码");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
            List<byte> b = new List<byte>();

            string[] arr = this.textBox1.Text.Split(',');
            foreach (var s in arr)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    short ts = short.Parse(s);
                    b.Add(BitConverter.GetBytes(ts)[1]);
                    b.Add(BitConverter.GetBytes(ts)[0]);
                }
            }
            //b.Add(BitConverter.GetBytes(Convert.ToInt16(this.textBox1.Text))[1]);
            //b.Add(BitConverter.GetBytes(Convert.ToInt16(this.textBox1.Text))[0]);
            melsecMc.Write("DB1000.12",b.ToArray());

            melsecMc.Write("DB1000.12", b.ToArray());
        }
    }
}
