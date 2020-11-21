using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class StorageInfo
    {
        public StorageInfo()
        {
            StorageName = "";
            LankMarkCode = "";
        }

        public int ID { get; set; }

        public string StorageName { get; set; }

        public int OwnArea { get; set; }

        public int SubOwnArea { get; set; }

        public int matterType { get; set; }

        public string LankMarkCode { get; set; }

        public string ActionLandMarkCode { get; set; }

        public int StorageState { get; set; }

        public int LockState { get; set; }

        /// <summary>
        /// 锁定车辆
        /// </summary>
        public int LockCar { get; set; }

        /// <summary>
        /// 物料类型[0 | 1010]  1 | 1515
        /// </summary>
        public int MaterielType { get; set; }

        /// <summary>
        /// AGV到达时间
        /// </summary>
        public string AGVArriveTime { get; set; }

        /// <summary>
        /// 监控操作修改时间
        /// </summary>
        public string HandTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }
}
