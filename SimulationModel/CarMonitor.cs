using AGVDAccess;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulationModel
{
    public class CarMonitor
    {
        public CarMonitor()
        {
            IsLock = false;
            Sate = 0;
            CurrLand = new LandmarkInfo();
            NextLand = new LandmarkInfo();
            CurrRoute = new List<LandmarkInfo>();
            RouteLands = new List<string>();
            TurnLands = new List<string>();
            StandbyLandMark = "";
            ExcuteTaksNo = "";
            ArmLand = "";
            Speed = 0.02;
            CurrSite = -1;
            CarActive_Timer = new System.Timers.Timer() { Enabled = false };
            CarActive_Timer.Elapsed += new System.Timers.ElapsedEventHandler(CarActive);
            CarActive_Timer.Interval = 50;
        }

        public bool IsLock { get; set; }

        public int AgvID { get; set; }

        public LandmarkInfo CurrLand { get; set; }

        public int CurrSite { get; set; }

        public int OldSite { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public double Rundistance { get; set; }

        public double Speed { get; set; }

        public LandmarkInfo NextLand { get; set; }

        /// <summary>
        /// 0,待命 1，运行中 2，停止
        /// </summary>
        public int Sate { get; set; }

        public List<LandmarkInfo> CurrRoute { get; set; }

        public List<string> RouteLands { get; set; }

        public List<string> TurnLands { get; set; }

        public string StandbyLandMark { get; set; }

        public string ExcuteTaksNo { get; set; }

        public bool IsBack { get; set; }

        public int TaskDetailID { get; set; }

        public string ArmLand { get; set; }

        public int OperType { get; set; }

        public int PutType { get; set; }

        public double ScalingRate { get; set; }


        public delegate void CarStepChange(object sender);
        public event CarStepChange StepChange;
        public System.Timers.Timer CarActive_Timer;

        private bool IsMove = false;


        public void Move()
        {
            try
            {

                CarActive_Timer.Enabled = false;
                if (IsMove)
                {
                    if (CurrRoute.Count() > 0 && ScalingRate > 0)
                    {
                        CurrRoute = CurrRoute.Distinct().ToList<LandmarkInfo>();

                        if (Math.Abs(X - CurrRoute.Last().LandX * ScalingRate) <= 0.035 && Math.Abs(Y - CurrRoute.Last().LandY * ScalingRate) <= 0.035)
                        {
                            DateTime dtBegin = DateTime.Now;
                            CurrLand = CurrRoute.Last();
                            X = (float)(CurrLand.LandX * ScalingRate);
                            Y = (float)(CurrLand.LandY * ScalingRate);
                            NextLand = null;
                            CurrSite = Convert.ToInt16(CurrLand.LandmarkCode);
                            Rundistance = 0;
                            CurrRoute.Clear();
                            Sate = 0;
                            CarActive_Timer.Enabled = false;
                            SendMove();
                            return;
                        }
                        if (Sate == 0)
                        {
                            DateTime dtBegin = DateTime.Now;
                            CurrLand = CurrRoute[0];
                            Rundistance = 0;
                            X = (float)(CurrLand.LandX * ScalingRate);
                            Y = (float)(CurrLand.LandY * ScalingRate);
                            OldSite = CurrSite;
                            CurrSite = Convert.ToInt16(CurrLand.LandmarkCode);
                            int index = CurrRoute.FindIndex(p => p.LandmarkCode == CurrLand.LandmarkCode);
                            if (index + 1 < CurrRoute.Count)
                            { NextLand = CurrRoute[index + 1]; }
                            Sate = 1;
                        }
                        else
                        {
                            DateTime dtBegin = DateTime.Now;
                            LandmarkInfo land = CurrRoute.FirstOrDefault(p => Math.Abs(p.LandX * ScalingRate - X) <= 0.035 && Math.Abs(p.LandY * ScalingRate - Y) <= 0.035);
                            if (land != null && NextLand != null && land.LandmarkCode == NextLand.LandmarkCode)
                            {
                                X = (float)(land.LandX * ScalingRate);
                                Y = (float)(land.LandY * ScalingRate);
                                int index = CurrRoute.FindIndex(p => p.LandmarkCode == land.LandmarkCode);
                                CurrLand = CurrRoute[index];
                                OldSite = CurrSite;
                                CurrSite = Convert.ToInt16(CurrLand.LandmarkCode);
                                if (index + 1 < CurrRoute.Count)
                                { NextLand = CurrRoute[index + 1]; }
                                Rundistance = 0;
                                Sate = 1;
                                DateTime dtEnd = DateTime.Now;
                            }
                            else
                            {
                                DateTime dtBegin1 = DateTime.Now;
                                if (CurrLand != null && NextLand != null)
                                {
                                    Rundistance += Speed;
                                    DateTime dtEnd1 = DateTime.Now;
                                    Sate = 1;
                                }
                            }
                        }
                        //DateTime dtBegin2 = DateTime.Now;
                        SendMove();
                        //DateTime dtEnd2 = DateTime.Now;
                    }
                    else
                    {
                        Sate = 0;
                        RouteLands.Clear();
                        TurnLands.Clear();
                        this.Dispose();
                        CarActive_Timer.Enabled = false;
                    }
                }
                CarActive_Timer.Enabled = true;
            }
            catch (Exception ex)
            { throw ex; }
            //finally
            //{CarActive_Timer.Enabled = true;}
        }

        private async void SendMove()
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    if (StepChange != null)
                    { StepChange(this); }
                });
            }
            catch
            { }
        }

        /// <summary>
        /// 停车
        /// </summary>
        public void Stop()
        {
            try
            {
                IsMove = CarActive_Timer.Enabled = false;
            }
            catch
            { }
        }

        public void Dispose()
        {
            IsMove = CarActive_Timer.Enabled = false;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            try
            {
                IsMove = CarActive_Timer.Enabled = true;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void CarActive(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Thread.Sleep(25);
                Move();
            }
            catch (Exception ex)
            { }
        }
    }
}
