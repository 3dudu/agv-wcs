using Model.MDM;
using Model.MSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace SimulationModel
{
    public class TrafficController
    {
        public static readonly object lockStopObj = new object();
        public static readonly object lockStartObj = new object();

        private List<LockResource> lockResource = new List<LockResource>();
        private IList<CarMonitor> CarList = new List<CarMonitor>();
        IList<AllSegment> AllSeg = new List<AllSegment>();
        IDictionary<string, string> SysParameter = new Dictionary<string, string>();
        IList<LandmarkInfo> AllLands = new List<LandmarkInfo>();

        public TrafficController(IList<CarMonitor> cars, IList<AllSegment> allSeg, IDictionary<string, string> sysParameter, IList<LandmarkInfo> allLands)
        {
            CarList = cars;
            AllSeg = allSeg;
            SysParameter = sysParameter;
            AllLands = allLands;
        }

        private bool CheckIsStop(CarMonitor CurrCar)
        {
            try
            {
                //1 先判断同向，即我当前行走路线资源集中是否有车辆的当前位置
                //2 判断是否路线交叉，即我当前的行走路线资源集有交集的
                //3 判断是否有旋转，即我的行走资源中是否有其他车的旋转资源
                //4 判断管制区域内是否有车

                //判断同向跟车
                CarMonitor HasSamDeriAnotherCar = (from a in CarList
                                                   where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCar.AgvID && CurrCar.RouteLands.Where(q => q == a.CurrSite.ToString()).Count() > 0
                                                   select a).FirstOrDefault();

                //判断是否路线交叉 排除了被我锁住的
                CarMonitor HasIntersectAnotherCar = (from a in CarList
                                                     where /*!a.bIsCommBreak && */a.AgvID != CurrCar.AgvID && CurrCar.RouteLands.Intersect(a.RouteLands).Count() > 0
                                                     orderby CurrCar.RouteLands.Intersect(a.RouteLands).Count() descending
                                                     //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCar.AgvID) <= 0
                                                     select a).FirstOrDefault();

                //判断是否有旋转交集  排除了被我锁住的
                CarMonitor HasTurnAnotherCar = (from a in CarList
                                                where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCar.AgvID && CurrCar.RouteLands.Intersect(a.TurnLands).Count() > 0
                                                //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCar.AgvID) <= 0
                                                select a).FirstOrDefault();

                //判断我的旋转资源中是否有其他车  排除了被我锁住的
                CarMonitor MyTurnHasAnotherCar = (from a in CarList
                                                  where /*!a.bIsCommBreak && */a.AgvID != CurrCar.AgvID && CurrCar.TurnLands.Contains(a.CurrSite.ToString())
                                                  //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCar.AgvID) <= 0
                                                  select a).FirstOrDefault();


                if (HasSamDeriAnotherCar != null)//同向必须停
                {
                    if (lockResource.Where(p => p.LockCarID == HasSamDeriAnotherCar.AgvID && p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                    {
                        LockResource LockBlock = new LockResource();
                        LockBlock.BeLockCarID = CurrCar.AgvID;
                        LockBlock.LockCarID = HasSamDeriAnotherCar.AgvID;
                        lockResource.Add(LockBlock);
                    }
                    //LogHelper.WriteTrafficLog("跟车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,因车" + HasSamDeriAnotherCar.AgvID.ToString() + "在站点" + HasSamDeriAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }

                if (HasTurnAnotherCar != null)
                {
                    ////如果发现我的行走路线中有需要旋转的车辆,并且旋转的车已经在旋转集合中那么不需要判断对方是否暂停,必须自己停
                    if (HasTurnAnotherCar.TurnLands.Contains(HasTurnAnotherCar.CurrSite.ToString()))
                    {
                        if (lockResource.Where(p => p.LockCarID == HasTurnAnotherCar.AgvID && p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                        {
                            LockResource LockBlock = new LockResource();
                            LockBlock.BeLockCarID = CurrCar.AgvID;
                            LockBlock.LockCarID = HasTurnAnotherCar.AgvID;
                            lockResource.Add(LockBlock);
                        }
                        //LogHelper.WriteTrafficLog("当前车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,因为当前车前方有即将旋转车且在他自己的旋转区域内" + HasTurnAnotherCar.AgvID.ToString() + ",目前在站点" + HasTurnAnotherCar.CurrSite.ToString());
                        return true;
                    }

                    //如果即将旋转的车还没在自己旋转区域内，并且当前车已经在即将旋转的车的旋转区域内，那么让即将旋转的车停
                    else if (!HasTurnAnotherCar.TurnLands.Contains(HasTurnAnotherCar.CurrSite.ToString()) /*&& HasTurnAnotherCar.TurnLands.Contains(CurrCar.CurrSite.ToString())*/)
                    {
                        if (lockResource.Where(p => p.LockCarID == CurrCar.AgvID && p.BeLockCarID == HasTurnAnotherCar.AgvID).Count() <= 0)
                        {
                            LockResource LockBlock = new LockResource();
                            LockBlock.BeLockCarID = HasTurnAnotherCar.AgvID;
                            LockBlock.LockCarID = CurrCar.AgvID;
                            lockResource.Add(LockBlock);
                        }
                        //LogHelper.WriteTrafficLog("当前车:" + HasTurnAnotherCar.AgvID.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "停,因为当前车即将旋转车且不在自己的旋转区域内,但有车" + CurrCar.AgvID.ToString() + "站点" + CurrCar.CurrSite.ToString() + "在当前车的旋转区域内");
                        //return true;
                        //Commer.AddControl(HasTurnAnotherCar.AgvID, new CommandToValue(AGVCommandEnum.Stop));
                        HasTurnAnotherCar.Stop();
                        HasTurnAnotherCar.IsLock = true;
                    }
                    else
                    {
                        if (lockResource.Where(p => p.LockCarID == HasTurnAnotherCar.AgvID && p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                        {
                            LockResource LockBlock = new LockResource();
                            LockBlock.BeLockCarID = CurrCar.AgvID;
                            LockBlock.LockCarID = HasTurnAnotherCar.AgvID;
                            lockResource.Add(LockBlock);
                        }
                        //LogHelper.WriteTrafficLog("当前车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,因为当前车前方有即将旋转车" + HasTurnAnotherCar.AgvID.ToString() + ",目前在站点" + HasTurnAnotherCar.CurrSite.ToString());
                        return true;
                    }
                }

                //我的旋转区域内有车，我必须停,不管对方是否不是被我锁住的，也要被对方锁住
                if (MyTurnHasAnotherCar != null)
                {
                    if (lockResource.Where(p => p.LockCarID == MyTurnHasAnotherCar.AgvID && p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                    {
                        LockResource LockBlock = new LockResource();
                        LockBlock.BeLockCarID = CurrCar.AgvID;
                        LockBlock.LockCarID = MyTurnHasAnotherCar.AgvID;
                        lockResource.Add(LockBlock);
                    }
                    //LogHelper.WriteTrafficLog("车:" + CurrCar.AgvID.ToString() + "当前旋转区域内有其他车" + MyTurnHasAnotherCar.AgvID.ToString() + "在站点" + MyTurnHasAnotherCar.CurrSite.ToString() + ",所以当前车在站点" + CurrCar.CurrSite.ToString() + "停");
                    return true;
                }

                if (HasIntersectAnotherCar != null)
                {
                    List<string> CurrInsect = CurrCar.RouteLands.Intersect(HasIntersectAnotherCar.RouteLands).ToList();
                    List<string> AnotherInsect = HasIntersectAnotherCar.RouteLands.Intersect(CurrCar.RouteLands).ToList();
                    if (CurrInsect.Count > 0 && AnotherInsect.Count > 0)
                    {
                        int CurrInsectIndex = CurrCar.RouteLands.FindIndex(q => q == CurrInsect[0]);
                        int AnotherInsectIndex = HasIntersectAnotherCar.RouteLands.FindIndex(q => q == AnotherInsect[0]);
                        if (AnotherInsectIndex >= CurrInsectIndex)//他车停
                        {
                            if (lockResource.Where(p => p.LockCarID == CurrCar.AgvID && p.BeLockCarID == HasIntersectAnotherCar.AgvID).Count() <= 0)
                            {
                                LockResource LockBlock = new LockResource();
                                LockBlock.BeLockCarID = HasIntersectAnotherCar.AgvID;
                                LockBlock.LockCarID = CurrCar.AgvID;
                                lockResource.Add(LockBlock);
                                HasIntersectAnotherCar.Stop();
                                //Commer.AddControl(HasIntersectAnotherCar.AgvID, new CommandToValue(AGVCommandEnum.Stop));
                                HasIntersectAnotherCar.IsLock = true;
                                //LogHelper.WriteTrafficLog("交叉路线汇车,远的车:" + HasIntersectAnotherCar.AgvID.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "停,车" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "走");
                            }
                        }
                        else
                        {
                            if (lockResource.Where(p => p.LockCarID == HasIntersectAnotherCar.AgvID && p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                            {
                                LockResource LockBlock = new LockResource();
                                LockBlock.BeLockCarID = CurrCar.AgvID;
                                LockBlock.LockCarID = HasIntersectAnotherCar.AgvID;
                                lockResource.Add(LockBlock);
                            }

                            //LogHelper.WriteTrafficLog("交叉路线汇车,远的车" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,车" + HasIntersectAnotherCar.AgvID.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "走");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTrafficLog("交通管制异常:" + ex.Message);
                if (lockResource.Where(p => p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                {
                    LockResource LockBlock = new LockResource();
                    LockBlock.BeLockCarID = CurrCar.AgvID;
                    lockResource.Add(LockBlock);
                }
                return true;
            }
        }


        /// <summary>
        /// 管制后只发停止
        /// </summary>
        /// <param name="CurrCar"></param>
        public void HandleTrafficForStop(object obj)
        {
            try
            {
                lock (lockStopObj)
                {
                    if (obj == null) { return; }
                    CarMonitor CurrCar = obj as CarMonitor;
                    if (CurrCar == null) { return; }
                    #region New
                    GetTrafficResour(CurrCar);
                    //LogHelper.WriteTrafficLog("判断交管停止前,车" + CurrCar.AgvID.ToString() + "行走资源集合:" + string.Join(",", CurrCar.RouteLands.Select(p => p)));
                    //LogHelper.WriteTrafficLog("判断交管停止前,车" + CurrCar.AgvID.ToString() + "旋转资源集合:" + string.Join(",", CurrCar.TurnLands.Select(p => p)));
                    if (CurrCar.Sate == 1)
                    {
                        if (CheckIsStop(CurrCar))
                        {
                            CurrCar.Stop();
                            //Commer.AddControl(CurrCar.AgvID, new CommandToValue(AGVCommandEnum.Stop));
                            CurrCar.IsLock = true;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTrafficLog("交通管制停止异常:" + ex.Message);
            }
        }


        /// <summary>
        /// 管制后只发启动
        /// </summary>
        /// <param name="CurrCar"></param>
        public void HandleTrafficForStart()
        {
            try
            {
                lock (lockStartObj)
                {
                    List<LockResource> CurrCarBlock = DataToObject.CreateDeepCopy<List<LockResource>>(lockResource);
                    foreach (LockResource Block in CurrCarBlock)
                    {
                        CarMonitor CurrCar = CarList.FirstOrDefault(p => p.AgvID == Block.BeLockCarID);
                        if (CurrCar == null) { continue; }
                        #region//将停止的车的资源集处理
                        //1 前进资源只添加120长度
                        //2 如果当前停止的点是自己的旋转点那么添加旋转集


                        int CurrStopSiteIndex = CurrCar.CurrRoute.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                        if (CurrStopSiteIndex < 0)
                        {
                            List<string> newRouteLand = DataToObject.CreateDeepCopy<List<string>>(CurrCar.RouteLands);
                            if (CurrCar.RouteLands.Count >= 2)
                            {
                                CurrCar.RouteLands.Clear();
                                CurrCar.RouteLands.Add(newRouteLand[0]);
                                CurrCar.RouteLands.Add(newRouteLand[1]);
                            }
                            continue;
                        }
                        double ResourLenth = 0;
                        double UpResourLenth = 0;
                        double TotalStopLenth = 2;
                        CurrCar.RouteLands.Clear();
                        List<string> TempRouteLands = new List<string>();
                        for (int i = CurrStopSiteIndex + 1; i < CurrCar.CurrRoute.Count; i++)
                        {
                            AllSegment segment = AllSeg.Where(p => p.BeginLandMakCode == (CurrCar.CurrRoute[i - 1]).LandmarkCode && p.EndLandMarkCode == (CurrCar.CurrRoute[i]).LandmarkCode).FirstOrDefault();
                            if (segment == null) { continue; }
                            ResourLenth += segment.Length;
                            if (UpResourLenth <= TotalStopLenth && ResourLenth >= TotalStopLenth)
                            {
                                if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                    CurrCar.RouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                }
                                break;
                            }
                            else
                            {
                                if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i - 1]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i - 1].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.CurrRoute[i - 1].LandmarkCode);
                                    CurrCar.RouteLands.Add(CurrCar.CurrRoute[i - 1].LandmarkCode);
                                }
                                if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                    CurrCar.RouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                }
                            }
                            UpResourLenth += segment.Length;
                        }

                        if (!CurrCar.RouteLands.Contains(CurrCar.CurrSite.ToString()))
                        { CurrCar.RouteLands.Insert(0, CurrCar.CurrSite.ToString()); }

                        //LogHelper.WriteTrafficLog("被交管停止的车" + CurrCar.AgvID.ToString() + "前进资源集合:" + string.Join(",", CurrCar.RouteLands.Select(p => p)));
                        CurrCar.TurnLands.Clear();
                        LandmarkInfo Land = null;
                        if (CurrCar.RouteLands.Count > 0)
                        {

                            Land = (from a in CurrCar.CurrRoute
                                    where a.IsRotateLand && CurrCar.RouteLands.Contains(a.LandmarkCode)
                                    select a).FirstOrDefault();
                        }

                        if (Land != null && Land.IsRotateLand && SysParameter.Keys.Contains("TurnLenth") && SysParameter.Keys.Contains("ScalingRate"))
                        {
                            string TurnLenth = SysParameter["TurnLenth"].ToString();
                            string ScalingRate = SysParameter["ScalingRate"].ToString();
                            if (!string.IsNullOrEmpty(TurnLenth) && !string.IsNullOrEmpty(ScalingRate))
                            {
                                double TurnLen = Convert.ToDouble(TurnLenth);
                                double ScalingRateLen = Convert.ToDouble(ScalingRate);
                                List<LandmarkInfo> CloseLands = (from a in AllLands
                                                                 where Math.Round(getDistant(a.LandX, a.LandY, Land.LandX, Land.LandY) * ScalingRateLen, 3) <= Math.Round(TurnLen, 3)
                                                                 select a).ToList();
                                foreach (LandmarkInfo Copy in CloseLands)
                                {
                                    if (CurrCar.TurnLands.Where(p => p == Copy.LandmarkCode).Count() <= 0)
                                    {
                                        CurrCar.TurnLands.Add(Copy.LandmarkCode);
                                    }
                                }
                            }
                        }
                        #endregion
                        #region New 
                        if (!SameHandleTrafficForStop(CurrCar))
                        {
                            GetTrafficResour(CurrCar);
                            //把启动后的车辆在锁资源中清除
                            List<LockResource> CurrCarLockstemp = lockResource.Where(p => p.BeLockCarID == CurrCar.AgvID).ToList();
                            List<LockResource> CurrCarLocks = DataToObject.CreateDeepCopy<List<LockResource>>(CurrCarLockstemp);
                            foreach (LockResource item in CurrCarLocks)
                            {
                                int index = lockResource.FindIndex(p => p.BeLockCarID == item.BeLockCarID);
                                lockResource.RemoveAt(index);
                            }
                            //Commer.AddControl(CurrCar.AgvID, new CommandToValue(AGVCommandEnum.Start));
                            CurrCar.Start();
                            CurrCar.IsLock = false;
                            //LogHelper.WriteTrafficLog("被管制停的车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "启动");
                        }
                        else
                        {
                            //如果当前车停在旋转点上，那么试图让其走一个，然后有交通管制让其再停下来
                            int CurrSiteIndex = CurrCar.CurrRoute.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                            if (CurrSiteIndex + 1 < CurrCar.CurrRoute.Count)
                            {
                                LandmarkInfo CurrLand = CurrCar.CurrRoute.FirstOrDefault(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                                //LandmarkInfo NextLand = CurrCar.CurrRoute[CurrSiteIndex + 1];
                                //if (CurrLand != null && NextLand != null && CurrLand.IsRotateLand && CoreData.CarList.FirstOrDefault(p => p.AgvID != CurrCar.AgvID && p.CurrSite.ToString() == NextLand.LandmarkCode) == null)
                                if (CurrLand != null && CurrLand.IsRotateLand && CarList.FirstOrDefault(p => p.AgvID != CurrCar.AgvID && CurrCar.RouteLands.Contains(p.CurrSite.ToString())) == null)
                                {
                                    //把启动后的车辆在锁资源中清除
                                    List<LockResource> CurrCarLockstemp = lockResource.Where(p => p.BeLockCarID == CurrCar.AgvID).ToList();
                                    List<LockResource> CurrCarLocks = DataToObject.CreateDeepCopy<List<LockResource>>(CurrCarLockstemp);
                                    foreach (LockResource item in CurrCarLocks)
                                    {
                                        int index = lockResource.FindIndex(p => p.BeLockCarID == item.BeLockCarID);
                                        lockResource.RemoveAt(index);
                                    }
                                    //Commer.AddControl(CurrCar.AgvID, new CommandToValue(AGVCommandEnum.Start));
                                    CurrCar.Start();
                                    CurrCar.IsLock = false;
                                    //LogHelper.WriteTrafficLog("被管制的车停在了自己的旋转点上:" + CurrCar.AgvID.ToString() + ",在地标" + CurrCar.CurrSite.ToString() + "上,往前挪一格");
                                }
                                //else if (CurrLand != null && NextLand != null && CoreData.CarList.FirstOrDefault(q => q.AgvID != CurrCar.AgvID && q.TurnLands.Contains(CurrCar.CurrSite.ToString()) && (CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == q.CurrSite.ToString()).IsRotateLand == false)) != null && CoreData.CarList.FirstOrDefault(p => p.AgvID != CurrCar.AgvID && p.CurrSite.ToString() == NextLand.LandmarkCode) == null)
                                else if (CurrLand != null && CarList.FirstOrDefault(q => q.AgvID != CurrCar.AgvID && q.TurnLands.Contains(CurrCar.CurrSite.ToString()) && (AllLands.FirstOrDefault(p => p.LandmarkCode == q.CurrSite.ToString()).IsRotateLand == false)) != null && CarList.FirstOrDefault(p => p.AgvID != CurrCar.AgvID && CurrCar.RouteLands.Contains(p.CurrSite.ToString())) == null)
                                {
                                    //把启动后的车辆在锁资源中清除
                                    List<LockResource> CurrCarLockstemp = lockResource.Where(p => p.BeLockCarID == CurrCar.AgvID).ToList();
                                    List<LockResource> CurrCarLocks = DataToObject.CreateDeepCopy<List<LockResource>>(CurrCarLockstemp);
                                    foreach (LockResource item in CurrCarLocks)
                                    {
                                        int index = lockResource.FindIndex(p => p.BeLockCarID == item.BeLockCarID);
                                        lockResource.RemoveAt(index);
                                    }
                                    CurrCar.Start();
                                    //Commer.AddControl(CurrCar.AgvID, new CommandToValue(AGVCommandEnum.Start));
                                    CurrCar.IsLock = false;
                                    //LogHelper.WriteTrafficLog("被管制的车停在了别人的旋转集合中:" + CurrCar.CurrSite.ToString() + ",往前挪一格");
                                }
                            }
                        }
                        #endregion
                    }//endFor
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTrafficLog("交通管制启动异常:" + ex.Message);
            }
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
        /// <summary>
        /// 计算资源集
        /// </summary>
        public void GetTrafficResour(CarMonitor CurrCar)
        {
            try
            {
                if (CurrCar == null) { return; }
                //CurrCar.RouteLands.Clear();
                //CurrCar.TurnLands.Clear();
                List<string> TempRouteLands = new List<string>();
                List<string> TempTurnLands = new List<string>();
                if (SysParameter.Keys.Contains("TrafficLenth") && SysParameter.Keys.Contains("StopLenth")
                            && SysParameter.Keys.Contains("TurnLenth") && SysParameter.Keys.Contains("ScalingRate"))
                {
                    string TrafficLenth = SysParameter["TrafficLenth"].ToString();
                    string StopLenth = SysParameter["StopLenth"].ToString();
                    string TurnLenth = SysParameter["TurnLenth"].ToString();
                    string ScalingRate = SysParameter["ScalingRate"].ToString();
                    if (!string.IsNullOrEmpty(TrafficLenth) && !string.IsNullOrEmpty(StopLenth)
                        && !string.IsNullOrEmpty(TurnLenth) && !string.IsNullOrEmpty(ScalingRate))
                    {
                        double StopLen = Convert.ToDouble(StopLenth);
                        double TrafficLen = Convert.ToDouble(TrafficLenth);
                        double TotalStopLenth = StopLen + TrafficLen;//需要发送停车距离
                        double TurnLen = Convert.ToDouble(TurnLenth);
                        double ScalingRateLen = Convert.ToDouble(ScalingRate);

                        //如果当前agv没有路线的话就把当前的站点放入到自己的行走资源集合中
                        if (CurrCar.CurrRoute.Count <= 0)
                        { return; }
                        int CurrSiteIndex = CurrCar.CurrRoute.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                        if (CurrSiteIndex < 0) { return; }

                        List<LandmarkInfo> TrafficLand = new List<LandmarkInfo>();
                        //通过管制距离的系统参数来计算需要锁定的地标集合
                        double ResourLenth = 0;
                        double UpResourLenth = 0;
                        for (int i = CurrSiteIndex + 1; i < CurrCar.CurrRoute.Count; i++)
                        {
                            AllSegment segment = AllSeg.Where(p => p.BeginLandMakCode == (CurrCar.CurrRoute[i - 1]).LandmarkCode && p.EndLandMarkCode == (CurrCar.CurrRoute[i]).LandmarkCode).FirstOrDefault();
                            if (segment == null) { continue; }
                            ResourLenth += segment.Length;
                            if (UpResourLenth <= TotalStopLenth && ResourLenth >= TotalStopLenth)
                            {
                                if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                    TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                                }
                                break;
                            }
                            else
                            {
                                if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i - 1]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i - 1].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.CurrRoute[i - 1].LandmarkCode);
                                    TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i - 1]));
                                }
                                if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                    TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                                }
                            }
                            UpResourLenth += segment.Length;
                        }

                        LandmarkInfo ld = CurrCar.CurrRoute.FirstOrDefault(q => q.LandmarkCode == CurrCar.CurrSite.ToString());
                        if (ld != null && TrafficLand.Where(p => p.LandmarkCode == ld.LandmarkCode).Count() <= 0)
                        { TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(ld)); }

                        //计算路线资源集中的旋转资源集
                        LandmarkInfo Land = (from a in TrafficLand
                                             where a.IsRotateLand
                                             orderby getDistant(a.LandX, a.LandY, ld.LandX, ld.LandY) ascending
                                             select a).FirstOrDefault();
                        if (Land != null && Land.IsRotateLand)
                        {
                            //根据旋转半径找到范围内的地标集
                            List<LandmarkInfo> CloseLands = (from a in AllLands
                                                             where Math.Round(getDistant(a.LandX, a.LandY, Land.LandX, Land.LandY) * ScalingRateLen, 3) <= Math.Round(TurnLen, 3)
                                                             select a).ToList();
                            foreach (LandmarkInfo Copy in CloseLands)
                            {
                                if (TempTurnLands.Where(p => p == Copy.LandmarkCode).Count() <= 0)
                                {
                                    TempTurnLands.Add(Copy.LandmarkCode);
                                }
                            }
                        }
                    }
                }
                CurrCar.RouteLands = TempRouteLands;
                CurrCar.TurnLands = TempTurnLands;
            }
            catch (Exception ex)
            { /*LogHelper.WriteTrafficLog("计算停止资源集异常:" + ex.Message);*/ }
        }


        //单独计算当前车的行走资源集合
        public Hashtable SameGetRouteLandResource(CarMonitor CurrCar)
        {
            try
            {
                if (CurrCar == null)
                { return null; }
                else
                {
                    List<string> TempRouteLands = new List<string>();
                    List<string> TempTurnLands = new List<string>();
                    if (SysParameter.Keys.Contains("TrafficLenth") && SysParameter.Keys.Contains("StopLenth")
                                && SysParameter.Keys.Contains("TurnLenth") && SysParameter.Keys.Contains("ScalingRate"))
                    {
                        string TrafficLenth = SysParameter["TrafficLenth"].ToString();
                        string StopLenth = SysParameter["StopLenth"].ToString();
                        string TurnLenth = SysParameter["TurnLenth"].ToString();
                        string ScalingRate = SysParameter["ScalingRate"].ToString();
                        if (!string.IsNullOrEmpty(TrafficLenth) && !string.IsNullOrEmpty(StopLenth)
                            && !string.IsNullOrEmpty(TurnLenth) && !string.IsNullOrEmpty(ScalingRate))
                        {
                            double StopLen = Convert.ToDouble(StopLenth);
                            double TrafficLen = Convert.ToDouble(TrafficLenth);
                            double TotalStopLenth = StopLen + TrafficLen;//需要发送停车距离
                            double TurnLen = Convert.ToDouble(TurnLenth);
                            double ScalingRateLen = Convert.ToDouble(ScalingRate);

                            //如果当前agv没有路线的话就把当前的站点放入到自己的行走资源集合中
                            if (CurrCar.CurrRoute.Count <= 0)
                            { return null; }
                            int CurrSiteIndex = CurrCar.CurrRoute.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                            if (CurrSiteIndex < 0) { return null; }

                            List<LandmarkInfo> TrafficLand = new List<LandmarkInfo>();
                            //通过管制距离的系统参数来计算需要锁定的地标集合
                            double ResourLenth = 0;
                            double UpResourLenth = 0;
                            for (int i = CurrSiteIndex + 1; i < CurrCar.CurrRoute.Count; i++)
                            {
                                AllSegment segment = AllSeg.Where(p => p.BeginLandMakCode == (CurrCar.CurrRoute[i - 1]).LandmarkCode && p.EndLandMarkCode == (CurrCar.CurrRoute[i]).LandmarkCode).FirstOrDefault();
                                if (segment == null) { continue; }
                                ResourLenth += segment.Length;
                                if (UpResourLenth <= TotalStopLenth && ResourLenth >= TotalStopLenth)
                                {
                                    if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                    {
                                        TempRouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                        TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                                    }
                                    break;
                                }
                                else
                                {
                                    if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i - 1]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i - 1].LandmarkCode != CurrCar.CurrSite.ToString())
                                    {
                                        TempRouteLands.Add(CurrCar.CurrRoute[i - 1].LandmarkCode);
                                        TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i - 1]));
                                    }
                                    if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                    {
                                        TempRouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                        TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                                    }
                                }
                                UpResourLenth += segment.Length;
                            }

                            LandmarkInfo ld = CurrCar.CurrRoute.FirstOrDefault(q => q.LandmarkCode == CurrCar.CurrSite.ToString());
                            if (ld != null && TrafficLand.Where(p => p.LandmarkCode == ld.LandmarkCode).Count() <= 0)
                            { TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(ld)); }

                            LandmarkInfo Land = (from a in TrafficLand
                                                 where a.IsRotateLand
                                                 orderby getDistant(a.LandX, a.LandY, ld.LandX, ld.LandY) ascending
                                                 select a).FirstOrDefault();
                            if (Land != null && Land.IsRotateLand)
                            {
                                //根据旋转半径找到范围内的地标集
                                List<LandmarkInfo> CloseLands = (from a in AllLands
                                                                 where Math.Round(getDistant(a.LandX, a.LandY, Land.LandX, Land.LandY) * ScalingRateLen, 3) <= Math.Round(TurnLen, 3)
                                                                 select a).ToList();
                                foreach (LandmarkInfo Copy in CloseLands)
                                {
                                    if (TempTurnLands.Where(p => p == Copy.LandmarkCode).Count() <= 0)
                                    {
                                        TempTurnLands.Add(Copy.LandmarkCode);
                                    }
                                }
                            }
                        }
                    }
                    Hashtable hs = new Hashtable();
                    hs["RouteLands"] = TempRouteLands;
                    hs["TurnLands"] = TempTurnLands;
                    return hs;
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTrafficLog("启动路径前的资源集计算异常:" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 单独计算当前车是否行走
        /// </summary>
        private bool SameCheckIsStop(int CurrCarID, int CurrSite, Hashtable hs)
        {
            try
            {
                //1 先判断同向，即我当前行走路线资源集中是否有车辆的当前位置
                //2 判断是否路线交叉，即我当前的行走路线资源集有交集的
                //3 判断是否有旋转，即我的行走资源中是否有其他车的旋转资源
                //4 判断管制区域内是否有车
                if (hs == null) { return true; }
                List<string> RouteLand = hs["RouteLands"] as List<string>;
                List<string> TurnLands = hs["TurnLands"] as List<string>;
                //判断同向跟车
                CarMonitor HasSamDeriAnotherCar = (from a in CarList
                                                   where/* !a.bIsCommBreak &&*/ a.AgvID != CurrCarID && RouteLand.Where(q => q == a.CurrSite.ToString()).Count() > 0
                                                   select a).FirstOrDefault();

                //判断是否路线交叉 排除了被我锁住的
                CarMonitor HasIntersectAnotherCar = (from a in CarList
                                                     where /*!a.bIsCommBreak && */a.AgvID != CurrCarID && RouteLand.Intersect(a.RouteLands).Count() > 0
                                                     orderby RouteLand.Intersect(a.RouteLands).Count() descending
                                                     //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                                     select a).FirstOrDefault();

                //判断是否有旋转交集  排除了被我锁住的
                CarMonitor HasTurnAnotherCar = (from a in CarList
                                                where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCarID && RouteLand.Intersect(a.TurnLands).Count() > 0
                                                //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                                select a).FirstOrDefault();

                //判断我的旋转资源中是否有其他车  排除了被我锁住的
                CarMonitor MyTurnHasAnotherCar = (from a in CarList
                                                  where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCarID && TurnLands.Contains(a.CurrSite.ToString())
                                                  //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                                  select a).FirstOrDefault();


                if (HasSamDeriAnotherCar != null)//同向必须停
                {
                    //LogHelper.WriteTrafficLog("启动路径前判断跟车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,因车" + HasSamDeriAnotherCar.AgvID.ToString() + "在站点" + HasSamDeriAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }

                //找我当前车交管地标集中已经扫描到的管制区域
                //需要判断一下当前车目的地是否在交管资源中
                if (HasTurnAnotherCar != null)
                {
                    ////如果发现我的行走路线中有需要旋转的车辆,并且旋转的车已经在旋转集合中那么不需要判断对方是否暂停,必须自己停
                    if (HasTurnAnotherCar.TurnLands.Contains(HasTurnAnotherCar.CurrSite.ToString()))
                    {
                        //LogHelper.WriteTrafficLog("启动前判断有旋转且在自己的旋转区域内,所以当前车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,车" + HasTurnAnotherCar.AgvID.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "阻挡");
                        return true;
                    }

                    //LogHelper.WriteTrafficLog("启动路径前判断前方有其他车即将旋转的区域,所以当前车" + CurrCarID.ToString() + "在站点:" + CurrSite.ToString() + "停,车" + HasTurnAnotherCar.AgvID.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }

                if (HasIntersectAnotherCar != null)
                {
                    //List<string> CurrInsect = RouteLand.Intersect(HasIntersectAnotherCar.RouteLands).ToList();
                    //List<string> AnotherInsect = HasIntersectAnotherCar.RouteLands.Intersect(RouteLand).ToList();
                    //if (CurrInsect.Count > 0 && AnotherInsect.Count > 0)
                    //{
                    //    int CurrInsectIndex = RouteLand.FindIndex(q => q == CurrInsect[0]);
                    //    int AnotherInsectIndex = HasIntersectAnotherCar.RouteLands.FindIndex(q => q == AnotherInsect[0]);
                    //    if (AnotherInsectIndex < CurrInsectIndex)
                    //    {
                    //        LogHelper.WriteTrafficLog("启动前判断交叉汇车,车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,车" + HasIntersectAnotherCar.AgvID.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "阻挡");
                    //        return true;
                    //    }
                    //}
                    //LogHelper.WriteTrafficLog("启动前判断交叉汇车,车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,车" + HasIntersectAnotherCar.AgvID.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTrafficLog("启动路径前判断交通管制异常:" + ex.Message);
                return true;
            }
        }

        /// <summary>
        /// 单独判断交通管制
        /// </summary>
        public bool SameHandleTrafficForStop(object obj)
        {
            try
            {
                if (obj == null) { return true; }
                CarMonitor CurrCar = obj as CarMonitor;
                if (CurrCar == null) { return true; }
                #region New
                Hashtable hs = SameGetRouteLandResource(CurrCar);
                //LogHelper.WriteTrafficLog("启动停止的车前判断" + CurrCar.AgvID.ToString() + "行走资源集合:" + string.Join(",", (hs["RouteLands"] as List<string>).Select(p => p)));
                //LogHelper.WriteTrafficLog("启动停止的车前判断" + CurrCar.AgvID.ToString() + "旋转资源集合:" + string.Join(",", (hs["TurnLands"] as List<string>).Select(p => p)));
                return SameCheckIsStop(CurrCar.AgvID, CurrCar.CurrSite, hs);
                #endregion
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTrafficLog("启动路径前判断交通管制异常:" + ex.Message);
                return true;
            }
        }


        bool IsHandleBeforStart = false;
        /// <summary>
        /// 启动前判断交通管制
        /// </summary>
        public bool BeforStartTrafficForStop(object obj)
        {
            try
            {
                if (!IsHandleBeforStart)
                {
                    IsHandleBeforStart = true;
                    if (obj == null) { return true; }
                    CarMonitor CurrCar = obj as CarMonitor;
                    if (CurrCar == null) { return true; }
                    #region New
                    Hashtable hs = SameGetRouteLandResource(CurrCar);
                    //LogHelper.WriteTrafficLog("启动前判断车" + CurrCar.AgvID.ToString() + "行走资源集合:" + string.Join(",", (hs["RouteLands"] as List<string>).Select(p => p)));
                    //LogHelper.WriteTrafficLog("启动前判断车" + CurrCar.AgvID.ToString() + "旋转资源集合:" + string.Join(",", (hs["TurnLands"] as List<string>).Select(p => p)));
                    return BeforStartCheckIsStop(CurrCar.AgvID, CurrCar.CurrSite, hs);
                    #endregion
                }
                else
                { return true; }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTrafficLog("启动前判断交通管制异常:" + ex.Message);
                return true;
            }
            finally
            { IsHandleBeforStart = false; }
        }


        /// <summary>
        /// 启动前判断是否可启动
        /// </summary>
        /// <param name="CurrCarID"></param>
        /// <param name="CurrSite"></param>
        /// <param name="hs"></param>
        /// <returns></returns>
        public bool BeforStartCheckIsStop(int CurrCarID, int CurrSite, Hashtable hs)
        {
            try
            {
                //1 先判断同向，即我当前行走路线资源集中是否有车辆的当前位置
                //2 判断是否路线交叉，即我当前的行走路线资源集有交集的
                //3 判断是否有旋转，即我的行走资源中是否有其他车的旋转资源
                //4 判断管制区域内是否有车
                if (hs == null) { return true; }
                List<string> RouteLand = hs["RouteLands"] as List<string>;
                List<string> TurnLands = hs["TurnLands"] as List<string>;
                //判断同向跟车
                CarMonitor HasSamDeriAnotherCar = (from a in CarList
                                                where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCarID && RouteLand.Where(q => q == a.CurrSite.ToString()).Count() > 0
                                                select a).FirstOrDefault();

                //判断是否路线交叉 排除了被我锁住的
                CarMonitor HasIntersectAnotherCar = (from a in CarList
                                                  where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCarID && RouteLand.Intersect(a.RouteLands).Count() > 0
                                                  orderby RouteLand.Intersect(a.RouteLands).Count() descending
                                                  //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                                  select a).FirstOrDefault();

                //判断是否有旋转交集  排除了被我锁住的
                CarMonitor HasTurnAnotherCar = (from a in CarList
                                             where/* !a.bIsCommBreak && */a.AgvID != CurrCarID && RouteLand.Intersect(a.TurnLands).Count() > 0
                                             //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                             select a).FirstOrDefault();

                //判断我的旋转资源中是否有其他车  排除了被我锁住的
                CarMonitor MyTurnHasAnotherCar = (from a in CarList
                                               where/* !a.bIsCommBreak &&*/ a.AgvID != CurrCarID && TurnLands.Contains(a.CurrSite.ToString())
                                               //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                               select a).FirstOrDefault();


                if (HasSamDeriAnotherCar != null)//同向必须停
                {
                    //LogHelper.WriteTrafficLog("启动前判断跟车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,因车" + HasSamDeriAnotherCar.AgvID.ToString() + "在站点" + HasSamDeriAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }

                //我的旋转区域内有车，我必须停,不管对方是否不是被我锁住的，也要被对方锁住
                if (MyTurnHasAnotherCar != null)
                {
                    //LogHelper.WriteTrafficLog("启动前判断我的旋转资源中有车,所以当前车" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,车" + MyTurnHasAnotherCar.AgvID.ToString() + "在站点" + MyTurnHasAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }


                if (HasTurnAnotherCar != null)
                {
                    ////如果发现我的行走路线中有需要旋转的车辆,并且旋转的车已经在旋转集合中那么不需要判断对方是否暂停,必须自己停
                    //if (HasTurnAnotherCar.TurnLands.Contains(HasTurnAnotherCar.CurrSite.ToString()))
                    //{
                    //    LogHelper.WriteLog("启动前判断有旋转且在旋转半径内车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,车" + HasTurnAnotherCar.AgvID.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "阻挡");
                    //    return true;
                    //}

                    //LogHelper.WriteTrafficLog("启动路径前判断前方有其他车即将旋转区域,所以当前车" + CurrCarID.ToString() + "在站点:" + CurrSite.ToString() + "停,即将旋转的车" + HasTurnAnotherCar.AgvID.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "继续走");
                    return true;
                }

                if (HasIntersectAnotherCar != null)
                {
                    //List<string> CurrInsect = RouteLand.Intersect(HasIntersectAnotherCar.RouteLands).ToList();
                    //List<string> AnotherInsect = HasIntersectAnotherCar.RouteLands.Intersect(RouteLand).ToList();
                    //if (CurrInsect.Count > 0 && AnotherInsect.Count > 0)
                    //{
                    //    int CurrInsectIndex = RouteLand.FindIndex(q => q == CurrInsect[0]);
                    //    int AnotherInsectIndex = HasIntersectAnotherCar.RouteLands.FindIndex(q => q == AnotherInsect[0]);
                    //    if (AnotherInsectIndex < CurrInsectIndex)
                    //    {
                    //        LogHelper.WriteLog("启动前判断交叉车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,车" + HasIntersectAnotherCar.AgvID.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "阻挡");
                    //        return true;
                    //    }
                    //}
                    //LogHelper.WriteTrafficLog("启动路径前判断交叉汇车,当前车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,车" + HasIntersectAnotherCar.AgvID.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTrafficLog("启动路径前判断交通管制异常:" + ex.Message);
                return true;
            }
        }
    }//end
}
