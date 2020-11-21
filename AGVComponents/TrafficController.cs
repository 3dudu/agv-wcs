using AGVCommunication;
using AGVCore;
using DipatchModel;
using Model.CarInfoExtend;
using Model.MDM;
using Model.MSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
namespace AGVComponents
{
    public class TrafficController
    {
        public static readonly object lockStopObj = new object();
        public static readonly object lockStartObj = new object();

        public List<LockResource> lockResource = new List<LockResource>();

        public CommunicationBase Commer { get; set; }

        /// <summary>
        /// 判断当前车是否在锁定资源集或锁定其他车
        /// </summary>
        /// <param name="CurrCar"></param>
        /// <returns></returns>
        public bool CheckCarIsInLockSource(CarInfo CurrCar)
        {
            try
            {
                List<LockResource> CurrCarLockstemp = lockResource.Where(p => p.BeLockCarID == CurrCar.AgvID).ToList();
                List<LockResource> OtherCarLockstemp = lockResource.Where(p => p.LockCarID == CurrCar.AgvID).ToList();
                if (CurrCarLockstemp.Count > 0)
                { return true; }
                else
                { return false; }

                //List<LockResource> CurrCarLocks = DataToObject.CreateDeepCopy<List<LockResource>>(CurrCarLockstemp);
                //foreach (LockResource item in CurrCarLocks)
                //{
                //    int index = lockResource.FindIndex(p => p.BeLockCarID == item.BeLockCarID);
                //    lockResource.RemoveAt(index);
                //}
                //LogHelper.WriteTrafficLog("发送路线前清除车辆交管所资源成功!");
            }
            catch (Exception ex)
            { LogHelper.WriteTrafficLog("发送路线前判断当前车是否在锁定资源集或锁定其他车异常:" + ex.Message); return false; }
        }

        private bool CheckIsStop(CarInfo CurrCar)
        {
            try
            {
                //1 先判断同向，即我当前行走路线资源集中是否有车辆的当前位置
                //2 判断是否路线交叉，即我当前的行走路线资源集有交集的
                //3 判断是否有旋转，即我的行走资源中是否有其他车的旋转资源
                //4 判断管制区域内是否有车

                //判断同向跟车
                CarInfo HasSamDeriAnotherCar = (from a in CoreData.CarList
                                                where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCar.AgvID && CurrCar.RouteLands.Where(q => q == a.CurrSite.ToString()).Count() > 0
                                                select a).FirstOrDefault();

                //判断是否路线交叉 排除了被我锁住的
                CarInfo HasIntersectAnotherCar = (from a in CoreData.CarList
                                                  where /*!a.bIsCommBreak && */a.AgvID != CurrCar.AgvID && CurrCar.RouteLands.Intersect(a.RouteLands).Count() > 0
                                                    && !CurrCar.RouteLands.Contains(a.CurrSite.ToString())
                                                  //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCar.AgvID) <= 0
                                                  orderby CurrCar.RouteLands.Intersect(a.RouteLands).Count() descending
                                                  select a).FirstOrDefault();

                //判断是否有旋转交集  排除了被我锁住的
                CarInfo HasTurnAnotherCar = (from a in CoreData.CarList
                                             where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCar.AgvID && CurrCar.RouteLands.Intersect(a.TurnLands).Count() > 0
                                             //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCar.AgvID) <= 0
                                             select a).FirstOrDefault();

                //判断我的旋转资源中是否有其他车  排除了被我锁住的
                CarInfo MyTurnHasAnotherCar = (from a in CoreData.CarList
                                               where /*!a.bIsCommBreak && */a.AgvID != CurrCar.AgvID && CurrCar.TurnLands.Contains(a.CurrSite.ToString())
                                               //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCar.AgvID) <= 0
                                               select a).FirstOrDefault();

                //判断我的旋转资源集和其他车的旋转资源集是否有冲突
                CarInfo SameTurnAnotherCar = (from a in CoreData.CarList
                                              where /*!a.bIsCommBreak && */a.AgvID != CurrCar.AgvID && CurrCar.TurnLands.Intersect(a.TurnLands).Count() > 0
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
                    LogHelper.WriteTrafficLog("跟车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,因车" + HasSamDeriAnotherCar.AgvID.ToString() + "在站点" + HasSamDeriAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }

                //找我当前车交管地标集中已经扫描到的管制区域
                //需要判断一下当前车目的地是否在交管资源中
                try
                {
                    IList<TraJunction> Juncs = CoreData.JunctionList.Where(p => CurrCar.RouteLands.Intersect(p.JuncLandCodes).ToList().Count > 0
                   && ((p.Segments.Count > 0 && p.Segments.Count(q => CurrCar.RealyAllRouteLandCode.IndexOf(q.LandCodes) >= 0) > 0) || p.Segments.Count == 0)).ToList();
                    bool IsStop = false;
                    foreach (TraJunction Junc in Juncs)
                    {
                        //判断扫描到的管制区域内是否有其他的车
                        if (Junc.LockCars.Count(p => p.AgvID != CurrCar.AgvID) >= Junc.Carnumber)
                        {

                            CarInfo HasAnotherCar = Junc.LockCars.FirstOrDefault(p => p.AgvID != CurrCar.AgvID);
                            if (HasAnotherCar != null)
                            {
                                if (lockResource.Where(p => p.LockCarID == HasAnotherCar.AgvID && p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                                {
                                    LockResource LockBlock = new LockResource();
                                    LockBlock.BeLockCarID = CurrCar.AgvID;
                                    LockBlock.LockCarID = HasAnotherCar.AgvID;
                                    lockResource.Add(LockBlock);
                                }
                                LogHelper.WriteTrafficLog("管制区域" + Junc.TraJunctionID.ToString() + "有车:" + HasAnotherCar.AgvID.ToString() + "在站点" + HasAnotherCar.CurrSite.ToString() + "锁定,当前车" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停");
                            }
                            IsStop = true;
                        }
                        else
                        {
                            if (Junc.LockCars.Count(p => p.AgvID == CurrCar.AgvID) <= 0)
                            {
                                Junc.LockCars.Add(CurrCar);
                                LogHelper.WriteTrafficLog("管制区域" + Junc.TraJunctionID.ToString() + "被车:" + CurrCar.AgvID.ToString() + "占用");
                            }
                        }
                    }
                    if (IsStop)
                    { return true; }
                }
                catch (Exception ex)
                { LogHelper.WriteTrafficLog("自定义管制处理逻辑异常：" + ex.Message + ex.StackTrace); }
                //JunctionInfo Junc = (from a in CoreData.JunctionList
                //                     where CurrCar.RouteLands.Where(p => p == a.EnterLandCode).Count() > 0
                //                     && a.OwerAGVID != -1 && a.OwerAGVID != CurrCar.AgvID
                //                     select a).FirstOrDefault();

                //if (Junc != null)
                //{
                //    if (lockResource.Where(p => p.LockCarID == Junc.OwerAGVID && p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                //    {
                //        LockResource LockBlock = new LockResource();
                //        LockBlock.BeLockCarID = CurrCar.AgvID;
                //        LockBlock.LockCarID = Junc.OwerAGVID;
                //        lockResource.Add(LockBlock);
                //    }
                //    CarInfo SourceCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == Junc.OwerAGVID);
                //    if (SourceCar != null)
                //    {
                //        LogHelper.WriteLog("管制区域有车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停" + SourceCar.AgvID.ToString() + "在站点" + SourceCar.CurrSite.ToString() + "继续走");
                //    }
                //    return true;
                //}
                //JunctionInfo MyJunc = (from a in CoreData.JunctionList
                //                       where a.OwerAGVID == CurrCar.AgvID
                //                       && !a.JunctionLandMarkCodes.Contains(CurrCar.CurrSite.ToString())
                //                       select a).FirstOrDefault();
                //if (MyJunc != null)
                //{ MyJunc.OwerAGVID = -1; }

                /////释放独锁资源集
                //JunctionInfo ReleasJunc = (from a in CoreData.JunctionList
                //                           where a.OwerAGVID == CurrCar.AgvID
                //                           select a).FirstOrDefault();
                //if (ReleasJunc != null)
                //{
                //    if (!ReleasJunc.JunctionLandMarkCodes.Contains(CurrCar.CurrSite.ToString()))
                //    { ReleasJunc.OwerAGVID = -1; }
                //}

                if (HasIntersectAnotherCar != null)
                {
                    List<string> CurrInsect = CurrCar.RouteLands.Intersect(HasIntersectAnotherCar.RouteLands).ToList();
                    List<string> AnotherInsect = HasIntersectAnotherCar.RouteLands.Intersect(CurrCar.RouteLands).ToList();
                    if (CurrInsect.Count > 0 && AnotherInsect.Count > 0)
                    {
                        int CurrInsectIndex = CurrCar.RouteLands.FindIndex(q => q == CurrInsect[0]);
                        int AnotherInsectIndex = HasIntersectAnotherCar.RouteLands.FindIndex(q => q == AnotherInsect[0]);

                        double CurrInsectDistance = 0;
                        double AnotherInsectDistance = 0;
                        //计算交叉车各自离交点的距离
                        LandmarkInfo AnotherCarCurrLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == HasIntersectAnotherCar.CurrSite.ToString());
                        LandmarkInfo CurrCarCurrLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                        LandmarkInfo AnotherInsectLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == AnotherInsect[0]);
                        LandmarkInfo CurrInsectLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == CurrInsect[0]);
                        if (AnotherCarCurrLand != null && CurrCarCurrLand != null && AnotherInsectLand != null && CurrInsectLand != null)
                        {
                            CurrInsectDistance = this.getDistant(CurrCarCurrLand.LandX, CurrCarCurrLand.LandY, CurrInsectLand.LandX, CurrInsectLand.LandY);
                            AnotherInsectDistance = this.getDistant(AnotherCarCurrLand.LandX, AnotherCarCurrLand.LandY, AnotherInsectLand.LandX, AnotherInsectLand.LandY);
                        }
                        //if (AnotherInsectIndex >= CurrInsectIndex)//他车停
                        if (AnotherInsectDistance >= CurrInsectDistance)//他车停
                        {
                            if (lockResource.Where(p => p.LockCarID == CurrCar.AgvID && p.BeLockCarID == HasIntersectAnotherCar.AgvID).Count() <= 0)
                            {
                                LockResource LockBlock = new LockResource();
                                LockBlock.BeLockCarID = HasIntersectAnotherCar.AgvID;
                                LockBlock.LockCarID = CurrCar.AgvID;
                                lockResource.Add(LockBlock);
                                Commer.AGV_AddControl(HasIntersectAnotherCar.AgvID, new CommandToValue(AGVCommandEnum.Stop));
                                HasIntersectAnotherCar.IsLock = true;
                                LogHelper.WriteTrafficLog("交叉路线汇车,远的车:" + HasIntersectAnotherCar.AgvID.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "停,车" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "走");
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

                            LogHelper.WriteTrafficLog("交叉路线汇车,远的车" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,车" + HasIntersectAnotherCar.AgvID.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "走");
                            return true;
                        }
                    }
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
                        LogHelper.WriteTrafficLog("当前车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,因为当前车前方有即将旋转车且在他自己的旋转区域内" + HasTurnAnotherCar.AgvID.ToString() + ",目前在站点" + HasTurnAnotherCar.CurrSite.ToString());
                        return true;
                    }

                    //如果即将旋转的车还没在自己旋转区域内，并且当前车已经在即将旋转的车的旋转区域内，那么让即将旋转的车停
                    else if (!HasTurnAnotherCar.TurnLands.Contains(HasTurnAnotherCar.CurrSite.ToString()) && HasTurnAnotherCar.TurnLands.Contains(CurrCar.CurrSite.ToString()))
                    {
                        if (lockResource.Where(p => p.LockCarID == CurrCar.AgvID && p.BeLockCarID == HasTurnAnotherCar.AgvID).Count() <= 0)
                        {
                            LockResource LockBlock = new LockResource();
                            LockBlock.BeLockCarID = HasTurnAnotherCar.AgvID;
                            LockBlock.LockCarID = CurrCar.AgvID;
                            lockResource.Add(LockBlock);
                        }
                        LogHelper.WriteTrafficLog("当前车:" + HasTurnAnotherCar.AgvID.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "停,因为当前车即将旋转车且不在自己的旋转区域内,但有车" + CurrCar.AgvID.ToString() + "站点" + CurrCar.CurrSite.ToString() + "在当前车的旋转区域内");
                        //return true;
                        Commer.AGV_AddControl(HasTurnAnotherCar.AgvID, new CommandToValue(AGVCommandEnum.Stop));
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
                        LogHelper.WriteTrafficLog("当前车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,因为当前车前方有即将旋转车" + HasTurnAnotherCar.AgvID.ToString() + ",目前在站点" + HasTurnAnotherCar.CurrSite.ToString());
                        return true;
                    }
                }

                //判断我的旋转资源集和别车旋转资源集是否冲突  zlc
                if (SameTurnAnotherCar != null)
                {
                    //旋转的车已经在旋转集合中那么不需要判断对方是否暂停,必须自己停
                    if (SameTurnAnotherCar.TurnLands.Contains(SameTurnAnotherCar.CurrSite.ToString()) && !CurrCar.TurnLands.Contains(CurrCar.CurrSite.ToString()))
                    {
                        if (lockResource.Where(p => p.LockCarID == SameTurnAnotherCar.AgvID && p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                        {
                            LockResource LockBlock = new LockResource();
                            LockBlock.BeLockCarID = CurrCar.AgvID;
                            LockBlock.LockCarID = SameTurnAnotherCar.AgvID;
                            lockResource.Add(LockBlock);
                        }
                        LogHelper.WriteTrafficLog("当前车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,因为当前车前方有即将旋转车且在他自己的旋转区域内" + SameTurnAnotherCar.AgvID.ToString() + ",目前在站点" + SameTurnAnotherCar.CurrSite.ToString());
                        return true;
                    }
                    //如果即将旋转的车还没在自己旋转区域内，并且当前车已经在即将旋转的车的旋转区域内，那么让即将旋转的车停
                    else if (!SameTurnAnotherCar.TurnLands.Contains(SameTurnAnotherCar.CurrSite.ToString()) && CurrCar.TurnLands.Contains(CurrCar.CurrSite.ToString()))
                    {
                        if (lockResource.Where(p => p.LockCarID == CurrCar.AgvID && p.BeLockCarID == SameTurnAnotherCar.AgvID).Count() <= 0)
                        {
                            LockResource LockBlock = new LockResource();
                            LockBlock.BeLockCarID = SameTurnAnotherCar.AgvID;
                            LockBlock.LockCarID = CurrCar.AgvID;
                            lockResource.Add(LockBlock);
                        }
                        LogHelper.WriteTrafficLog("当前车:" + SameTurnAnotherCar.AgvID.ToString() + "在站点" + SameTurnAnotherCar.CurrSite.ToString() + "停,因为有车在自己的旋转区域内,车" + CurrCar.AgvID.ToString() + "站点" + CurrCar.CurrSite.ToString() + "在当前车的旋转区域内");
                        //return true;
                        Commer.AGV_AddControl(SameTurnAnotherCar.AgvID, new CommandToValue(AGVCommandEnum.Stop));
                        SameTurnAnotherCar.IsLock = true;
                    }
                    else
                    {
                        if (lockResource.Where(p => p.LockCarID == SameTurnAnotherCar.AgvID && p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                        {
                            LockResource LockBlock = new LockResource();
                            LockBlock.BeLockCarID = CurrCar.AgvID;
                            LockBlock.LockCarID = SameTurnAnotherCar.AgvID;
                            lockResource.Add(LockBlock);
                        }
                        LogHelper.WriteTrafficLog("当前车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,因为当前车前方有即将旋转车" + SameTurnAnotherCar.AgvID.ToString() + ",目前在站点" + SameTurnAnotherCar.CurrSite.ToString());
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
                    LogHelper.WriteTrafficLog("车:" + CurrCar.AgvID.ToString() + "当前旋转区域内有其他车" + MyTurnHasAnotherCar.AgvID.ToString() + "在站点" + MyTurnHasAnotherCar.CurrSite.ToString() + ",所以当前车在站点" + CurrCar.CurrSite.ToString() + "停");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteTrafficLog("交通管制异常:" + ex.Message);
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
                    CarInfo CurrCar = obj as CarInfo;
                    if (CurrCar == null) { return; }
                    #region New
                    if (lockResource.Count(p => p.BeLockCarID == CurrCar.AgvID) > 0)
                    {
                        LogHelper.WriteTrafficLog("停止的车:" + CurrCar.AgvID.ToString() + "还在停车资源里,所有不能计算资源集");
                        return;
                    }
                    GetTrafficResour(CurrCar);
                    LogHelper.WriteTrafficLog("判断交管停止前,车" + CurrCar.AgvID.ToString() + "行走资源集合:" + string.Join(",", CurrCar.RouteLands.Select(p => p)));
                    LogHelper.WriteTrafficLog("判断交管停止前,车" + CurrCar.AgvID.ToString() + "旋转资源集合:" + string.Join(",", CurrCar.TurnLands.Select(p => p)));
                    if (CurrCar.CarState == 1)
                    {
                        if (CheckIsStop(CurrCar))
                        {
                            Commer.AGV_AddControl(CurrCar.AgvID, new CommandToValue(AGVCommandEnum.Stop));
                            CurrCar.IsLock = true;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteTrafficLog("交通管制停止异常:" + ex.Message);
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
                        CarInfo CurrCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == Block.BeLockCarID);
                        if (CurrCar == null) { continue; }
                        #region//将停止的车的资源集处理
                        //1 停止的车子前进资源集为停车距离
                        //2 如果当前停止的点是自己的旋转点那么添加旋转集

                        //得到车当前站点在路线中的地标位置
                        int CurrStopSiteIndex = CurrCar.RealyRoute.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                        //异常判断一下，正常不会进入该if判断，如果异常则固定保护路线开始的两个地标距离
                        if (CurrStopSiteIndex < 0)
                        {
                            List<string> newRouteLand = DataToObject.CreateDeepCopy<List<string>>(CurrCar.RouteLands);
                            if (CurrCar.RouteLands.Count >= 2)
                            {
                                CurrCar.RouteLands.Clear();
                                CurrCar.TurnLands.Clear();
                                CurrCar.RouteLands.Add(newRouteLand[0]);
                                CurrCar.RouteLands.Add(newRouteLand[1]);
                                if (CurrCar.RouteLands.Count(p => p == CurrCar.CurrSite.ToString()) <= 0)
                                { CurrCar.RouteLands.Insert(0, CurrCar.CurrSite.ToString()); }
                            }
                            continue;
                        }

                        //正常则通过一下系统参数的停车距离来计算交管资源集，那么系统参数中的
                        //停车距离应该配置的比交管距离要短。
                        double ResourLenth = 0;
                        double UpResourLenth = 0;
                        double TotalStopLenth = 3;
                        //List<string> CountTurnLands = new List<string>();
                        if (CoreData.SysParameter.Keys.Contains("StopLenth"))
                        {
                            string StopLenth = CoreData.SysParameter["StopLenth"].ToString();
                            double StopLen = 0;
                            try
                            {
                                StopLen = Convert.ToDouble(StopLenth);
                            }
                            catch
                            { }
                            if (StopLen > 0)
                            { TotalStopLenth = StopLen; }
                        }


                        CurrCar.RouteLands.Clear();
                        CurrCar.TurnLands.Clear();
                        List<string> TempRouteLands = new List<string>();
                        for (int i = CurrStopSiteIndex + 1; i < CurrCar.RealyRoute.Count; i++)
                        {
                            AllSegment segment = CoreData.AllSeg.Where(p => p.BeginLandMakCode == (CurrCar.RealyRoute[i - 1]).LandmarkCode && p.EndLandMarkCode == (CurrCar.RealyRoute[i]).LandmarkCode).FirstOrDefault();
                            if (segment == null) { continue; }
                            ResourLenth += segment.Length;
                            if (UpResourLenth <= TotalStopLenth && ResourLenth >= TotalStopLenth)
                            {
                                if (TempRouteLands.Where(p => p == (CurrCar.RealyRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.RealyRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.RealyRoute[i].LandmarkCode);
                                    CurrCar.RouteLands.Add(CurrCar.RealyRoute[i].LandmarkCode);
                                }
                                LogHelper.WriteTrafficLog("补丁日期:2020-3-26");
                                break;
                            }
                            else
                            {
                                if (TempRouteLands.Where(p => p == (CurrCar.RealyRoute[i - 1]).LandmarkCode).Count() <= 0 && CurrCar.RealyRoute[i - 1].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.RealyRoute[i - 1].LandmarkCode);
                                    CurrCar.RouteLands.Add(CurrCar.RealyRoute[i - 1].LandmarkCode);
                                    //if (UpResourLenth < TotalStopLenth / 2)
                                    //{
                                    //    CountTurnLands.Add(CurrCar.Route[i - 1].LandmarkCode);
                                    //}

                                }
                                if (TempRouteLands.Where(p => p == (CurrCar.RealyRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.RealyRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.RealyRoute[i].LandmarkCode);
                                    CurrCar.RouteLands.Add(CurrCar.RealyRoute[i].LandmarkCode);
                                    //if (UpResourLenth < TotalStopLenth / 2)
                                    //{
                                    //    CountTurnLands.Add(CurrCar.Route[i].LandmarkCode);
                                    //}
                                }
                            }
                            UpResourLenth += segment.Length;
                        }

                        if (!CurrCar.RouteLands.Contains(CurrCar.CurrSite.ToString()))
                        { CurrCar.RouteLands.Insert(0, CurrCar.CurrSite.ToString()); }

                        //if (!CountTurnLands.Contains(CurrCar.CurrSite.ToString()))
                        //{ CountTurnLands.Insert(0, CurrCar.CurrSite.ToString()); }

                        LogHelper.WriteTrafficLog("被交管停止的车" + CurrCar.AgvID.ToString() + "前进资源集合:" + string.Join(",", CurrCar.RouteLands.Select(p => p)));


                        //List<string> newRouteLand = DataToObject.CreateDeepCopy<List<string>>(CurrCar.RouteLands);
                        //if (CurrCar.RouteLands.Count >= 2)
                        //{
                        //    CurrCar.RouteLands.Clear();
                        //    CurrCar.RouteLands.Add(newRouteLand[0]);
                        //    CurrCar.RouteLands.Add(newRouteLand[1]);
                        //}





                        CurrCar.TurnLands.Clear();
                        LandmarkInfo Land = null;
                        if (CurrCar.RouteLands.Count > 0)
                        {

                            Land = (from a in CurrCar.RealyRoute
                                    where a.IsRotateLand && CurrCar.RouteLands.Contains(a.LandmarkCode)
                                    select a).FirstOrDefault();
                        }

                        if (Land != null && Land.IsRotateLand && CoreData.SysParameter.Keys.Contains("TurnLenth") && CoreData.SysParameter.Keys.Contains("ScalingRate"))
                        {
                            string TurnLenth = CoreData.SysParameter["TurnLenth"].ToString();
                            string ScalingRate = CoreData.SysParameter["ScalingRate"].ToString();
                            if (!string.IsNullOrEmpty(TurnLenth) && !string.IsNullOrEmpty(ScalingRate))
                            {
                                double TurnLen = Convert.ToDouble(TurnLenth);
                                double ScalingRateLen = Convert.ToDouble(ScalingRate);
                                List<LandmarkInfo> CloseLands = (from a in CoreData.AllLands
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
                        if (CurrCar.TurnLands.Count(p => p == CurrCar.CurrSite.ToString()) <= 0)
                        { CurrCar.TurnLands.Add(CurrCar.CurrSite.ToString()); }
                        LogHelper.WriteTrafficLog("被交管停止的车" + CurrCar.AgvID.ToString() + "旋转资源集合:" + string.Join(",", CurrCar.TurnLands.Select(p => p)));
                        #endregion
                        #region New 
                        if (!SameHandleTrafficForStop(CurrCar))
                        {
                            CurrCar.IsLock = false;
                            GetTrafficResour(CurrCar);
                            Commer.AGV_AddControl(CurrCar.AgvID, new CommandToValue(AGVCommandEnum.Start));
                            LogHelper.WriteTrafficLog("被管制停的车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "启动");
                          
                            //把启动后的车辆在锁资源中清除
                            List<LockResource> CurrCarLockstemp = lockResource.Where(p => p.BeLockCarID == CurrCar.AgvID).ToList();
                            List<LockResource> CurrCarLocks = DataToObject.CreateDeepCopy<List<LockResource>>(CurrCarLockstemp);
                            foreach (LockResource item in CurrCarLocks)
                            {
                                int index = lockResource.FindIndex(p => p.BeLockCarID == item.BeLockCarID);
                                lockResource.RemoveAt(index);
                            }
                        }
                        else
                        {
                            LogHelper.WriteTrafficLog("被管制停的车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "继续等待启动");
                            #region 解锁逻辑先不要
                            ////如果当前车停在旋转点上，那么试图让其走一个，然后有交通管制让其再停下来
                            //int CurrSiteIndex = CurrCar.Route.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                            //if (CurrSiteIndex + 1 < CurrCar.Route.Count)
                            //{
                            //    LandmarkInfo CurrLand = CurrCar.Route.FirstOrDefault(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                            //    //LandmarkInfo NextLand = CurrCar.Route[CurrSiteIndex + 1];
                            //    //if (CurrLand != null && NextLand != null && CurrLand.IsRotateLand && CoreData.CarList.FirstOrDefault(p => p.AgvID != CurrCar.AgvID && p.CurrSite.ToString() == NextLand.LandmarkCode) == null)
                            //    if (CurrLand != null && CurrLand.IsRotateLand && CoreData.CarList.FirstOrDefault(p => p.AgvID != CurrCar.AgvID && CountTurnLands.Contains(p.CurrSite.ToString())) == null &&
                            //        CoreData.CarList.FirstOrDefault(q => q.AgvID != CurrCar.AgvID && CountTurnLands.Intersect(q.RouteLands).Count() > 0) == null)
                            //    {
                            //        //把启动后的车辆在锁资源中清除
                            //        List<LockResource> CurrCarLockstemp = lockResource.Where(p => p.BeLockCarID == CurrCar.AgvID).ToList();
                            //        List<LockResource> CurrCarLocks = DataToObject.CreateDeepCopy<List<LockResource>>(CurrCarLockstemp);
                            //        foreach (LockResource item in CurrCarLocks)
                            //        {
                            //            int index = lockResource.FindIndex(p => p.BeLockCarID == item.BeLockCarID);
                            //            lockResource.RemoveAt(index);
                            //        }
                            //        Commer.AGV_AddControl(CurrCar.AgvID, new CommandToValue(AGVCommandEnum.Start));
                            //        CurrCar.IsLock = false;
                            //        LogHelper.WriteTrafficLog("被管制的车停在了自己的旋转点上:" + CurrCar.AgvID.ToString() + ",在地标" + CurrCar.CurrSite.ToString() + "上,往前挪一格");
                            //    }
                            //    //else if (CurrLand != null && NextLand != null && CoreData.CarList.FirstOrDefault(q => q.AgvID != CurrCar.AgvID && q.TurnLands.Contains(CurrCar.CurrSite.ToString()) && (CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == q.CurrSite.ToString()).IsRotateLand == false)) != null && CoreData.CarList.FirstOrDefault(p => p.AgvID != CurrCar.AgvID && p.CurrSite.ToString() == NextLand.LandmarkCode) == null)
                            //    else if (CurrLand != null && CoreData.CarList.FirstOrDefault(q => q.AgvID != CurrCar.AgvID && q.TurnLands.Contains(CurrCar.CurrSite.ToString()) && (CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == q.CurrSite.ToString()).IsRotateLand == false)) != null && CoreData.CarList.FirstOrDefault(p => p.AgvID != CurrCar.AgvID && CountTurnLands.Contains(p.CurrSite.ToString())) == null &&
                            //        CoreData.CarList.FirstOrDefault(q => q.AgvID != CurrCar.AgvID && CountTurnLands.Intersect(q.RouteLands).Count() > 0) == null)
                            //    {
                            //        //把启动后的车辆在锁资源中清除
                            //        List<LockResource> CurrCarLockstemp = lockResource.Where(p => p.BeLockCarID == CurrCar.AgvID).ToList();
                            //        List<LockResource> CurrCarLocks = DataToObject.CreateDeepCopy<List<LockResource>>(CurrCarLockstemp);
                            //        foreach (LockResource item in CurrCarLocks)
                            //        {
                            //            int index = lockResource.FindIndex(p => p.BeLockCarID == item.BeLockCarID);
                            //            lockResource.RemoveAt(index);
                            //        }
                            //        Commer.AGV_AddControl(CurrCar.AgvID, new CommandToValue(AGVCommandEnum.Start));
                            //        CurrCar.IsLock = false;
                            //        LogHelper.WriteTrafficLog("被管制的车停在了别人的旋转集合中:" + CurrCar.CurrSite.ToString() + ",往前挪一格");
                            //    }
                            //}
                            #endregion
                        }
                        #endregion
                    }//endFor
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteTrafficLog("交通管制启动异常:" + ex.Message);
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
        public void GetTrafficResour(CarInfo CurrCar)
        {
            try
            {
                if (CurrCar == null) { return; }
                //CurrCar.RouteLands.Clear();
                //CurrCar.TurnLands.Clear();
                List<string> TempRouteLands = new List<string>();
                List<string> TempTurnLands = new List<string>();
                if (CoreData.SysParameter.Keys.Contains("TrafficLenth") /*&& CoreData.SysParameter.Keys.Contains("StopLenth")*/
                            && CoreData.SysParameter.Keys.Contains("TurnLenth") && CoreData.SysParameter.Keys.Contains("ScalingRate"))
                {
                    string TrafficLenth = CoreData.SysParameter["TrafficLenth"].ToString();
                    //string StopLenth = CoreData.SysParameter["StopLenth"].ToString();
                    string TurnLenth = CoreData.SysParameter["TurnLenth"].ToString();
                    string ScalingRate = CoreData.SysParameter["ScalingRate"].ToString();
                    if (!string.IsNullOrEmpty(TrafficLenth)/* && !string.IsNullOrEmpty(StopLenth)*/
                        && !string.IsNullOrEmpty(TurnLenth) && !string.IsNullOrEmpty(ScalingRate))
                    {
                        //double StopLen = Convert.ToDouble(StopLenth);
                        double TrafficLen = Convert.ToDouble(TrafficLenth);
                        double TotalStopLenth = /*StopLen + */TrafficLen;//需要发送停车距离
                        double TurnLen = Convert.ToDouble(TurnLenth);
                        double ScalingRateLen = Convert.ToDouble(ScalingRate);

                        //如果当前agv没有路线的话就把当前的站点放入到自己的行走资源集合中
                        if (CurrCar.RealyRoute.Count <= 0)
                        { return; }
                        int CurrSiteIndex = CurrCar.RealyRoute.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                        if (CurrSiteIndex < 0) { return; }

                        List<LandmarkInfo> TrafficLand = new List<LandmarkInfo>();
                        //通过管制距离的系统参数来计算需要锁定的地标集合
                        double ResourLenth = 0;
                        double UpResourLenth = 0;
                        for (int i = CurrSiteIndex + 1; i < CurrCar.RealyRoute.Count; i++)
                        {
                            AllSegment segment = CoreData.AllSeg.Where(p => p.BeginLandMakCode == (CurrCar.RealyRoute[i - 1]).LandmarkCode && p.EndLandMarkCode == (CurrCar.RealyRoute[i]).LandmarkCode).FirstOrDefault();
                            if (segment == null) { continue; }
                            ResourLenth += segment.Length;
                            if (UpResourLenth <= TotalStopLenth && ResourLenth >= TotalStopLenth)
                            {
                                if (TempRouteLands.Where(p => p == (CurrCar.RealyRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.RealyRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.RealyRoute[i].LandmarkCode);
                                    TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.RealyRoute[i]));
                                }
                                break;
                            }
                            else
                            {
                                if (TempRouteLands.Where(p => p == (CurrCar.RealyRoute[i - 1]).LandmarkCode).Count() <= 0 && CurrCar.RealyRoute[i - 1].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.RealyRoute[i - 1].LandmarkCode);
                                    TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.RealyRoute[i - 1]));
                                }
                                if (TempRouteLands.Where(p => p == (CurrCar.RealyRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.RealyRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                {
                                    TempRouteLands.Add(CurrCar.RealyRoute[i].LandmarkCode);
                                    TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.RealyRoute[i]));
                                }
                            }
                            UpResourLenth += segment.Length;
                        }

                        LandmarkInfo ld = CurrCar.RealyRoute.FirstOrDefault(q => q.LandmarkCode == CurrCar.CurrSite.ToString());
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
                            List<LandmarkInfo> CloseLands = (from a in CoreData.AllLands
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
                if (CurrCar.RouteLands.Count(p => p == CurrCar.CurrSite.ToString()) <= 0)
                { CurrCar.RouteLands.Insert(0, CurrCar.CurrSite.ToString()); }
                if (CurrCar.TurnLands.Count(p => p == CurrCar.CurrSite.ToString()) <= 0)
                { CurrCar.TurnLands.Add(CurrCar.CurrSite.ToString()); }
            }
            catch (Exception ex)
            { LogHelper.WriteTrafficLog("计算停止资源集异常:" + ex.Message); }
        }


        //单独计算被停止的车的行走资源集合
        public Hashtable SameGetRouteLandResource(CarInfo CurrCar)
        {
            try
            {
                if (CurrCar == null)
                { return null; }
                else
                {
                    List<string> TempRouteLands = new List<string>();
                    List<string> TempTurnLands = new List<string>();
                    if (CoreData.SysParameter.Keys.Contains("TrafficLenth") /*&& CoreData.SysParameter.Keys.Contains("StopLenth")*/
                                && CoreData.SysParameter.Keys.Contains("TurnLenth") && CoreData.SysParameter.Keys.Contains("ScalingRate"))
                    {
                        string TrafficLenth = CoreData.SysParameter["TrafficLenth"].ToString();
                        //string StopLenth = CoreData.SysParameter["StopLenth"].ToString();
                        string TurnLenth = CoreData.SysParameter["TurnLenth"].ToString();
                        string ScalingRate = CoreData.SysParameter["ScalingRate"].ToString();
                        if (!string.IsNullOrEmpty(TrafficLenth)/* && !string.IsNullOrEmpty(StopLenth)*/
                            && !string.IsNullOrEmpty(TurnLenth) && !string.IsNullOrEmpty(ScalingRate))
                        {
                            //double StopLen = Convert.ToDouble(StopLenth);
                            double TrafficLen = Convert.ToDouble(TrafficLenth);
                            double TotalStopLenth =/* StopLen + */TrafficLen;//需要发送停车距离
                            double TurnLen = Convert.ToDouble(TurnLenth);
                            double ScalingRateLen = Convert.ToDouble(ScalingRate);

                            //如果当前agv没有路线的话就把当前的站点放入到自己的行走资源集合中
                            if (CurrCar.RealyRoute.Count <= 0)
                            { return null; }
                            int CurrSiteIndex = CurrCar.RealyRoute.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                            if (CurrSiteIndex < 0) { return null; }

                            List<LandmarkInfo> TrafficLand = new List<LandmarkInfo>();
                            //通过管制距离的系统参数来计算需要锁定的地标集合
                            double ResourLenth = 0;
                            double UpResourLenth = 0;
                            for (int i = CurrSiteIndex + 1; i < CurrCar.RealyRoute.Count; i++)
                            {
                                AllSegment segment = CoreData.AllSeg.Where(p => p.BeginLandMakCode == (CurrCar.RealyRoute[i - 1]).LandmarkCode && p.EndLandMarkCode == (CurrCar.RealyRoute[i]).LandmarkCode).FirstOrDefault();
                                if (segment == null) { continue; }
                                ResourLenth += segment.Length;
                                if (UpResourLenth <= TotalStopLenth && ResourLenth >= TotalStopLenth)
                                {
                                    if (TempRouteLands.Where(p => p == (CurrCar.RealyRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.RealyRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                    {
                                        TempRouteLands.Add(CurrCar.RealyRoute[i].LandmarkCode);
                                        TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.RealyRoute[i]));
                                    }
                                    break;
                                }
                                else
                                {
                                    if (TempRouteLands.Where(p => p == (CurrCar.RealyRoute[i - 1]).LandmarkCode).Count() <= 0 && CurrCar.RealyRoute[i - 1].LandmarkCode != CurrCar.CurrSite.ToString())
                                    {
                                        TempRouteLands.Add(CurrCar.RealyRoute[i - 1].LandmarkCode);
                                        TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.RealyRoute[i - 1]));
                                    }
                                    if (TempRouteLands.Where(p => p == (CurrCar.RealyRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.RealyRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                    {
                                        TempRouteLands.Add(CurrCar.RealyRoute[i].LandmarkCode);
                                        TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.RealyRoute[i]));
                                    }
                                }
                                UpResourLenth += segment.Length;
                            }

                            LandmarkInfo ld = CurrCar.RealyRoute.FirstOrDefault(q => q.LandmarkCode == CurrCar.CurrSite.ToString());
                            if (ld != null && TrafficLand.Where(p => p.LandmarkCode == ld.LandmarkCode).Count() <= 0)
                            { TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(ld)); }

                            LandmarkInfo Land = (from a in TrafficLand
                                                 where a.IsRotateLand
                                                 orderby getDistant(a.LandX, a.LandY, ld.LandX, ld.LandY) ascending
                                                 select a).FirstOrDefault();
                            if (Land != null && Land.IsRotateLand)
                            {
                                //根据旋转半径找到范围内的地标集
                                List<LandmarkInfo> CloseLands = (from a in CoreData.AllLands
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
                    if (TempRouteLands.Count(p => p == CurrCar.CurrSite.ToString()) <= 0)
                    { TempRouteLands.Insert(0, CurrCar.CurrSite.ToString()); }
                    if (TempTurnLands.Count(p => p == CurrCar.CurrSite.ToString()) <= 0)
                    { TempTurnLands.Add(CurrCar.CurrSite.ToString()); }
                    hs["RouteLands"] = TempRouteLands;
                    hs["TurnLands"] = TempTurnLands;
                    return hs;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteTrafficLog("启动路径前的资源集计算异常:" + ex.Message);
                return null;
            }
        }


        //单独计算当前车的行走资源集合
        public Hashtable BeforeTrafficSameGetRouteLandResource(CarInfo CurrCar)
        {
            try
            {
                if (CurrCar == null)
                { return null; }
                else
                {
                    List<string> TempRouteLands = new List<string>();
                    List<string> TempTurnLands = new List<string>();
                    if (CoreData.SysParameter.Keys.Contains("TrafficLenth") /*&& CoreData.SysParameter.Keys.Contains("StopLenth")*/
                                && CoreData.SysParameter.Keys.Contains("TurnLenth") && CoreData.SysParameter.Keys.Contains("ScalingRate"))
                    {
                        string TrafficLenth = CoreData.SysParameter["TrafficLenth"].ToString();
                        //string StopLenth = CoreData.SysParameter["StopLenth"].ToString();
                        string TurnLenth = CoreData.SysParameter["TurnLenth"].ToString();
                        string ScalingRate = CoreData.SysParameter["ScalingRate"].ToString();
                        if (!string.IsNullOrEmpty(TrafficLenth)/* && !string.IsNullOrEmpty(StopLenth)*/
                            && !string.IsNullOrEmpty(TurnLenth) && !string.IsNullOrEmpty(ScalingRate))
                        {
                            //double StopLen = Convert.ToDouble(StopLenth);
                            double TrafficLen = Convert.ToDouble(TrafficLenth);
                            double TotalStopLenth =/* StopLen + */TrafficLen;//需要发送停车距离
                            double TurnLen = Convert.ToDouble(TurnLenth);
                            double ScalingRateLen = Convert.ToDouble(ScalingRate);

                            //如果当前agv没有路线的话就把当前的站点放入到自己的行走资源集合中
                            if (CurrCar.Route.Count <= 0)
                            { return null; }
                            int CurrSiteIndex = CurrCar.Route.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                            if (CurrSiteIndex < 0) { return null; }

                            List<LandmarkInfo> TrafficLand = new List<LandmarkInfo>();
                            //通过管制距离的系统参数来计算需要锁定的地标集合
                            double ResourLenth = 0;
                            double UpResourLenth = 0;
                            for (int i = CurrSiteIndex + 1; i < CurrCar.Route.Count; i++)
                            {
                                AllSegment segment = CoreData.AllSeg.Where(p => p.BeginLandMakCode == (CurrCar.Route[i - 1]).LandmarkCode && p.EndLandMarkCode == (CurrCar.Route[i]).LandmarkCode).FirstOrDefault();
                                if (segment == null) { continue; }
                                ResourLenth += segment.Length;
                                if (UpResourLenth <= TotalStopLenth && ResourLenth >= TotalStopLenth)
                                {
                                    if (TempRouteLands.Where(p => p == (CurrCar.Route[i]).LandmarkCode).Count() <= 0 && CurrCar.Route[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                    {
                                        TempRouteLands.Add(CurrCar.Route[i].LandmarkCode);
                                        TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.Route[i]));
                                    }
                                    break;
                                }
                                else
                                {
                                    if (TempRouteLands.Where(p => p == (CurrCar.Route[i - 1]).LandmarkCode).Count() <= 0 && CurrCar.Route[i - 1].LandmarkCode != CurrCar.CurrSite.ToString())
                                    {
                                        TempRouteLands.Add(CurrCar.Route[i - 1].LandmarkCode);
                                        TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.Route[i - 1]));
                                    }
                                    if (TempRouteLands.Where(p => p == (CurrCar.Route[i]).LandmarkCode).Count() <= 0 && CurrCar.Route[i].LandmarkCode != CurrCar.CurrSite.ToString())
                                    {
                                        TempRouteLands.Add(CurrCar.Route[i].LandmarkCode);
                                        TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.Route[i]));
                                    }
                                }
                                UpResourLenth += segment.Length;
                            }

                            LandmarkInfo ld = CurrCar.Route.FirstOrDefault(q => q.LandmarkCode == CurrCar.CurrSite.ToString());
                            if (ld != null && TrafficLand.Where(p => p.LandmarkCode == ld.LandmarkCode).Count() <= 0)
                            { TrafficLand.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(ld)); }

                            LandmarkInfo Land = (from a in TrafficLand
                                                 where a.IsRotateLand
                                                 orderby getDistant(a.LandX, a.LandY, ld.LandX, ld.LandY) ascending
                                                 select a).FirstOrDefault();
                            if (Land != null && Land.IsRotateLand)
                            {
                                //根据旋转半径找到范围内的地标集
                                List<LandmarkInfo> CloseLands = (from a in CoreData.AllLands
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
                    if (TempRouteLands.Count(p => p == CurrCar.CurrSite.ToString()) <= 0)
                    { TempRouteLands.Insert(0, CurrCar.CurrSite.ToString()); }
                    if (TempTurnLands.Count(p => p == CurrCar.CurrSite.ToString()) <= 0)
                    { TempTurnLands.Add(CurrCar.CurrSite.ToString()); }
                    hs["RouteLands"] = TempRouteLands;
                    hs["TurnLands"] = TempTurnLands;
                    return hs;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteTrafficLog("启动路径前的资源集计算异常:" + ex.Message);
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
                CarInfo HasSamDeriAnotherCar = (from a in CoreData.CarList
                                                where/* !a.bIsCommBreak &&*/ a.AgvID != CurrCarID && RouteLand.Where(q => q == a.CurrSite.ToString()).Count() > 0
                                                select a).FirstOrDefault();

                //判断是否路线交叉 排除了被我锁住的
                CarInfo HasIntersectAnotherCar = (from a in CoreData.CarList
                                                  where /*!a.bIsCommBreak && */a.AgvID != CurrCarID && RouteLand.Intersect(a.RouteLands).Count() > 0
                                                  && !RouteLand.Contains(a.CurrSite.ToString())
                                                  orderby RouteLand.Intersect(a.RouteLands).Count() descending
                                                  //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                                  select a).FirstOrDefault();

                //判断是否有旋转交集  排除了被我锁住的
                CarInfo HasTurnAnotherCar = (from a in CoreData.CarList
                                             where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCarID && RouteLand.Intersect(a.TurnLands).Count() > 0
                                             //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                             select a).FirstOrDefault();

                //判断我的旋转资源中是否有其他车  排除了被我锁住的
                CarInfo MyTurnHasAnotherCar = (from a in CoreData.CarList
                                               where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCarID && TurnLands.Contains(a.CurrSite.ToString())
                                               //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                               select a).FirstOrDefault();


                if (HasSamDeriAnotherCar != null)//同向必须停
                {
                    LogHelper.WriteTrafficLog("启动被交管停前判断跟车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,因车" + HasSamDeriAnotherCar.AgvID.ToString() + "在站点" + HasSamDeriAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }

                //找我当前车交管地标集中已经扫描到的管制区域
                //需要判断一下当前车目的地是否在交管资源中
                try
                {
                    CarInfo CrrCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == CurrCarID);
                    if (CrrCar != null)
                    {
                        IList<TraJunction> Juncs = CoreData.JunctionList.Where(p => RouteLand.Intersect(p.JuncLandCodes).ToList().Count > 0
       && ((p.Segments.Count > 0 && p.Segments.Count(q => CrrCar.RealyAllRouteLandCode.IndexOf(q.LandCodes) >= 0) > 0) || p.Segments.Count == 0)).ToList();
                        bool IsStop = false;
                        foreach (TraJunction Junc in Juncs)
                        {
                            //判断扫描到的管制区域内锁定车辆数量
                            //List<CarInfo> HasAnotherCars = CoreData.CarList.Where(p => p.AgvID != CurrCarID && Junc.JuncLandCodes.Contains(p.CurrSite.ToString())).ToList();
                            //if (HasAnotherCars != null && HasAnotherCars.Count >= Junc.Carnumber)
                            if (Junc.LockCars.Count(p => p.AgvID != CrrCar.AgvID) >= Junc.Carnumber)
                            {
                                //if (lockResource.Where(p => p.LockCarID == HasAnotherCar.AgvID && p.BeLockCarID == CurrCarID).Count() <= 0)
                                //{
                                //    LockResource LockBlock = new LockResource();
                                //    LockBlock.BeLockCarID = CurrCarID;
                                //    LockBlock.LockCarID = HasAnotherCar.AgvID;
                                //    lockResource.Add(LockBlock);
                                //}
                                CarInfo HasAnotherCar = Junc.LockCars.FirstOrDefault(p => p.AgvID != CrrCar.AgvID);
                                if (HasAnotherCar != null)
                                { LogHelper.WriteTrafficLog("启动被交管停前判断自定义管制区域" + Junc.TraJunctionID.ToString() + "有车:" + HasAnotherCar.AgvID.ToString() + "在站点" + HasAnotherCar.CurrSite.ToString() + "没出,当前车" + CrrCar.AgvID.ToString() + "在站点" + CrrCar.CurrSite.ToString() + "停"); }
                                IsStop = true;
                            }
                            else
                            {
                                if (Junc.LockCars.Count(p => p.AgvID == CrrCar.AgvID) <= 0)
                                { Junc.LockCars.Add(CrrCar); }
                            }
                        }
                        if (IsStop)
                        { return true; }
                    }
                }
                catch (Exception ex)
                { LogHelper.WriteTrafficLog("启动被交管停前判断自定义管制处理逻辑异常：" + ex.Message + ex.StackTrace); }
                //JunctionInfo Junc = (from a in CoreData.JunctionList
                //                     where RouteLand.Where(p => p == a.EnterLandCode).Count() > 0
                //                     && a.OwerAGVID != -1&&a.OwerAGVID!=CurrCarID
                //                     select a).FirstOrDefault();

                //if (Junc != null)
                //{
                //    CarInfo SourceCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == Junc.OwerAGVID);
                //    if (SourceCar != null)
                //    {
                //        LogHelper.WriteLog("启动前判断管制区域有车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停" + SourceCar.AgvID.ToString() + "在站点" + SourceCar.CurrSite.ToString() + "阻挡");
                //    }
                //    return true;
                //}

                ////我的旋转区域内有车，我必须停,不管对方是否不是被我锁住的，也要被对方锁住
                if (MyTurnHasAnotherCar != null)
                {
                    LogHelper.WriteLog("启动被交管停前判断我的旋转资源中有车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,车" + MyTurnHasAnotherCar.AgvID.ToString() + "在站点" + MyTurnHasAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }
                if (HasIntersectAnotherCar != null)
                {
                    //启动交叉停止的不需要判断谁近谁远
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
                    LogHelper.WriteTrafficLog("启动被交管停前判断交叉汇车,车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,车" + HasIntersectAnotherCar.AgvID.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }

                if (HasTurnAnotherCar != null)
                {
                    ////如果发现我的行走路线中有需要旋转的车辆,并且旋转的车已经在旋转集合中那么不需要判断对方是否暂停,必须自己停
                    if (HasTurnAnotherCar.TurnLands.Contains(HasTurnAnotherCar.CurrSite.ToString()))
                    {
                        LogHelper.WriteTrafficLog("启动被交管停前判断有旋转且在自己的旋转区域内,所以当前车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "继续停,车" + HasTurnAnotherCar.AgvID.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "阻挡");
                        return true;
                    }

                    LogHelper.WriteTrafficLog("启动被交管停前判断前方有其他车即将旋转的区域,所以当前车" + CurrCarID.ToString() + "在站点:" + CurrSite.ToString() + "停,车" + HasTurnAnotherCar.AgvID.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteTrafficLog("启动被交管停前判断交通管制异常:" + ex.Message);
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
                CarInfo CurrCar = obj as CarInfo;
                if (CurrCar == null) { return true; }
                #region New
                //判断停止的车的资源集已在上面通过停车距离系统参数计算过了
                Hashtable hs = SameGetRouteLandResource(CurrCar);
                LogHelper.WriteTrafficLog("启动停止的车前判断" + CurrCar.AgvID.ToString() + "行走资源集合:" + string.Join(",", (hs["RouteLands"] as List<string>).Select(p => p)));
                LogHelper.WriteTrafficLog("启动停止的车前判断" + CurrCar.AgvID.ToString() + "旋转资源集合:" + string.Join(",", (hs["TurnLands"] as List<string>).Select(p => p)));
                return SameCheckIsStop(CurrCar.AgvID, CurrCar.CurrSite, hs);
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteTrafficLog("启动路径前判断交通管制异常:" + ex.Message);
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
                    CarInfo CurrCar = obj as CarInfo;
                    if (CurrCar == null) { return true; }
                    #region New
                    Hashtable hs = BeforeTrafficSameGetRouteLandResource(CurrCar);
                    if (hs == null)
                    {
                        LogHelper.WriteTrafficLog("启动前判断车" + CurrCar.AgvID.ToString() + "线路未能计算出行走资源集合");
                        return false;
                    }
                    LogHelper.WriteTrafficLog("启动前判断车" + CurrCar.AgvID.ToString() + "行走资源集合:" + string.Join(",", (hs["RouteLands"] as List<string>).Select(p => p)));
                    LogHelper.WriteTrafficLog("启动前判断车" + CurrCar.AgvID.ToString() + "旋转资源集合:" + string.Join(",", (hs["TurnLands"] as List<string>).Select(p => p)));
                    return BeforStartCheckIsStop(CurrCar.AgvID, CurrCar.CurrSite, hs);
                    #endregion
                }
                else
                { return true; }
            }
            catch (Exception ex)
            {
                LogHelper.WriteTrafficLog("启动前判断交通管制异常:" + ex.Message + ex.StackTrace);
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
                CarInfo HasSamDeriAnotherCar = (from a in CoreData.CarList
                                                where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCarID && RouteLand.Where(q => q == a.CurrSite.ToString()).Count() > 0
                                                select a).FirstOrDefault();

                //判断是否路线交叉 排除了被我锁住的
                CarInfo HasIntersectAnotherCar = (from a in CoreData.CarList
                                                  where /*!a.bIsCommBreak &&*/ a.AgvID != CurrCarID && RouteLand.Intersect(a.RouteLands).Count() > 0
                                                  && !RouteLand.Contains(a.CurrSite.ToString())
                                                  orderby RouteLand.Intersect(a.RouteLands).Count() descending
                                                  //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                                  select a).FirstOrDefault();

                //判断是否有旋转交集  排除了被我锁住的
                CarInfo HasTurnAnotherCar = (from a in CoreData.CarList
                                             where/* !a.bIsCommBreak && */a.AgvID != CurrCarID && RouteLand.Intersect(a.TurnLands).Count() > 0
                                             //&& lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                             select a).FirstOrDefault();

                //判断我的旋转资源中是否有其他车  排除了被我锁住的
                CarInfo MyTurnHasAnotherCar = (from a in CoreData.CarList
                                               where/* !a.bIsCommBreak &&*/ a.AgvID != CurrCarID && TurnLands.Contains(a.CurrSite.ToString())
                                               && lockResource.Count(p => p.BeLockCarID == a.AgvID && p.LockCarID == CurrCarID) <= 0
                                               select a).FirstOrDefault();

                //我的旋转资源集中是否跟其他车线路冲突
                CarInfo MyTurnIntersectRoute = (from a in CoreData.CarList
                                                where a.AgvID != CurrCarID && TurnLands.Intersect(a.RouteLands).Count() > 0
                                                select a).FirstOrDefault();

                //判断我的旋转资源集和其他车的旋转资源集是否有冲突
                CarInfo SameTurnAnotherCar = (from a in CoreData.CarList
                                              where /*!a.bIsCommBreak && */a.AgvID != CurrCarID && TurnLands.Intersect(a.TurnLands).Count() > 0
                                              select a).FirstOrDefault();

                if (HasSamDeriAnotherCar != null)//同向必须停
                {
                    LogHelper.WriteTrafficLog("启动线路前判断跟车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "等待启动,因车" + HasSamDeriAnotherCar.AgvID.ToString() + "在站点" + HasSamDeriAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }

                //我的旋转区域内有车，我必须停,不管对方是否不是被我锁住的，也要被对方锁住
                if (MyTurnHasAnotherCar != null)
                {
                    LogHelper.WriteTrafficLog("启动线路前判断我的旋转资源中有车,所以当前车" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "等待启动,车" + MyTurnHasAnotherCar.AgvID.ToString() + "在站点" + MyTurnHasAnotherCar.CurrSite.ToString() + "阻挡");
                 //   return true;
                }

                if (MyTurnIntersectRoute != null)
                {
                    LogHelper.WriteTrafficLog("启动线路前判断我的旋转资源中有车的行走线路,所以当前车" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "等待启动,车" + MyTurnIntersectRoute.AgvID.ToString() + "在站点" + MyTurnIntersectRoute.CurrSite.ToString() + "阻挡");
                    return true;
                }

                if (SameTurnAnotherCar != null)
                {
                    LogHelper.WriteTrafficLog("启动线路前判断我的旋转资源中和其他车旋转资源集冲突,所以当前车" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "等待启动,车" + SameTurnAnotherCar.AgvID.ToString() + "在站点" + SameTurnAnotherCar.CurrSite.ToString() + "阻挡");
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

                    LogHelper.WriteTrafficLog("启动线路前判断前方有其他车即将旋转区域,所以当前车" + CurrCarID.ToString() + "在站点:" + CurrSite.ToString() + "等待启动,即将旋转的车" + HasTurnAnotherCar.AgvID.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "继续走");
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
                    LogHelper.WriteTrafficLog("启动线路前判断交叉汇车,当前车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "等待启动,车" + HasIntersectAnotherCar.AgvID.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "阻挡");
                    return true;
                }


                //找我当前车交管地标集中已经扫描到的管制区域
                //需要判断一下当前车目的地是否在交管资源中
                try
                {
                    CarInfo CrrCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == CurrCarID);
                    if (CrrCar != null)
                    {
                        IList<TraJunction> Juncs = CoreData.JunctionList.Where(p => RouteLand.Intersect(p.JuncLandCodes).ToList().Count > 0
         && ((p.Segments.Count > 0 && p.Segments.Count(q => CrrCar.AllRouteLandCode.IndexOf(q.LandCodes) >= 0) > 0) || p.Segments.Count == 0)).ToList();

                        bool IsStop = false;
                        foreach (TraJunction Junc in Juncs)
                        {
                            //判断扫描到的管制区域内是否有其他的车
                            //List<CarInfo> HasAnotherCars = CoreData.CarList.Where(p => p.AgvID != CurrCarID && Junc.JuncLandCodes.Contains(p.CurrSite.ToString())).ToList();
                            //if (HasAnotherCars != null && HasAnotherCars.Count >= Junc.Carnumber)
                            if (Junc.LockCars.Count(p => p.AgvID != CrrCar.AgvID) >= Junc.Carnumber)
                            {
                                //if (lockResource.Where(p => p.LockCarID == HasAnotherCar.AgvID && p.BeLockCarID == CurrCarID).Count() <= 0)
                                //{
                                //    LockResource LockBlock = new LockResource();
                                //    LockBlock.BeLockCarID = CurrCarID;
                                //    LockBlock.LockCarID = HasAnotherCar.AgvID;
                                //    lockResource.Add(LockBlock);
                                //}
                                CarInfo HasAnotherCar = Junc.LockCars.FirstOrDefault(p => p.AgvID != CrrCar.AgvID);
                                if (HasAnotherCar != null)
                                { LogHelper.WriteTrafficLog("启动线路前判断自定义管制区域" + Junc.TraJunctionID.ToString() + "有车:" + HasAnotherCar.AgvID.ToString() + "在站点" + HasAnotherCar.CurrSite.ToString() + "没出,当前车" + CrrCar.AgvID.ToString() + "在站点" + CrrCar.CurrSite.ToString() + "等待启动"); }
                                IsStop = true;
                                break;
                            }
                            else
                            {
                                //if (Junc.LockCars.Count(p => p.AgvID == CrrCar.AgvID) <= 0)
                                //{ Junc.LockCars.Add(CrrCar); }
                            }
                        }
                        if (IsStop)
                        { return true; }
                        foreach (TraJunction Junc in Juncs)
                        {
                            //判断扫描到的管制区域内是否有其他的车
                            //List<CarInfo> HasAnotherCars = CoreData.CarList.Where(p => p.AgvID != CurrCarID && Junc.JuncLandCodes.Contains(p.CurrSite.ToString())).ToList();
                            //if (HasAnotherCars != null && HasAnotherCars.Count >= Junc.Carnumber)
                            if (Junc.LockCars.Count(p => p.AgvID != CrrCar.AgvID) >= Junc.Carnumber)
                            {
                            }
                            else
                            {
                                if (Junc.LockCars.Count(p => p.AgvID == CrrCar.AgvID) <= 0)
                                { Junc.LockCars.Add(CrrCar); }
                            }
                        }

                    }
                }
                catch (Exception ex)
                { LogHelper.WriteTrafficLog("启动线路前判断自定义管制处理逻辑异常：" + ex.Message); }

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteTrafficLog("启动线路前判断交通管制异常:" + ex.Message);
                return true;
            }
        }

        public void CheckCarIsLockJuck(CarInfo CurrCar)
        {
            try
            {
                List<TraJunction> CurrCarLockJuncs = CoreData.JunctionList.Where(p => p.LockCars.Count(q => q.AgvID == CurrCar.AgvID) > 0).ToList();
                foreach (TraJunction Junc in CurrCarLockJuncs)
                {
                    LogHelper.WriteTrafficLog("车" + CurrCar.AgvID.ToString() + "锁定的管制区域ID是:" + Junc.TraJunctionID.ToString() + ",判断是否移除");
                    if (CurrCar.RouteLands.Intersect(Junc.JuncLandCodes).ToList().Count <= 0)
                    {
                        LogHelper.WriteTrafficLog("车" + CurrCar.AgvID.ToString() + "锁定的管制区域ID是:" + Junc.TraJunctionID.ToString() + ",车行走资源集是:" + string.Join(",", CurrCar.RouteLands.Select(p => p)) + "移除前锁定车集合数量为:" + Junc.LockCars.Count.ToString());
                        CarInfo removeCar = Junc.LockCars.FirstOrDefault(p => p.AgvID == CurrCar.AgvID);
                        if (removeCar != null)
                        {
                            Junc.LockCars.Remove(removeCar);
                            LogHelper.WriteTrafficLog("车" + CurrCar.AgvID.ToString() + "锁定的管制区域ID是:" + Junc.TraJunctionID.ToString() + "移除后锁定车集合数量为:" + Junc.LockCars.Count.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            { LogHelper.WriteTrafficLog("检查当前车是否解锁管制区域异常:" + ex.Message); }
        }

    }//end
}
