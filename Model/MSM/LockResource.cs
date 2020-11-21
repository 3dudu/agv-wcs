using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MSM
{
    [Serializable]
    public class LockResource
    {
        /// <summary>
        /// 交管锁车
        /// </summary>
        public int LockCarID { get; set; }

        /// <summary>
        /// 交管被锁车
        /// </summary>
        public int BeLockCarID { get; set; }
    }
}
