using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class TrafficController
    {
        public TrafficController()
        {
            EnterP = "";
            EnterLandCode = "";
            JunctionP = "";
            JunctionLandMarkCodes = "";
            RealseLandMarkCode = "";
            ID = 0;
            Description = "";
            LandMarkList = "";
        }
        public int ID { get; set; }
        public string Description { get; set; }
        public string LandMarkList { get; set; }

        public int JunctionID { get; set; }

        public string EnterP { get; set; }

        public string EnterLandCode { get; set; }

        public string JunctionP { get; set; }

        public string JunctionLandMarkCodes { get; set; }

        public string RealseLandMarkCode { get; set; }
    }
}
