using AGVCommunication;
using AGVCore;
using DipatchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace AGVDispacth
{
    public class NonstandardDispatch
    {
        #region 属性
        //主调度实例
        Dispacther Dispacther { get; set; }
        CommunicationBase Commer { get; set; }
        #endregion

        public NonstandardDispatch(Dispacther mainDispath)
        {
            Dispacther = mainDispath;
        }
        public void AGV_AddCommand(int agvid, CommandToValue ctov)
        {
            try
            {
                Commer.AGV_AddControl(agvid, ctov);
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }
        public void WriteRegister(int agvid, int opentype)//0降 1升
        {
            List<CommandForkliftEnum> ForkliftList = new List<CommandForkliftEnum>();
            CommandForkliftEnum Forklift = null;
            //起始地标
            //开始位
            Forklift = new CommandForkliftEnum();
            if (opentype == 1)
            {
                Forklift.ForkAction = 2;
                Forklift.height = 1;
            }
            else
            {
                Forklift.ForkAction = 1;
                Forklift.height = 0;
            }
            ForkliftList.Add(Forklift);
            CommandToValue CommandTo = new CommandToValue(AGVCommandEnum.WriteRegister1);
            CommandTo.CommandValue = ForkliftList;
            AGV_AddCommand(agvid, CommandTo);
        }

    }
}
