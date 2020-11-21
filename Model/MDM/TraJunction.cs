using Model.CarInfoExtend;
using Model.Comoon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class TraJunction
    {
        public TraJunction()
        {
            TraJunctionID = 1;
            Carnumber = 1;
            JunctionLandMarkCodes = "";
            JuncLandCodes = new List<string>();
            Segments = new List<TraJunctionSegInfo>();
            LockCars = new List<CarInfo>();
        }
        /// <summary>
        /// ID
        /// </summary>
        public int TraJunctionID { get; set; }
        /// <summary>
        /// 小车数量
        /// </summary>
        public int  Carnumber { get; set; }
        /// <summary>
        /// 地标集（格式: ,0001,0002,）
        /// </summary>
        public string JunctionLandMarkCodes { get; set; }

        /// <summary>
        /// 地标集合
        /// </summary>
        public IList<string> JuncLandCodes { get; set; }

        /// <summary>
        /// 对应的配置线段
        /// </summary>
        public IList<TraJunctionSegInfo> Segments { get; set; }

        /// <summary>
        /// 锁住当前资源集的车辆集合
        /// </summary>
        public IList<CarInfo> LockCars { get; set; }
    }
}
