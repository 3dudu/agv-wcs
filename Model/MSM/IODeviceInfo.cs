using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MSM
{
    /// <summary>
    /// 远程IO设备档案主表
    /// </summary>
    public class IODeviceInfo
    {
        public IODeviceInfo()
        {
            DeviceName = "";
            IP = "";
            Port = "";
        }

        /// <summary>
        /// IO设备ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// IO设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        ///IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 是否掉线
        /// </summary>
        public bool bIsCommBreak { get; set; }

        /// <summary>
        /// DI口状态
        /// </summary>
        public IList<IOPortInfo> DIPortList { get; set; }


        public bool IsChange(IODeviceInfo IOInfo)
        {
            if (IOInfo.bIsCommBreak != this.bIsCommBreak)
            { return true; }
            else
            { return false; }
        }

        public void GetValue(IODeviceInfo IOInfo)
        {
            this.bIsCommBreak = IOInfo.bIsCommBreak;
        }
    }//end IODeviceInfo

    /// <summary>
    /// IO设备对应端子口信息
    /// </summary>
    public class IOPortInfo
    {
        /// <summary>
        /// 口号
        /// </summary>
        public int PortNo { get; set; }

        /// <summary>
        /// 口状态
        /// </summary>
        public int PortState { get; set; }
    }//end IOPortInfo
}//end
