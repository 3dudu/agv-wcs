using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    public class TraJunctionSegInfo
    {

        public TraJunctionSegInfo()
        {
            LandCodes = "";
        }
        public int TraJunctionID { get; set; }

        public int SegmentID { get; set;}

        public string LandCodes { get; set; }
    }
}
