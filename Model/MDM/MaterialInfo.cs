using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class MaterialInfo
    {
        public MaterialInfo()
        {
            MaterialName = "";
        }
        /// <summary>
        /// 物料类型
        /// </summary>
        public int MaterialType { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 夹抱大小
        /// </summary>
        public int PickSize { get; set; }

        public string PickSizeStr
        {
            get
            {
                switch (PickSize)
                {
                    case 0:
                        return "小";
                    case 1:
                        return "中";
                    default:
                        return "大";
                }
            }
            set
            {
                switch (value)
                {
                    case "小":
                        PickSize = 0;
                        break;
                    case "中":
                        PickSize = 1;
                        break;
                    default:
                        PickSize = 2;
                        break;
                }
            }
        }
    }
}
