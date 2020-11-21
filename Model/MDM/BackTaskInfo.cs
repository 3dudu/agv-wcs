using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    public class BackTaskInfo
    {
        public BackTaskInfo()
        {
            UserID = "AGVS";
            Password = "B6V#*987";
        }
        public string UserID { get; set; }

        public string Password { get; set; }

        public string TaskNo { get; set; }

        public string TaskState { get; set; }
    }
}
