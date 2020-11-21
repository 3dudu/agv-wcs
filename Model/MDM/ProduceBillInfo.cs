using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class ProduceBillInfo
    {
        public ProduceBillInfo()
        {
            ProduceBillID = "";
            BuildDate = "";
            SubmitDate = "";
            FinishDate = "";
            MachineTypeStr = "";
            IsNew = false;
        }

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsChoose { get; set; }

        public string ProduceBillID { get; set; }
        /// <summary>
        /// 机种类型 0金、1铜、2共阴、3共阳
        /// </summary>
        public int MachineType { get; set; }

        /// <summary>
        /// 状态[0 未提交 1已提交 2 完成]
        /// </summary>
        public int BillState { get; set; }

        public bool IsNew { get; set; }
        public string MachineTypeStr { get; set; }


        /// <summary>
        /// 创建日期
        /// </summary>
        public string BuildDate { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        public string SubmitDate { get; set; }

        /// <summary>
        /// 完成日期
        /// </summary>
        public string FinishDate { get; set; }

        /// <summary>
        /// RGB类型 0 红光 1 绿光 2 蓝光
        /// </summary>
        public int RGBType { get; set; }
    }
}
