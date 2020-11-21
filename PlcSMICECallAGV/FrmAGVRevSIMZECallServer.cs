using AGVCore;
using HslCommunication.Profinet.Siemens;
using Model.MDM;
using SQLServerOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace PlcSMICECallAGV
{
    public partial class FrmAGVRevSIMZECallServer : Form
    {
        public FrmAGVRevSIMZECallServer()
        {
            InitializeComponent();
        }

        #region 属性
        CommunicationSiemensPlc Communit { get; set; }
        IList<CommunicationSiemensPlc> Seccions = new List<CommunicationSiemensPlc>();
        /// <summary>
        /// 显示日志信息
        /// </summary>
        private DispatchStateCallBack delshowMessage;
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object lockobj = new object();
        #endregion


        #region 窗体事件
        private void FrmAGVRevSIMZECallServer_Load(object sender, EventArgs e)
        {
           
        }


        private void FrmAGVRevSIMZECallServer_Shown(object sender, EventArgs e)
        {
            try
            {

                Process[] processByName = Process.GetProcessesByName("PlcSMICECallAGV");
                if (processByName.Length > 1)
                {
                    this.FormClosing -= this.FrmAGVRevSIMZECallServer_FormClosing;
                    MessageBox.Show("已有PLC进程运行");
                    Application.Exit();
                    return;
                }
                //绑定日志显示
                delshowMessage = AddMessage;
                //绑定事件
                DelegateState.DispatchStateEvent += this.AddMessage;
                this.tmClear.Interval = 1000 * 60 * 1;//五分钟回收一次内存
                this.tmClear.Enabled = true;
                this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
                notifyIcon1.Visible = false;
                InitAndStartServer();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnToBot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Close();
                return;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void FrmAGVRevSIMZECallServer_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void FrmAGVRevSIMZECallServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定退出服务?", "询问", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Stop();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                    this.notifyIcon1.Visible = false;
                    this.ShowInTaskbar = true;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void FrmAGVRevSIMZECallServer_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.ShowInTaskbar = false;
                    this.notifyIcon1.Visible = true;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private void tmClear_Tick(object sender, EventArgs e)
        {
            try
            {
                tmClear.Enabled = false;
                ClearMemory();
                LogHelper.DeleteLogFile();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally
            { tmClear.Enabled = true; }
        }
        #endregion


        #region 自定义函数
        private bool InitAndStartServer()
        {
            try
            {
                DelegateState.InvokeDispatchStateEvent("正在读取数据库连接...");
                ConnectConfigTool.setDBase();
                if (ConnectConfigTool.DBase == null)
                {
                    DelegateState.InvokeDispatchStateEvent("数据库连接文件不存在!");
                    return false;
                }
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                if (dbOperator == null)
                {
                    DelegateState.InvokeDispatchStateEvent("数据库连接文件不存在!");
                    return false;
                }
                bool IsConnectDB = false;

                try
                {
                    IsConnectDB = dbOperator.ServerIsThrough();
                }
                catch (Exception ex)
                {
                    DelegateState.InvokeDispatchStateEvent("数据库连接失败【" + ex.Message + "】");
                }
                if (!IsConnectDB)
                { return false; }
                CoreData.dbOperator = dbOperator;
                #region 读取系统参数
                DelegateState.InvokeDispatchStateEvent("正在读取系统参数...");
                DataTable dtSysparameter = CoreData.dbOperator.LoadDatas("QuerySyspara");
                foreach (DataRow row in dtSysparameter.Rows)
                {
                    CoreData.SysParameter[row["ParameterCode"].ToString()] = row["ParameterValue"].ToString();
                }
                DelegateState.InvokeDispatchStateEvent("读取系统参数成功...");
                #endregion
                DataTable dtAllCallBox = CoreData.dbOperator.LoadDatas("QueryAllCallBox");
                CoreData.AllCallBoxes = DataToObject.TableToEntity<CallBoxInfo>(dtAllCallBox);
                DataTable dtAllCallBoxDetail = CoreData.dbOperator.LoadDatas("QueryAllCallBoxDetails");
                CoreData.AllCallBoxDetail = DataToObject.TableToEntity<CallBoxDetail>(dtAllCallBoxDetail);
                //读取储位
                DataTable dtstorage = CoreData.dbOperator.LoadDatas("QueryAllStore");
                CoreData.StorageList = DataToObject.TableToEntity<StorageInfo>(dtstorage);

                //读取区域
                DataTable dtarea = CoreData.dbOperator.LoadDatas("LoadAllArea");
                CoreData.AllAreaList = DataToObject.TableToEntity<AreaInfo>(dtarea);

                //初始化呼叫器通信
                Seccions.Clear();
                foreach (CallBoxInfo callbox in CoreData.AllCallBoxes)
                {
                    //if (callbox.CallBoxID != 5)
                    //{
                    //    continue;
                    //}
                    SiemensConnectInfo ConnConfig = new SiemensConnectInfo() { ServerIP = callbox.CallBoxIP, Port = callbox.CallBoxPort };

                    CommunicationSiemensPlc Commer = new CommunicationSiemensPlc(callbox.CallBoxID, ConnConfig);
                    if (!Commer.InitSiemens())
                    {
                        DelegateState.InvokeDispatchStateEvent("初始化呼叫器:" + callbox.CallBoxID.ToString() + "IP:" + "callbox.CallBoxIP" + "失败!");
                        continue;
                    }
                    Commer.Start(); 
                    Seccions.Add(Commer); 
                }
                DelegateState.InvokeDispatchStateEvent("初始化成功...");
                return true;
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent(ex.Message);
                return false;
            }
        }

        private bool Stop()
        {
            try
            {
                foreach (CommunicationSiemensPlc commer in Seccions)
                {
                    if (!commer.Stop())
                    {
                        DelegateState.InvokeDispatchStateEvent("呼叫器:" + commer.PLCCode.ToString() + "IP:" + commer.ConnParam.ServerIP + "停止失败!");
                    }
                    else
                    {
                        DelegateState.InvokeDispatchStateEvent("呼叫器:" + commer.PLCCode.ToString() + "IP:" + commer.ConnParam.ServerIP + "停止成功!");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="msg"></param>
        private void AddMessage(string msg)
        {
            try
            {
                if (listboxtask.InvokeRequired)
                {
                    this.listboxtask.Invoke(delshowMessage, new object[] { msg });
                }
                else
                {
                    lock (lockobj)
                    {
                        try
                        {
                            if (this.listboxtask.Items.Count > 50)
                            {
                                this.listboxtask.Items.Remove(this.listboxtask.Items[0]);
                            }
                            this.listboxtask.Items.Add(msg);
                            this.listboxtask.SelectedIndex = this.listboxtask.Items.Count - 1;//定位显示最后一条信息 
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


        #region 内存回收
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        #endregion
        #endregion


    }
}
