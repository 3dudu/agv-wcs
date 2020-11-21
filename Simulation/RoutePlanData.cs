using AGVDAccess;
using AGVModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Simulation
{
    public class RoutePlanData
    {
        public List<LandmarkInfo> RouteList = new List<LandmarkInfo>();
        public List<LandmarkInfo> CloseList = new List<LandmarkInfo>();
        public Stack PivotAnotherLand = new Stack();
        private IDictionary dic_Land = new Hashtable();
        private IList<SegmentResInfo> Segments;
        private int OpenCount = 0;
        private LandmarkInfo BeginLand = null;
        private LandmarkInfo EndLand = null;
        private IList<LandmarkInfo> AllLands = new List<LandmarkInfo>();

        public RoutePlanData(IList<SegmentResInfo> Segs)
        {
            Segments = Segs;
            AllLands = AGVClientDAccess.LoadLandByCondition("1=1");
        }

        public int OpenAmount
        {
            get
            {
                if (OpenCount == 0)
                {
                    OpenCount = AGVClientDAccess.LandAmount();
                }
                return OpenCount;
            }
        }


        public List<LandmarkInfo> GetRoute(LandmarkInfo StartLand, LandmarkInfo EndLand)
        {
            try
            {
                List<List<LandmarkInfo>> hs_route = new List<List<LandmarkInfo>>();
                AcountRoute(StartLand, EndLand);
                hs_route.Add(CreateDeepCopy<List<LandmarkInfo>>(RouteList));
                List<LandmarkInfo> OrialRoute = CreateDeepCopy<List<LandmarkInfo>>(RouteList);
                List<LandmarkInfo> OrialClose = CreateDeepCopy<List<LandmarkInfo>>(CloseList);
                Stack NewPivotAnotherLand = CreateDeepCopy<Stack>(PivotAnotherLand);
                while (NewPivotAnotherLand.Count > 0)
                {
                    PivotAnotherLand.Clear();
                    RouteList = CreateDeepCopy<List<LandmarkInfo>>(OrialRoute);
                    CloseList = CreateDeepCopy<List<LandmarkInfo>>(OrialClose);
                    Hashtable hs = NewPivotAnotherLand.Pop() as Hashtable;
                    ArrayList keyList = new ArrayList(hs.Keys);
                    LandmarkInfo PivotLand = hs[keyList[0].ToString()] as LandmarkInfo;

                  

                    int index = RouteList.FindIndex(p => p.LandmarkCode == keyList[0].ToString()) + 1;
                    int indexClose = CloseList.FindIndex(p => p.LandmarkCode == keyList[0].ToString()) + 1;
                    RouteList.RemoveRange(index, RouteList.Count - index);
                    CloseList.RemoveRange(indexClose, CloseList.Count - indexClose);
                    AcountRoute(PivotLand, EndLand);
                    hs_route.Add(CreateDeepCopy<List<LandmarkInfo>>(RouteList));
                }
                List<LandmarkInfo> Routes = null;
                SysParameter sys = AGVClientDAccess.GetParameterByCode("RouteCountMode");
                if (sys!=null)
                {
                    if (sys.ParameterValue == "最少地标数")
                    {
                        Routes = (from a in hs_route
                                  where a.Where(p => p.LandmarkCode == EndLand.LandmarkCode).Count() > 0
                                  orderby a.Count ascending
                                  select a).FirstOrDefault();
                    }
                    else if (sys.ParameterValue == "最少拐弯")
                    {
                        Routes = (from a in hs_route
                                  where a.Where(p => p.LandmarkCode == EndLand.LandmarkCode).Count() > 0
                                  orderby a.Count ascending
                                  select a).FirstOrDefault();

                        Routes = (from a in hs_route
                                  where a.Where(p => p.LandmarkCode == EndLand.LandmarkCode).Count() > 0
                                  orderby AcountInflect(a) ascending
                                  select a).FirstOrDefault();
                    }
                }
                else
                {
                    Routes = (from a in hs_route
                              where a.Where(p => p.LandmarkCode == EndLand.LandmarkCode).Count() > 0
                              orderby a.Count ascending
                              select a).FirstOrDefault();
                }
                if (Routes == null)
                { Routes = new List<LandmarkInfo>(); }
                //this.RouteList = Routes;
                AcountDirect(ref Routes);
                return Routes;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void AcountRoute(LandmarkInfo StarLand, LandmarkInfo EndLand)
        {
            try
            {
                if (Segments.Count > 0)
                {
                    if (this.BeginLand == null) { this.BeginLand = CreateDeepCopy<LandmarkInfo>(StarLand); }
                    if (this.EndLand == null) { this.EndLand = CreateDeepCopy<LandmarkInfo>(EndLand); }
                    LandmarkInfo PreLand = StarLand;
                    if (CloseList.Count < OpenAmount && PreLand.LandmarkCode != EndLand.LandmarkCode)
                    {
                        if (CloseList.Where(p => p.LandmarkCode == StarLand.LandmarkCode).Count() <= 0)
                        {
                            RouteList.Add(StarLand);
                            CloseList.Add(StarLand);
                        }
                        List<LandmarkInfo> lands = getNextLand(PreLand);
                        if (lands.Count == 0)
                        {
                            if (PivotAnotherLand.Count > 0)
                            {
                                Hashtable hs = PivotAnotherLand.Pop() as Hashtable;
                                ArrayList keyList = new ArrayList(hs.Keys);
                                LandmarkInfo PivotLand = hs[keyList[0].ToString()] as LandmarkInfo;
                                int index = RouteList.FindIndex(p => p.LandmarkCode == keyList[0].ToString()) + 1;
                                RouteList.RemoveRange(index, RouteList.Count - index);
                                PreLand = PivotLand;
                            }
                            else
                            { return; }
                        }
                        else if (lands.Count == 1)
                        {
                            if (CloseList.Where(p => p.LandmarkCode == lands[0].LandmarkCode).Count() <= 0)
                            {
                                RouteList.Add(lands[0]);
                                CloseList.Add(lands[0]);
                            }
                            PreLand = lands[0];
                        }
                        else
                        {
                            List<LandmarkInfo> CloseLand = (from a in lands
                                                            orderby getDistant(a.LandX, a.LandY, EndLand.LandX, EndLand.LandY) ascending
                                                            select a).ToList();

                            if (CloseLand == null)
                            {
                                throw (new Exception("计算路径异常!"));
                            }
                            else
                            {
                                if (CloseList.Where(p => p.LandmarkCode == CloseLand[0].LandmarkCode).Count() <= 0)
                                {
                                    RouteList.Add(CloseLand[0]);
                                    CloseList.Add(CloseLand[0]);
                                    Hashtable hs = new Hashtable();
                                    //for (int i=1;i< CloseLand.Count;i++)
                                    //{
                                    //    LandmarkInfo item = CloseLand[i];
                                    //    hs[PreLand.LandmarkCode] = PreLand;
                                    //    PivotAnotherLand.Push(hs);
                                    //}
                                    hs[PreLand.LandmarkCode] = CloseLand[1];
                                    PivotAnotherLand.Push(hs);
                                }
                                PreLand = CloseLand[0];
                            }
                        }
                        AcountRoute(PreLand, EndLand);
                    }
                }//end
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 得到下一个地标
        /// </summary>
        /// <returns></returns>
        private List<LandmarkInfo> getNextLand(LandmarkInfo PreLand)
        {
            try
            {
                List<LandmarkInfo> lands = new List<LandmarkInfo>();
                List<SegmentResInfo> Children = Segments.Where(p => p.StartLandMark.LandmarkCode == PreLand.LandmarkCode).ToList();
                foreach (SegmentResInfo item in Children)
                {
                    if (CloseList.Where(p => p.LandmarkCode == item.EndLandMark.LandmarkCode).Count() <= 0)
                    { lands.Add(item.EndLandMark); }
                }
                return lands;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 计算两点之间距离
        /// </summary>
        /// <returns></returns>
        private double getDistant(double X1, double Y1, double X2, double Y2)
        {
            try
            {
                double distant = Math.Sqrt(Math.Pow(Math.Abs(X1 - X2), 2) + Math.Pow(Math.Abs(Y1 - Y2), 2));
                return distant;
            }
            catch (Exception ex)
            { throw ex; }
        }

        //copy对象
        public T CreateDeepCopy<T>(T obj)
        {
            T t;
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            t = (T)formatter.Deserialize(memoryStream);
            return t;
        }


        //计算转向和前进和后退
        public void AcountDirect(ref List<LandmarkInfo> Routes)
        {
            try
            {
                IList<AllSegment>AllSegs = AGVClientDAccess.LoadAllSegment();
                DataTable dtCoor = AGVClientDAccess.LoadAGVCoordinate();
                LandmarkInfo UpLand = null;
                LandmarkInfo InflectLand = null;
                LandmarkInfo NextLand = null;
                if (Routes.Count <= 1) { return; }
                else if (Routes.Count >= 3)
                {
                    for (int i = 2; i < Routes.Count; i++)
                    {
                        UpLand = Routes[i - 2];
                        InflectLand = Routes[i - 1];
                        NextLand = Routes[i];
                        //判断退进
                        //    var A = Math.Atan2(Math.Round(NextLand.LandY, 3) - Math.Round(InflectLand.LandY, 3), Math.Round(NextLand.LandX, 3) - Math.Round(InflectLand.LandX, 3));
                        //    var B = Math.Atan2(Math.Round(UpLand.LandY, 3) - Math.Round(InflectLand.LandY, 3), Math.Round(UpLand.LandX, 3) - Math.Round(InflectLand.LandX, 3));
                        //    var C = B - A;
                        //    double angel = (180 * (float)C) / Math.PI;
                        //    if (Math.Abs(angel) < 90)
                        //    { InflectLand.IsBack = true; }


                        double SP = (Math.Round(UpLand.LandX, 3) - Math.Round(NextLand.LandX, 3)) * (Math.Round(InflectLand.LandY, 3) - Math.Round(NextLand.LandY, 3)) - (Math.Round(UpLand.LandY, 3) - Math.Round(NextLand.LandY, 3)) * (Math.Round(InflectLand.LandX, 3) - Math.Round(NextLand.LandX, 3));
                        if (SP > 0.05)
                        {
                            InflectLand.sway = SwayEnum.Left;
                        }
                        else if (SP < -0.05)
                        {
                            InflectLand.sway = SwayEnum.Right;
                        }
                        else
                        { InflectLand.sway = SwayEnum.None; }

                        //判断第一个地标角度
                        if (i == 2)//计算开始行走的角度
                        {
                            if (Math.Round(UpLand.LandX, 1, MidpointRounding.AwayFromZero) == Math.Round(InflectLand.LandX, 1, MidpointRounding.AwayFromZero))
                            {
                                //找对应的行走线段
                                AllSegment Seg = AllSegs.FirstOrDefault(p => p.BeginLandMakCode == UpLand.LandmarkCode && p.EndLandMarkCode == InflectLand.LandmarkCode);
                                if (Seg != null && Seg.ExcuteAngle != -1)
                                { UpLand.Angle = Seg.ExcuteAngle; }
                                else
                                {
                                    if (InflectLand.LandY > UpLand.LandY)//地图上北
                                    {
                                        if (dtCoor.Rows.Count > 0)
                                        {
                                            DataRow dr = dtCoor.Select("Direction=" + 0).FirstOrDefault();
                                            UpLand.Angle = Convert.ToInt16(dr["Angle"]);
                                        }
                                        else
                                        { UpLand.Angle = 90; }
                                    }
                                    else//地图下南
                                    {
                                        if (dtCoor.Rows.Count > 0)
                                        {
                                            DataRow dr = dtCoor.Select("Direction=" + 2).FirstOrDefault();
                                            UpLand.Angle = Convert.ToInt16(dr["Angle"]);
                                        }
                                        else
                                        { UpLand.Angle = 270; }
                                    }
                                }
                            }
                            else if (Math.Round(UpLand.LandY, 1, MidpointRounding.AwayFromZero) == Math.Round(InflectLand.LandY, 1, MidpointRounding.AwayFromZero))
                            {
                                //找对应的行走线段
                                AllSegment Seg = AllSegs.FirstOrDefault(p => p.BeginLandMakCode == UpLand.LandmarkCode && p.EndLandMarkCode == InflectLand.LandmarkCode);
                                if (Seg != null && Seg.ExcuteAngle != -1)
                                { UpLand.Angle = Seg.ExcuteAngle; }
                                else
                                {
                                    if (InflectLand.LandX > UpLand.LandX)//地图右东
                                    {
                                        if (dtCoor.Rows.Count > 0)
                                        {
                                            DataRow dr = dtCoor.Select("Direction=" + 1).FirstOrDefault();
                                            UpLand.Angle = Convert.ToInt16(dr["Angle"]);
                                        }
                                        else
                                        { UpLand.Angle = 0; }
                                    }
                                    else//地图左西
                                    {
                                        if (dtCoor.Rows.Count > 0)
                                        {
                                            DataRow dr = dtCoor.Select("Direction=" + 3).FirstOrDefault();
                                            UpLand.Angle = Convert.ToInt16(dr["Angle"]);
                                        }
                                        else
                                        { UpLand.Angle = 180; }
                                    }
                                }
                            }
                        }

                        //判断最后一个地标的角度
                        if (i == Routes.Count - 1)//计算开始行走的角度
                        {
                            NextLand.Angle = InflectLand.Angle;
                        }


                        if (SP > 0.05 || SP < -0.05)
                        {
                            if (Math.Round(InflectLand.LandX, 1, MidpointRounding.AwayFromZero) == Math.Round(NextLand.LandX, 1, MidpointRounding.AwayFromZero))
                            {
                                //找对应的行走线段
                                AllSegment Seg = AllSegs.FirstOrDefault(p => p.BeginLandMakCode == InflectLand.LandmarkCode && p.EndLandMarkCode == NextLand.LandmarkCode);
                                if (Seg != null && Seg.ExcuteAngle != -1)
                                { InflectLand.Angle = Seg.ExcuteAngle; }
                                else
                                {
                                    if (NextLand.LandY > InflectLand.LandY)//地图上北
                                    {
                                        if (dtCoor.Rows.Count > 0)
                                        {
                                            DataRow dr = dtCoor.Select("Direction=" + 0).FirstOrDefault();
                                            InflectLand.Angle = Convert.ToInt16(dr["Angle"]);
                                        }
                                        else
                                        { InflectLand.Angle = 90; }
                                    }
                                    else//地图下南
                                    {
                                        if (dtCoor.Rows.Count > 0)
                                        {
                                            DataRow dr = dtCoor.Select("Direction=" + 2).FirstOrDefault();
                                            InflectLand.Angle = Convert.ToInt16(dr["Angle"]);
                                        }
                                        else
                                        { InflectLand.Angle = 270; }
                                    }
                                }
                            }
                            else if (Math.Round(InflectLand.LandY, 1, MidpointRounding.AwayFromZero) == Math.Round(NextLand.LandY, 1, MidpointRounding.AwayFromZero))
                            {
                                AllSegment Seg = AllSegs.FirstOrDefault(p => p.BeginLandMakCode == InflectLand.LandmarkCode && p.EndLandMarkCode == NextLand.LandmarkCode);
                                if (Seg != null && Seg.ExcuteAngle != -1)
                                { InflectLand.Angle = Seg.ExcuteAngle; }
                                else
                                {
                                    if (NextLand.LandX > InflectLand.LandX)//地图右东
                                    {
                                        if (dtCoor.Rows.Count > 0)
                                        {
                                            DataRow dr = dtCoor.Select("Direction=" + 1).FirstOrDefault();
                                            InflectLand.Angle = Convert.ToInt16(dr["Angle"]);
                                        }
                                        else
                                        { InflectLand.Angle = 0; }
                                    }
                                    else//地图左西
                                    {
                                        if (dtCoor.Rows.Count > 0)
                                        {
                                            DataRow dr = dtCoor.Select("Direction=" + 3).FirstOrDefault();
                                            InflectLand.Angle = Convert.ToInt16(dr["Angle"]);
                                        }
                                        else
                                        { InflectLand.Angle = 180; }
                                    }
                                }
                            }
                        }
                    }//end foreach
                }
                else
                {
                    InflectLand = Routes[0];
                    NextLand = Routes[1];
                    if (Math.Round(InflectLand.LandX, 1, MidpointRounding.AwayFromZero) == Math.Round(NextLand.LandX, 1, MidpointRounding.AwayFromZero))
                    {
                        //找对应的行走线段
                        AllSegment Seg = AllSegs.FirstOrDefault(p => p.BeginLandMakCode == InflectLand.LandmarkCode && p.EndLandMarkCode == NextLand.LandmarkCode);
                        if (Seg != null && Seg.ExcuteAngle != -1)
                        { InflectLand.Angle = Seg.ExcuteAngle; }
                        else
                        {
                            if (NextLand.LandY > InflectLand.LandY)//地图上北
                            {
                                if (dtCoor.Rows.Count > 0)
                                {
                                    DataRow dr = dtCoor.Select("Direction=" + 0).FirstOrDefault();
                                    InflectLand.Angle = Convert.ToInt16(dr["Angle"]);
                                }
                                else
                                { InflectLand.Angle = 90; }
                            }
                            else//地图下南
                            {
                                if (dtCoor.Rows.Count > 0)
                                {
                                    DataRow dr = dtCoor.Select("Direction=" + 2).FirstOrDefault();
                                    InflectLand.Angle = Convert.ToInt16(dr["Angle"]);
                                }
                                else
                                { InflectLand.Angle = 270; }
                            }
                        }
                    }
                    else if (Math.Round(InflectLand.LandY, 1, MidpointRounding.AwayFromZero) == Math.Round(NextLand.LandY, 1, MidpointRounding.AwayFromZero))
                    {
                        AllSegment Seg = AllSegs.FirstOrDefault(p => p.BeginLandMakCode == InflectLand.LandmarkCode && p.EndLandMarkCode == NextLand.LandmarkCode);
                        if (Seg != null && Seg.ExcuteAngle != -1)
                        { InflectLand.Angle = Seg.ExcuteAngle; }
                        else
                        {
                            if (NextLand.LandX > InflectLand.LandX)//地图右东
                            {
                                if (dtCoor.Rows.Count > 0)
                                {
                                    DataRow dr = dtCoor.Select("Direction=" + 1).FirstOrDefault();
                                    InflectLand.Angle = Convert.ToInt16(dr["Angle"]);
                                }
                                else
                                { InflectLand.Angle = 0; }
                            }
                            else//地图左西
                            {
                                if (dtCoor.Rows.Count > 0)
                                {
                                    DataRow dr = dtCoor.Select("Direction=" + 3).FirstOrDefault();
                                    InflectLand.Angle = Convert.ToInt16(dr["Angle"]);
                                }
                                else
                                { InflectLand.Angle = 180; }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }


        /// <summary>
        /// 计算路线中拐弯数量
        /// </summary>
        private int AcountInflect(List<LandmarkInfo> route)
        {
            try
            {
                int Count = 0;
                LandmarkInfo UpLand = null;
                LandmarkInfo InflectLand = null;
                LandmarkInfo NextLand = null;
                if (route.Count >= 3)
                {
                    for (int i = 2; i < route.Count; i++)
                    {
                        UpLand = route[i - 2];
                        InflectLand = route[i - 1];
                        NextLand = route[i];
                        double SP = (Math.Round(UpLand.LandX, 3) - Math.Round(NextLand.LandX, 3)) * (Math.Round(InflectLand.LandY, 3) - Math.Round(NextLand.LandY, 3)) - (Math.Round(UpLand.LandY, 3) - Math.Round(NextLand.LandY, 3)) * (Math.Round(InflectLand.LandX, 3) - Math.Round(NextLand.LandX, 3));
                        if (SP > 0.05 || SP < -0.05)
                        { Count += 1; }
                    }
                }
                return Count;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 得到路线的地标集合
        /// </summary>
        public List<string> GetRouteStr(List<LandmarkInfo> route)
        {
            try
            {
                List<LandmarkInfo> CopyRoutes = CreateDeepCopy<List<LandmarkInfo>>(route);
                List<LandmarkInfo> ResultRoute = CreateDeepCopy<List<LandmarkInfo>>(route);
                foreach (LandmarkInfo item in CopyRoutes)
                {
                    if (item.sway != SwayEnum.None)
                    {
                        List<LandmarkInfo> CloseLand = (from a in AllLands
                                                        where a.LandmarkCode!= item.LandmarkCode
                                                        orderby getDistant(a.LandX, a.LandY, item.LandX, item.LandY) ascending
                                                        select a).ToList();
                        int IndexItem = ResultRoute.FindIndex(p => p.LandmarkCode == item.LandmarkCode);
                        int Count = 1;
                        int InsetCount = 1;
                        foreach (LandmarkInfo Copy in CloseLand)
                        {
                           
                            if (Count == 5)
                            { break; }
                            if (ResultRoute.Where(p => p.LandmarkCode == Copy.LandmarkCode).Count() <= 0)
                            {
                                LandmarkInfo newLand = CreateDeepCopy<LandmarkInfo>(Copy);
                                ResultRoute.Insert(IndexItem+ InsetCount, newLand);
                                InsetCount++;
                            }
                            Count++;
                        }
                       
                    }
                }
                string RouteStr = string.Join(",", ResultRoute.Distinct().Select(p => p.LandmarkCode));
                List<string> ResultStr = RouteStr.Split(',').ToList<string>();
                //string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                //File.AppendAllText(dir + "//RouteStr.txt", RouteStr);
                return ResultStr;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
