using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketServer;
using SocketModel;
using AGVCore;
using System.Threading;
using Tools;
using DevExpress.XtraEditors;
using Model.SimulateTcpCar;

namespace SimulatorAGVTCP
{
    public partial class FrmMain : DevExpress.XtraEditors.XtraForm
    {
        #region 属性
        //获取端口
        ServerConfig sconfig = new ServerConfig() { Port = 4000 };
        TCPSimulatorAGV tcpAgv = new TCPSimulatorAGV();
        SimulationAGVInstructions SendCommod = null;

        List<string> strs = new List<string>();
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object lockobj = new object();
        /// <summary>
        /// 显示日志信息
        /// </summary>
        public delegate void DispatchStateCallBack(List<string> msg, int fk, SimulationAGVInstructions carCallBack);
        public DispatchStateCallBack delshowMessage;

        public delegate void FocusCbo(SimulationAGVInstructions msg);
        public FocusCbo Cbo;

        private AppSession Session;
        #endregion

        public FrmMain()
        {
            InitializeComponent();
            SimulationAGVInstructions Item = new SimulationAGVInstructions();
            this.bsSource.DataSource = Item;
            this.bsSource.ResetBindings(false);
        }

        //启动
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                lblAGVID.Focus();
                tcpAgv.ReceiveMes -= ViewSendCommd;
                tcpAgv.ServerCarStates -= ViewS;
                delshowMessage = null;
                sconfig.Port = Convert.ToInt32(tbPort.Text);
                bool IsSuccess = tcpAgv.Setup(sconfig);
                //启动服务
                if (tcpAgv.Start())
                {
                    XtraMessageBox.Show("启动成功", "提示", MessageBoxButtons.OK);
                    //tcpAgv.ServerCarStates += ViewS;
                    //delshowMessage = ItemsAdd;
                }
                else
                {
                    XtraMessageBox.Show("启动失败", "提示", MessageBoxButtons.OK);
                }

                if (checkAutomaticManual.Checked == true)
                {
                    tcpAgv.ServerCarStates += ViewS;
                    delshowMessage = ItemsAdd;
                }
                else
                {
                    tcpAgv.ReceiveMes += ViewSendCommd;
                    delshowMessage = ItemsAdd;
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                //tcpAgv.ReceiveMes += ViewSendCommd;
                //delshowMessage = ItemsAdd;
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                tcpAgv.Stop();
                XtraMessageBox.Show("停止成功", "提示", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            { XtraMessageBox.Show("停止失败\r\n" + ex.Message, "提示", MessageBoxButtons.OK); }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 清空指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                listAction.Items.Clear();
                listHeartBeat.Items.Clear();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 客户端和服务端数据指令
        /// </summary>
        /// <param name="obj"></param>
        public void ViewSendCommd(object obj)
        {
            try
            {
                
                NetEventArgs args = obj as NetEventArgs;
                if (args != null)
                {
                    Session = args.Session;
                    List<byte> msg = args.MesContent;

                    #region 发送出客户端指令

                    string Str = "";
                    foreach (byte item in msg)
                    { Str += item.ToString("X") + " "; }
                    List<string> Result = new List<string>();
                    Result.Add(Str);
                    if (Result != null)
                    {
                        delshowMessage?.Invoke(Result, 2, null);
                    }

                    #endregion


                    #region 发送服务端指令

                    //if (Session.State == "1")
                    //{
                    //    this.tbLogicalSite.Text = "0";
                    //}
                    //else if (Session.State == "2")
                    //{
                    //    this.tbLogicalSite.Text = "5";
                    //}

                    SendCommod = bsSource.Current as SimulationAGVInstructions;



                    if (SendCommod != null)
                    {
                        List<byte> ByteCommond = tcpAgv.Unpack(Session, SendCommod);
                        List<string> CommondMesStr = new List<string>();
                        string MesStr = "";
                        foreach (byte item in ByteCommond)
                        { MesStr += item.ToString("X") + " "; }
                        CommondMesStr.Add(MesStr);
                        Session.ClientSocket.Send(ByteCommond.ToArray());
                        delshowMessage?.Invoke(CommondMesStr, 1, null);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            { throw ex; }

        }

        public void ViewS(AppSession Session, SimulationAGVInstructions SCar, List<byte> msg)
        {
            try
            {
                if (msg != null)
                {
                    string Str = "";
                    foreach (byte item in msg)
                    {
                        Str += item.ToString("X") + " ";
                    }
                    List<string> Result = new List<string>();
                    Result.Add(Str);
                    if (Result != null)
                    {
                        delshowMessage?.Invoke(Result, 2, null);
                    }
                }

                List<byte> ByteCommand = tcpAgv.Unpack(Session, SCar);
                List<string> NewState = new List<string>();
                string NewStateStr = "";
                foreach (var item in ByteCommand)
                {
                    NewStateStr += item.ToString("X") + " ";
                }
                NewState.Add(NewStateStr);
                Session.ClientSocket.Send(ByteCommand.ToArray());
                delshowMessage?.Invoke(NewState, 1, SCar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 接收和发送的指令，委托指向的方法
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="fk"></param>
        public void ItemsAdd(List<string> msg, int fk, SimulationAGVInstructions car)
        {
            SendCommod = new SimulationAGVInstructions();
            if (fk == 1)
            {

                try
                {
                    if (listHeartBeat.InvokeRequired)
                    {
                        this.listHeartBeat.Invoke(delshowMessage, new object[] { msg, fk, car });
                    }
                    else
                    {
                        lock (lockobj)
                        {
                            try
                            {
                                if (this.listHeartBeat.Items.Count > 35)
                                {
                                    this.listHeartBeat.Items.Remove(this.listHeartBeat.Items[0]);
                                }
                                if (ckEdit.Checked == true)
                                {
                                    List<string> po = msg.Where(k => k.Split(' ')[4] != "82").ToList();
                                    listHeartBeat.Items.AddRange(po.ToArray());
                                }
                                else
                                {
                                    foreach (var item in msg)
                                    { this.listHeartBeat.Items.Add(item); }
                                    if (car != null)
                                    {
                                        bsSource.DataSource = car;
                                        bsSource.ResetBindings(false);
                                        lblHookStatus.Focus();
                                    }
                                }
                                this.listHeartBeat.SelectedIndex = this.listHeartBeat.Items.Count - 1;//定位显示最后一条信息

                            }
                            catch
                            {
                                LogHelper.WriteLog("捕捉到异常");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteErrorLog(ex);
                }
            }
            else if (fk == 2)
            {
                try
                {
                    if (listAction.InvokeRequired)
                    {
                        this.listAction.Invoke(delshowMessage, new object[] { msg, fk, car });
                    }
                    else
                    {
                        lock (lockobj)
                        {
                            try
                            {
                                if (this.listAction.Items.Count > 35)
                                {
                                    this.listAction.Items.Remove(this.listAction.Items[0]);
                                }
                                if (ckEdit.Checked == true)
                                {
                                    List<string> pos = msg.Where(k => k.Split(' ')[4] != "2").ToList();
                                    listAction.Items.AddRange(pos.ToArray());
                                }
                                else
                                {
                                    foreach (var item in msg)
                                    { this.listAction.Items.Add(item); }
                                    strs.Clear();
                                }
                                this.listAction.SelectedIndex = this.listAction.Items.Count - 1;//定位显示最后一条信息 
                            }
                            catch
                            {
                                LogHelper.WriteLog("捕捉到异常");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteErrorLog(ex);
                }
            }

        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        private async void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblAGVID.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 显示和隐藏
        /// </summary>
        private void ckEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckEdit.Checked)
                {
                    listHeartBeat.Items.Clear();
                    listAction.Items.Clear();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
