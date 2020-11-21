using Model.CarInfoExtend;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using SQLServerOperator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVCore
{
    /// <summary>
    /// 初始化调度系统所有的档案信息
    /// </summary>
    public static class CoreData
    {
        /// <summary>
        /// 全局服务
        /// </summary>
        public static bool IsGlobServer { get; set; }
        /// <summary>
        /// 数据库信息
        /// </summary>
        public static IDbOperator dbOperator { get; set; }

        /// <summary>
        /// 系统参数集合
        /// </summary>
        public static IDictionary<string, string> SysParameter = new Dictionary<string, string>();

        /// <summary>
        /// 数据库链接信息
        /// </summary>
        public static DbHelperSQL DbHelper = new DbHelperSQL();

        /// <summary>
        /// AGV集合
        /// </summary>
        public static IList<CarInfo> CarList = new List<CarInfo>();

        /// <summary>
        /// 所有呼叫器
        /// </summary>
        public static IList<CallBoxInfo> AllCallBoxes = new List<CallBoxInfo>();

        /// <summary>
        /// 所有呼叫器明细
        /// </summary>
        public static IList<CallBoxDetail> AllCallBoxDetail = new List<CallBoxDetail>();

        /// <summary>
        /// 所有充电桩
        /// </summary>
        public static IList<ChargeStationInfo> ChargeList = new List<ChargeStationInfo>();
        /// <summary>
        /// 所有IO设备
        /// </summary>
        public static IList<IODeviceInfo> IOList = new List<IODeviceInfo>();
        /// <summary>
        /// 所有储位集合
        /// </summary>
        public static IList<StorageInfo> StorageList = new List<StorageInfo>();

        /// <summary>
        /// 所有线段
        /// </summary>
        public static IList<AllSegment> AllSeg = new List<AllSegment>();

        /// <summary>
        /// 所有地标
        /// </summary>
        public static IList<LandmarkInfo> AllLands = new List<LandmarkInfo>();

        /// <summary>
        /// 所有物料信息
        /// </summary>
        public static IList<MaterialInfo> AllMaterials = new List<MaterialInfo>();

        /// <summary>
        /// agv地图坐标对照
        /// </summary>
        public static DataTable AGVCoordinate = new DataTable();

        /// <summary>
        /// 地图与实际尺寸的缩放比例
        /// </summary>
        public static int ScalingRate { get; set; }

        /// <summary>
        /// 交通管制路口档案集合
        /// </summary>
        public static IList<TraJunction> JunctionList = new List<TraJunction>();

        /// <summary>
        /// 所有线路线段配置
        /// </summary>
        public static IList<RouteFragmentConfigInfo> RouteFrages = new List<RouteFragmentConfigInfo>();

        /// <summary>
        /// 所有平板信息
        /// </summary>
        public static IList<PdaInfo> AllPads = new List<PdaInfo>();

        /// <summary>
        /// 所有的平板操作设置
        /// </summary>
        public static IList<PdaOperSetInfo> PadOperSets = new List<PdaOperSetInfo>();

        /// <summary>
        /// 所有的指令信息
        /// </summary>
        public static IList<CmdInfo> Cmdes = new List<CmdInfo>();

        /// <summary>
        /// 调度程序类配置信息
        /// </summary>
        public static IList<DispatchAssembly> DispathAssemblies = new List<DispatchAssembly>();

        /// <summary>
        /// 所有区域档案
        /// </summary>
        public static IList<AreaInfo> AllAreaList = new List<AreaInfo>();

        /// <summary>
        /// AGV记忆信息xml文件目录
        /// </summary>
        public static string RemerberFilePath= System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
    }
}
