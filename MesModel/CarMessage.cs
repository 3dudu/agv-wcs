using Model.CarInfoExtend;
using System;
using System.Collections.Generic;
using System.Text;

namespace MesModel
{
    public class CarMessage
    {
        public CarMessage()
        {
            Plant = "";
            BH = "";
            Coord = "";
        }

        public string Plant { get; set; }

        public string BH { get; set; }

        public string Coord { get; set; }

        public void setValue(CarInfo car)
        {
            Plant = "JBCJ";
            BH = car.AgvID.ToString();
            Coord = car.CurrSite.ToString();
        }

        public void setValue(string AGVID,string CurrSite)
        {
            Plant = "JBCJ";
            BH = AGVID;
            Coord = CurrSite;
        }
    }
}
