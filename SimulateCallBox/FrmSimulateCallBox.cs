using SocketClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using SocketModel;
using Tools;
using System.IO;

namespace SimulateCallBox
{
    public partial class FrmSimulateCallBox : Form
    {
        /// <summary>
        /// 客户端服务
        /// </summary>
        private static CallBoxTcpClientServer client = null;
        /// <summary>
        /// 客户端端口、ip配置
        /// </summary>
        private static ClientConfig config = null;
        /// <summary>
        /// 心跳定时器
        /// </summary>
        private System.Timers.Timer tmSendHeartBeat = null;
        /// <summary>
        /// 重连定时器
        /// </summary>
        private System.Timers.Timer tmReconnect = null;
        /// <summary>
        /// 发送包
        /// </summary>
        List<byte> byteAll = new List<byte>();
        /// <summary>
        ///除去开头两个字节的包
        /// </summary>
        byte[] bytePack = new byte[] { 0x00, 0x00, 0x00, 0x06, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00 };
        public FrmSimulateCallBox()
        {
            InitializeComponent();
            //this.skinEngine = new Sunisoft.IrisSkin.SkinEngine((System.ComponentModel.Component)(this));
            //this.skinEngine.SkinFile = Application.StartupPath + "//DeepCyan.ssk";
            client = new CallBoxTcpClientServer();
            config = new ClientConfig();
            client.RecvSuccess += RecMes;
            //bytePack.AddRange(new byte[] {0x00,0x88,0x00,0x00,0x00,0x06,0x00,0x00,0x00,0x00,0x00});
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                config.ServerIP = "127.0.0.1";
                config.Port = 7000;
                if (!client.Setup(config))
                {
                    MessageBox.Show("初始化连接失败");
                    return;
                }
                bool flag = client.Start();
                if (flag)
                {
                    MessageBox.Show("连接成功");
                    byteAll.AddRange(new byte[] { 0x00, 0x00 });
                    byteAll.AddRange(bytePack);
                    if (tmSendHeartBeat == null)
                    {
                        tmSendHeartBeat = new System.Timers.Timer(2000);
                        tmSendHeartBeat.Enabled = true;
                        tmSendHeartBeat.Elapsed += heartTimesUp;
                        tmReconnect = new System.Timers.Timer(3000);
                        tmReconnect.Enabled = true;
                        tmReconnect.Elapsed += Reconnect;
                    }
                    textScreen.Text = DateTime.Now + "，呼叫盒连接调度系统成功";
                }
                else
                {
                    MessageBox.Show("连接失败");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 定时触发重连
        /// </summary>
        private void Reconnect(object sender, ElapsedEventArgs e)
        {
            try
            {
                tmReconnect.Enabled = false;
                DateTime time = DateTime.Now;
                if (preRecTime == null)
                { return; }
                TimeSpan timeSpan = time - preRecTime;
                if (timeSpan.TotalSeconds > 2)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        textScreen.Text = DateTime.Now + "，呼叫盒正在尝试重连至服务";
                    }));
                    if (client != null)
                    {
                        client.Exit();
                        client = null;
                    }
                    client = new CallBoxTcpClientServer();
                    client.RecvSuccess += RecMes;
                    if (client.Setup(config))
                    {
                        if (client.Start())
                        {
                            //重连成功，启动心跳
                            if (tmSendHeartBeat != null)
                            {
                                tmSendHeartBeat.Enabled = true;
                            }
                            this.Invoke((EventHandler)(delegate
                            {
                                textScreen.Text = DateTime.Now + "，呼叫盒已重新连接";
                            }));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                tmReconnect.Enabled = true;
            }
        }
        /// <summary>
        /// 上次收到心跳的时间
        /// </summary>
        DateTime preRecTime;

        private void RecMes(object sender, PackageInfo packinfo)
        {
            try
            {
                preRecTime = DateTime.Now;
                string pack = packinfo.PackContent;
                string[] increment = pack.Split('-');
                string head = increment[0] + increment[1];
                //因为前两个字节组合起来是一个事务号，所以将前两个16进制字符串拼在一起转成整型
                int a = Convert.ToInt16(head, 16);
                a += 1;
                if (a > 65535)
                {
                    byteAll.Clear();
                    byteAll.AddRange(new byte[] { 0x00, 0x00 });
                    byteAll.AddRange(bytePack);
                }
                else
                {
                    byte[] add = BitConverter.GetBytes(a);
                    byteAll.Clear();
                    byteAll.Add(add[1]);
                    byteAll.Add(add[0]);
                    byteAll.AddRange(bytePack);
                }
                if (this.txtShow.InvokeRequired)
                {
                    txtShow.Invoke(new Action<string>(s =>
                    {
                        this.txtShow.Text = s;
                    }), BitConverter.ToString(byteAll.ToArray()));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 记录心跳日志
        /// </summary>
        /// <param name="msg"></param>
        //private void Log(string msg)
        //{
        //    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Log\\log.txt", msg + "\r\n");
        //}

        /// <summary>
        /// 发送心跳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heartTimesUp(object sender, ElapsedEventArgs e)
        {
            try
            {
                tmSendHeartBeat.Enabled = false;
                lock (lockSendHeart)
                {
                    client.SendMessage(byteAll.ToArray());
                }
                tmSendHeartBeat.Enabled = true;
            }
            catch (Exception ex)
            {
                tmSendHeartBeat.Enabled = false;
                this.Invoke((EventHandler)(delegate
                {
                    textScreen.Text = DateTime.Now + "，调度服务关闭，连接中断";
                }));
                throw ex;
            }
        }
        /// <summary>
        /// 心跳锁
        /// </summary>
        private static object lockSendHeart = new object();

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string txtHeartBeat = txtHeart.Text.Trim();
                if (txtHeartBeat == "")
                { return; }
                double heart = 0.0;
                try
                {
                    heart = Convert.ToDouble(txtHeartBeat);
                }
                catch (Exception)
                {
                    MessageBox.Show("请输入数字");
                    return;
                }
                if (tmSendHeartBeat != null)
                {
                    tmSendHeartBeat.Interval = heart;
                }else
                {
                    MessageBox.Show("请先连接至服务!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void circleBtnOne_Click(object sender, EventArgs e)
        {
            try
            {
                byte temp = 1;
                bytePack[9] = temp;
                //string hexStr = BitConverter.ToString(bytePack);
                //this.txtShow.Text = hexStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void circleBtnTwo_Click(object sender, EventArgs e)
        {
            try
            {
                byte temp = 2;
                bytePack[9] = temp;
                //string hexStr = BitConverter.ToString(bytePack);
                //this.txtShow.Text = hexStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void circleBtnThree_Click(object sender, EventArgs e)
        {
            try
            {
                byte temp = 4;
                bytePack[9] = temp;
                //string hexStr = BitConverter.ToString(bytePack);
                //this.txtShow.Text = hexStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void circleBtnFour_Click(object sender, EventArgs e)
        {
            try
            {
                byte temp = 8;
                bytePack[9] = temp;
                //string hexStr = BitConverter.ToString(bytePack);
                //this.txtShow.Text = hexStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void circleBtnFive_Click(object sender, EventArgs e)
        {
            try
            {
                byte temp = 16;
                bytePack[9] = temp;
                //string hexStr = BitConverter.ToString(bytePack);
                //this.txtShow.Text = hexStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void circleBtnSix_Click(object sender, EventArgs e)
        {
            try
            {
                byte temp = 32;
                bytePack[9] = temp;
                //string hexStr = BitConverter.ToString(bytePack);
                //this.txtShow.Text = hexStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void circleBtnSeven_Click(object sender, EventArgs e)
        {
            try
            {
                byte temp = 64;
                bytePack[9] = temp;
                //string hexStr = BitConverter.ToString(bytePack);
                //this.txtShow.Text = hexStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CircleBtnEight_Click(object sender, EventArgs e)
        {
            try
            {
                byte temp = 128;
                bytePack[9] = temp;
                //string hexStr = BitConverter.ToString(bytePack);
                //this.txtShow.Text = hexStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 关闭窗体时，关闭连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSimulateCallBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (client != null)
                { client.Exit(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
