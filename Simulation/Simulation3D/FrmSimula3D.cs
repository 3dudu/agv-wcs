using Canvas3D.Canvass3DDrawObj;
using DevExpress.Utils;
using Model.MDM;
using Simulation.SimulationCommon;
using Simulation.SimulationSysForm;
using SQLServerOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnyCAD.Platform;
using Application = System.Windows.Forms.Application;
using Model.CarInfoExtend;
using System.Runtime.InteropServices;
using Canvas;
using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using SimulationModel;
using Model.Comoon;
using AGVDAccess;
using System.Collections;

namespace Simulation.Simulation3D
{
    public partial class FrmSimula3D : BaseForm
    {
        #region 属性
        AnyCAD.Platform.Application theApplication = new AnyCAD.Platform.Application();
        BrepTools shapeMaker = new BrepTools();
        AnyCAD.Platform.View3d theView;
        int ShapeID = 100;
        double GroudHeight = 0;
        Hashtable CarDic = new Hashtable();
        #endregion

        #region 构造函数
        public FrmSimula3D()
        {
            InitializeComponent();
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.OnMouseWheel);

            bool isSucc = true;
            try
            {
                ConnectConfigTool.setDBase();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message + "请检查...");
                isSucc = false;
            }
            if (ConnectConfigTool.DBase == null || !isSucc)
            {
                //弹出维护数据库维护界面
                using (FrmSysConnSet frm = new FrmSysConnSet())
                {
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        Application.ExitThread();
                        Application.Exit();
                        return;
                    }
                }
            }
            try
            {
                ConnectConfigTool.setDBase();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message + "请检查...");
                return;
            }
        }
        #endregion

        #region 窗体事件
        private void FrmSimula3D_Load(object sender, EventArgs e)
        {
            try
            {
                theApplication.Initialize();
                this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
                Size size = this.pnlMain.Size;
                theView = theApplication.CreateView(pnlMain.Handle.ToInt32(), size.Width, size.Height);
                theView.RequestDraw();
                this.timerDraw.Enabled = true;
                CreatAxis();
                InitialMap();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmSimula3D_Shown(object sender, EventArgs e)
        {

        }

        private void FrmSimula3D_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                Size size = this.pnlMain.Size;
                if (theView != null)
                { theView.OnSize(size.Width, size.Height); }
            }
            catch
            { }
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            ViewUtility.OnMouseWheelEvent(theView, e);
        }

        private void timerDraw_Tick(object sender, EventArgs e)
        {
            try
            {
                theView.RequestDraw();
                theView.Redraw();
            }
            catch
            { }
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (theView == null)
                { return; }
                theView.Redraw();
            }
            catch
            { }
        }

        private void pnlMain_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ViewUtility.OnMouseDownEvent(theView, e);
            }
            catch
            { }
        }

        private void pnlMain_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                ViewUtility.OnMouseUpEvent(theView, e);
            }
            catch
            { }
        }

        private void pnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                ViewUtility.OnMouseMoveEvent(theView, e);
            }
            catch
            { }
        }


        private void btnLoadShapMode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "STL (*.stl)|*.stl|All Files(*.*)|*.*";
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    using (WaitDialogForm dialog = new WaitDialogForm("正在加载,请稍后...", "提示"))
                    {
                        LoadShapMode(dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        //TopoShape Moveshape = null;
        //float xdis = 0;
        //float zdis = 0;
        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                theView.GetSceneManager().ClearNodes();
                CreatAxis();


                //TopoShapeConvert convertor = new TopoShapeConvert();
                //TopoShape dd = shapeMaker.Translate(Moveshape, new Vector3(xdis, 0, zdis));
                //ShowTopoShape(dd);
                //xdis += 1;
                //zdis += 1;
                //SceneNode ss = theView.GetSceneManager().FindNode(ShapeID - 1);
                //SceneManager Scemange = theView.GetSceneManager();
                //Scemange.RemoveNode(ss);

                //SceneManager Scemange = theView.GetSceneManager();
                //SceneNode ss = theView.GetSceneManager().FindNode(ShapeID - 1);
                //Scemange.SelectNode(ss);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        #endregion

        #region 自定义函数
        /// <summary>
        /// 初始化调度系统地图
        /// </summary>
        /// <returns></returns>
        private bool InitialMap()
        {
            try
            {
                using (WaitDialogForm dialog = new WaitDialogForm("正在初始化地图,请稍后...", "提示"))
                {
                    LoadAGVDispatchMap();
                    CreatCarModeByCarInfos();
                }
                return true;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); return false; }
        }

        /// <summary>
        /// 抽象出物体粒子
        /// </summary>
        private SceneNode ShowTopoShape(TopoShape topoShape)
        {
            try
            {
                TopoShapeConvert convertor = new TopoShapeConvert();
                SceneNode faceNode = convertor.ToFaceNode(topoShape, 0.5f);
                if (faceNode != null)
                {
                    faceNode.SetId(ShapeID);
                    theView.GetSceneManager().AddNode(faceNode);
                    ShapeID += 1;
                }
                return faceNode;
            }
            catch (Exception ex)
            { return null; }
        }



        /// <summary>
        /// 抽象出模拟小车物体粒子
        /// </summary>
        private SceneNode ShowToCarpoShape(TopoShape topoShape, int CarID)
        {
            try
            {
                TopoShapeConvert convertor = new TopoShapeConvert();
                SceneNode faceNode = convertor.ToFaceNode(topoShape, 0.5f);
                if (faceNode != null)
                {
                    faceNode.SetId(ShapeID);
                    theView.GetSceneManager().AddNode(faceNode);
                    ShapeID += 1;
                }
                return faceNode;
            }
            catch (Exception ex)
            { return null; }
        }

        /// <summary>
        /// 绘制坐标轴
        /// </summary>
        private void CreatAxis()
        {
            try
            {
                Size pnlCanvasSize = pnlMain.Size;
                float AxisHeight = 50;
                double Lenth = 20;

                //X轴
                TopoShape CylindeX = shapeMaker.MakeCylinder(new Vector3(-pnlCanvasSize.Width, -pnlCanvasSize.Height, AxisHeight), new Vector3(1, 0, 0), 1, Lenth, 360);
                FaceStyle CylindeXStyle = new FaceStyle();
                CylindeXStyle.SetColor(new ColorValue(2.55f, 2.55f, 0f, 1));
                SceneNode sceneNode = ShowTopoShape(CylindeX);
                sceneNode.SetFaceStyle(CylindeXStyle);
                TopoShape ConeX = shapeMaker.MakeCone(new Vector3(-pnlCanvasSize.Width + (float)Lenth, -pnlCanvasSize.Height, AxisHeight), new Vector3(1, 0, 0), 2, 5, 0, 360);
                FaceStyle ConeXStyle = new FaceStyle();
                ConeXStyle.SetColor(new ColorValue(2.55f, 2.55f, 0f, 1));
                SceneNode sceneNodeConeX = ShowTopoShape(ConeX);
                sceneNodeConeX.SetFaceStyle(ConeXStyle);
                //Y轴
                TopoShape CylindeY = shapeMaker.MakeCylinder(new Vector3(-pnlCanvasSize.Width, -pnlCanvasSize.Height, AxisHeight), new Vector3(0, 1, 0), 1, Lenth, 360);
                FaceStyle CylindeYStyle = new FaceStyle();
                CylindeYStyle.SetColor(new ColorValue(2.55f, 0.48f, 0.48f, 1));
                SceneNode sceneNodeX = ShowTopoShape(CylindeY);
                sceneNodeX.SetFaceStyle(CylindeYStyle);

                TopoShape ConeY = shapeMaker.MakeCone(new Vector3(-pnlCanvasSize.Width, -pnlCanvasSize.Height + (float)Lenth, AxisHeight), new Vector3(0, 1, 0), 2, 5, 0, 360);
                FaceStyle ConeYStyle = new FaceStyle();
                ConeYStyle.SetColor(new ColorValue(2.55f, 0.48f, 0.48f, 1));
                SceneNode sceneNodeConeY = ShowTopoShape(ConeY);
                sceneNodeConeY.SetFaceStyle(ConeYStyle);

                //Z轴
                TopoShape CylindeZ = shapeMaker.MakeCylinder(new Vector3(-pnlCanvasSize.Width, -pnlCanvasSize.Height, AxisHeight), new Vector3(0, 0, 1), 1, Lenth, 360);
                FaceStyle CylindeZStyle = new FaceStyle();
                CylindeZStyle.SetColor(new ColorValue(0.3f, 1.3f, 0.3f, 1));
                SceneNode sceneNodeZ = ShowTopoShape(CylindeZ);
                sceneNodeZ.SetFaceStyle(CylindeZStyle);

                TopoShape ConeZ = shapeMaker.MakeCone(new Vector3(-pnlCanvasSize.Width, -pnlCanvasSize.Height, AxisHeight + (float)Lenth), new Vector3(0, 0, 1), 2, 5, 0, 360);
                FaceStyle ConeZStyle = new FaceStyle();
                ConeZStyle.SetColor(new ColorValue(0.3f, 1.3f, 0.3f, 1));
                SceneNode sceneNodeConeZ = ShowTopoShape(ConeZ);
                sceneNodeConeZ.SetFaceStyle(ConeZStyle);

                //重新绘制地面
                //TopoShape GroundShap = shapeMaker.MakeBox(new Vector3(-pnlCanvasSize.Width, pnlCanvasSize.Height, 0), new Vector3(pnlCanvasSize.Width, pnlCanvasSize.Height, 0), Width, GroudHeight);
                //FaceStyle GroundShapStyle = new FaceStyle();
                //GroundShapStyle.SetColor(new ColorValue(0.47f, 0.79f, 0.79f, 1));
                //SceneNode sceneNodeGround = ShowTopoShape(GroundShap);
                //sceneNodeGround.SetFaceStyle(GroundShapStyle);
            }
            catch (Exception ex)
            { throw ex; }
        }


        /// <summary>
        /// 加载三位物体模型
        /// </summary>
        /// <param name="FilePath"></param>
        public void LoadShapMode(string FilePath)
        {
            try
            {
                TopoShape Moveshape = shapeMaker.LoadFile(FilePath);
                if (Moveshape != null)
                { ShowTopoShape(Moveshape); }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 清楚场景
        /// </summary>
        public void ClearScene()
        {
            try
            {
                theView.GetSceneManager().ClearNodes();
                CreatAxis();
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region 根据调度系统数据创建调度系统3D地图模型


        /// <summary>
        /// 加载AGV调度系统地图
        /// </summary>
        public bool LoadAGVDispatchMap()
        {
            try
            {
                //将所有的地标抽离为3D物体
                IList<LandMarkShap> LandShaps = AGVDAccess.AGVSimulationDAccess.LoadLandShaps();
                if (LandShaps != null && LandShaps.Count > 0)
                {
                    foreach (LandMarkShap landShap in LandShaps)
                    {
                        float X = (float)Math.Round(landShap.LandX, 1) * 40f + (0.05f * 40f);
                        float Y = (float)Math.Round(landShap.LandY, 1) * 40f - (0.1f * 40f);
                        TopoShape LandShap = shapeMaker.MakeCylinder(new Vector3(X, Y, 0f), new Vector3(0, 0, 1), 0.1f * 40f, GroudHeight + 0.3, 360);
                        FaceStyle LandShapStyle = new FaceStyle();
                        LandShapStyle.SetColor(new ColorValue(0f, 0f, 0f, 1));
                        SceneNode sceneNodeLandShap = ShowTopoShape(LandShap);
                        sceneNodeLandShap.SetFaceStyle(LandShapStyle);
                    }
                }
                //将所有路径
                IList<AllSegment> AllSegments = AGVDAccess.AGVClientDAccess.LoadAllSegment();
                if (AllSegments != null && AllSegments.Count > 0)
                {
                    foreach (AllSegment Segment in AllSegments)
                    {
                        LandMarkShap BeinLand = LandShaps.FirstOrDefault(p => p.LandmarkCode == Segment.BeginLandMakCode);
                        LandMarkShap EndLand = LandShaps.FirstOrDefault(p => p.LandmarkCode == Segment.EndLandMarkCode);
                        if (BeinLand != null && EndLand != null)
                        {
                            float BeginX = (float)Math.Round(BeinLand.LandX, 1) * 40f + (0.05f * 40f);
                            float BeginY = (float)Math.Round(BeinLand.LandY, 1) * 40f - (0.1f * 40f);
                            float EndX = (float)Math.Round(EndLand.LandX, 1) * 40f + (0.05f * 40f);
                            float EndY = (float)Math.Round(EndLand.LandY, 1) * 40f - (0.1f * 40f);
                            if (Segment.SegmentType == 0)//直线
                            {
                                TopoShape RouteShap = shapeMaker.MakeRoad(new Vector3(BeginX, BeginY, 0), new Vector3(EndX, EndY, 0), 2, GroudHeight + 0.2, 0.2, false);
                                FaceStyle RouteStyle = new FaceStyle();
                                RouteStyle.SetColor(new ColorValue(2.05f, 1.73f, 0f, 1));
                                SceneNode sceneNodeRouteShap = ShowTopoShape(RouteShap);
                                sceneNodeRouteShap.SetFaceStyle(RouteStyle);
                            }
                            else if (Segment.SegmentType == 1)//圆弧线
                            {
                                try
                                {
                                    PointF p1 = new PointF(BeginX, BeginY);
                                    PointF p2 = new PointF(EndX, EndY);
                                    PointF p3 = new PointF((float)(Segment.Point3X * 40f), (float)(Segment.Point3Y * 40));
                                    PointF p4 = new PointF((float)(Segment.Point4X * 40f), (float)(Segment.Point4Y * 40));

                                    PointF[] pointList = new PointF[] { p1, p4, p3, p2 };
                                    PointF[] list = draw_bezier_curves(pointList, pointList.Length, 0.001f * 40);
                                    PointF[] unitlist = list.Where(p => p.X != p3.X && p.Y != p3.Y && p.X != p4.X && p.Y != p4.Y).ToArray();
                                    for (int i = 1; i < list.Length; i++)
                                    {
                                        TopoShape RouteShap = shapeMaker.MakeRoad(new Vector3(unitlist[i - 1].X, unitlist[i - 1].Y, 0), new Vector3(unitlist[i].X, unitlist[i].Y, 0), 2, GroudHeight + 0.2, 0.2, false);
                                        FaceStyle RouteStyle = new FaceStyle();
                                        RouteStyle.SetColor(new ColorValue(2.05f, 1.73f, 0f, 1));
                                        SceneNode sceneArc = ShowTopoShape(RouteShap);
                                        sceneArc.SetFaceStyle(RouteStyle);
                                    }
                                }
                                catch (Exception ex)
                                { }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            { throw ex; }
        }


        /// <summary>
        /// 根据三个点坐标计算圆心坐标
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        private PointF CountArcCenterPoint(PointF P1, PointF P2, PointF P3)
        {
            PointF center = new PointF();
            try
            {
                float a = 2 * (P2.X - P1.X);
                float b = 2 * (P2.Y - P1.Y);
                float c = P2.X * P2.X + P2.Y * P2.Y - P1.X * P1.X - P1.Y * P1.Y;
                float d = 2 * (P3.X - P2.X);
                float e = 2 * (P3.Y - P2.Y);
                float f = P3.X * P3.X + P3.Y * P3.Y - P2.X * P2.X - P2.Y * P2.Y;
                float x = (b * f - e * c) / (b * d - e * a) == 0 ? 1 : (b * d - e * a);
                float y = (d * c - a * f) / (b * d - e * a) == 0 ? 1 : (b * d - e * a);
                //float r = (float)Math.Sqrt((double)((x - P1.X) * (x - P1.X) + (y - P1.Y) * (y - P1.Y)));
                //R = r;
                center.X = x;
                center.Y = y;
            }
            catch
            { }
            return center;
        }
        #endregion

        #region 根据车辆信息创建车辆模型
        private void CreatCarModeByCarInfos()
        {
            try
            {
                


                //PointF CarP1 = new PointF(); 


                //float BeginX = (float)Math.Round(BeinLand.LandX, 1) * 40f + (0.05f * 40f);
                //float BeginY = (float)Math.Round(BeinLand.LandY, 1) * 40f - (0.1f * 40f);
                //float EndX = (float)Math.Round(EndLand.LandX, 1) * 40f + (0.05f * 40f);
                //float EndY = (float)Math.Round(EndLand.LandY, 1) * 40f - (0.1f * 40f);
                //if (Segment.SegmentType == 0)//直线
                //{
                //    TopoShape RouteShap = shapeMaker.MakeRoad(new Vector3(BeginX, BeginY, 0), new Vector3(EndX, EndY, 0), 2, GroudHeight + 0.2, 0.2, false);
                //    FaceStyle RouteStyle = new FaceStyle();
                //    RouteStyle.SetColor(new ColorValue(2.05f, 1.73f, 0f, 1));
                //    SceneNode sceneNodeRouteShap = ShowTopoShape(RouteShap);
                //    sceneNodeRouteShap.SetFaceStyle(RouteStyle);
                //}




                //IList<CarInfo> CarShaps = AGVDAccess.AGVSimulationDAccess.LoadCarShap();
                //if (CarShaps != null && CarShaps.Count > 0)
                //{
                //    foreach (CarInfo car in CarShaps)
                //    {
                //    }
                //}



                //testCarShap
                double CarLenth = 25;
                double CarWidth = 10;
                double CarHeight = 4;
                PointF CarPoint = new PointF(80, -300);
                TopoShape FrmackBoxShap = shapeMaker.MakeBox(new Vector3(CarPoint.X, CarPoint.Y, 0), new Vector3(CarPoint.X + (float)CarLenth, CarPoint.Y, 0), CarWidth, CarHeight);
                FaceStyle FrmackBoxStyle = new FaceStyle();
                FrmackBoxStyle.SetColor(new ColorValue(0.3f, 1.44f, 2.55f, 1));
                SceneNode sceneFrmackBox = ShowTopoShape(FrmackBoxShap);
                sceneFrmackBox.SetFaceStyle(FrmackBoxStyle);


                TopoShape TrayBoxShap = shapeMaker.MakeBox(new Vector3(CarPoint.X + 1, CarPoint.Y, (float)CarHeight - 1), new Vector3(CarPoint.X + (float)CarLenth - 1, CarPoint.Y, (float)CarHeight - 1), CarWidth - 0.5, 1.2);
                FaceStyle TrayBoxStyle = new FaceStyle();
                TrayBoxStyle.SetColor(new ColorValue(1.05f, 1.05f, 1.05f, 1));
                SceneNode sceneTrayBox = ShowTopoShape(TrayBoxShap);
                sceneTrayBox.SetFaceStyle(TrayBoxStyle);

                TopoShape wheelShap1 = shapeMaker.MakeCone(new Vector3(CarPoint.X + (float)CarLenth / 4f, CarPoint.Y - (float)CarWidth / 2f - 0.1f, (float)CarHeight / 3), new Vector3(0, 1, 0), (float)CarHeight / 4 * 2, 0.5, 0, 360);
                FaceStyle wheelStyle = new FaceStyle();
                wheelStyle.SetColor(new ColorValue(0f, 0f,0f, 1));
                SceneNode scenewheel = ShowTopoShape(wheelShap1);
                scenewheel.SetFaceStyle(wheelStyle);


                TopoShape wheelShap2 = shapeMaker.MakeCone(new Vector3(CarPoint.X + (float)CarLenth / 4f * 3, CarPoint.Y - (float)CarWidth / 2f - 0.1f, (float)CarHeight / 3), new Vector3(0, 1, 0), (float)CarHeight / 4 * 2, 0.5, 0, 360);
                FaceStyle wheelStyle2 = new FaceStyle();
                wheelStyle2.SetColor(new ColorValue(2.55f, 0.48f, 0.48f, 1));
                SceneNode scenewhee2 = ShowTopoShape(wheelShap2);
                scenewhee2.SetFaceStyle(wheelStyle2);

                TopoShape wheelShap3 = shapeMaker.MakeCone(new Vector3(CarPoint.X + (float)CarLenth / 4f, CarPoint.Y + (float)CarWidth / 2f + 0.1f, (float)CarHeight / 3), new Vector3(0, -1, 0), (float)CarHeight / 4 * 2, 0.5, 0, 360);
                FaceStyle wheelStyle3 = new FaceStyle();
                wheelStyle3.SetColor(new ColorValue(2.55f, 0.48f, 0.48f, 1));
                SceneNode scenewheel3 = ShowTopoShape(wheelShap3);
                scenewheel3.SetFaceStyle(wheelStyle3);

                TopoShape wheelShap4 = shapeMaker.MakeCone(new Vector3(CarPoint.X + (float)CarLenth / 4f * 3, CarPoint.Y + (float)CarWidth / 2f + 0.1f, (float)CarHeight / 3), new Vector3(0, -1, 0), (float)CarHeight / 4 * 2, 0.5, 0, 360);
                FaceStyle wheelStyle4 = new FaceStyle();
                wheelStyle4.SetColor(new ColorValue(2.55f, 0.48f, 0.48f, 1));
                SceneNode scenewheel4 = ShowTopoShape(wheelShap4);
                scenewheel4.SetFaceStyle(wheelStyle4);

            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region 贝塞尔曲线
        /// <summary>  
        /// 绘制n阶贝塞尔曲线路径  
        /// </summary>  
        /// <param name="points">输入点</param>  
        /// <param name="count">点数(n+1)</param>  
        /// <param name="step">步长,步长越小，轨迹点越密集</param>  
        /// <returns></returns>  
        public static PointF[] draw_bezier_curves(PointF[] points, int count, float step)
        {
            List<PointF> bezier_curves_points = new List<PointF>();
            float t = 0F;
            do
            {
                PointF temp_point = bezier_interpolation_func(t, points, count);    // 计算插值点  
                t += step;
                bezier_curves_points.Add(temp_point);
            }
            while (t <= 1 && count > 1);    // 一个点的情况直接跳出.  
            return bezier_curves_points.ToArray();  // 曲线轨迹上的所有坐标点  
        }


        /// <summary>  
        /// n阶贝塞尔曲线插值计算函数  
        /// 根据起点，n个控制点，终点 计算贝塞尔曲线插值  
        /// </summary>  
        /// <param name="t">当前插值位置0~1 ，0为起点，1为终点</param>  
        /// <param name="points">起点，n-1个控制点，终点</param>  
        /// <param name="count">n+1个点</param>  
        /// <returns></returns>  
        private static PointF bezier_interpolation_func(float t, PointF[] points, int count)
        {
            PointF PointF = new PointF();
            float[] part = new float[count];
            float sum_x = 0, sum_y = 0;
            for (int i = 0; i < count; i++)
            {
                ulong tmp;
                int n_order = count - 1;    // 阶数  
                tmp = calc_combination_number(n_order, i);
                sum_x += (float)(tmp * points[i].X * Math.Pow((1 - t), n_order - i) * Math.Pow(t, i));
                sum_y += (float)(tmp * points[i].Y * Math.Pow((1 - t), n_order - i) * Math.Pow(t, i));
            }
            PointF.X = sum_x;
            PointF.Y = sum_y;
            return PointF;
        }

        /// <summary>  
        /// 计算组合数公式  
        /// </summary>  
        /// <param name="n"></param>  
        /// <param name="k"></param>  
        /// <returns></returns>  
        private static ulong calc_combination_number(int n, int k)
        {
            ulong[] result = new ulong[n + 1];
            for (int i = 1; i <= n; i++)
            {
                result[i] = 1;
                for (int j = i - 1; j >= 1; j--)
                    result[j] += result[j - 1];
                result[0] = 1;
            }
            return result[k];
        }
        #endregion

        private void timerMove_Tick(object sender, EventArgs e)
        {
            try
            {
                //SceneManager Scemange = theView.GetSceneManager();
                //SceneNode ss = Scemange.FindNode(ShapeID - 1);
                //Scemange.RemoveNode(ss);
                //Mi
                //ss.SetTransform()
                //ShapeID -= 1;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }




        #region 内存回收
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        #endregion



        #region 模拟运行
        System.Windows.Forms.Timer FreshDate_Timer = new System.Windows.Forms.Timer();
        Simulator simula = null;
        bool IsRunninSimula = false;
        IDictionary<string, UnitPoint[]> BezierPoints = new Dictionary<string, UnitPoint[]>(); //存储贝塞尔曲线上的点集
        private void IniMoni()
        {
            try
            {
                BezierPoints.Clear();
                simula = new Simulator();
                FreshDate_Timer.Enabled = true;
                FreshDate_Timer.Interval = 2000;
                FreshDate_Timer.Tick += FreshDate_Timer_Tick;
                simula.Car_Move += Simula_Car_Move;
                simula.Car_Ini += Simula_Car_Ini;
                simula.Inital();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        double ScalingRate = 0;
        private void Simula_Car_Ini(object obj)
        {
            try
            {
                //if (!this.IsHandleCreated) { return; }
                //this.Invoke((EventHandler)(delegate
                //{

                //    SysParameter sys = AGVClientDAccess.GetParameterByCode("ScalingRate");
                //    if (sys != null)
                //    {
                //        try
                //        {
                //            ScalingRate = Convert.ToDouble(sys.ParameterValue);
                //        }
                //        catch
                //        {
                //            return;
                //        }
                //    }

                //    if (ScalingRate <= 0)
                //    {
                //        return;
                //    }
                    //SceneManager Scemange = theView.GetSceneManager();
                    //SceneNode ss = Scemange.FindNode(ShapeID - 1);
                    //Scemange.RemoveNode(ss);
                    //Mi
                    //ss.SetTransform()
                    //ShapeID -= 1;




                //    IEnumerable<IDrawObject> objects = from a in m_data.ActiveLayer.Objects
                //                                       where a.Id == "AGVTool"
                //                                       select a;
                //    m_data.DeleteObjects(objects);

                //    List<CarMonitor> allCars = obj as List<CarMonitor>;
                //    if (allCars != null)
                //    {
                //        foreach (CarMonitor car in allCars)
                //        {
                //            AGVTool agv = new AGVTool();
                //            agv.Agv_id = car.AgvID.ToString();
                //            agv.Position = new UnitPoint(car.X / ScalingRate, car.Y / ScalingRate);
                //            m_data.AddObject(m_data.ActiveLayer, agv);
                //            m_canvas.DoInvalidate(true);
                //        }
                //    }
                //}));
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void Simula_Car_Move(object obj)
        {
            try
            {
                //    if (obj == null) { return; }
                //    if (!this.m_canvas.IsHandleCreated) { return; }
                //    this.m_canvas.Invoke((EventHandler)(delegate
                //    {
                //        if (ScalingRate <= 0)
                //        {
                //            return;
                //        }

                //        CarMonitor car = obj as CarMonitor;
                //        IEnumerable<IDrawObject> objects = from a in m_data.ActiveLayer.Objects
                //                                           where a.Id == "AGVTool" && (a as AGVTool).Agv_id == car.AgvID.ToString()
                //                                           select a;
                //        if (objects.Count() <= 0)
                //        {
                //            AGVTool agv = new AGVTool();
                //            agv.Agv_id = car.AgvID.ToString();
                //            agv.Position = new UnitPoint(car.X / ScalingRate, car.Y / ScalingRate);
                //            m_data.AddObject(m_data.ActiveLayer, agv);
                //            FreshCanvas();
                //        }
                //        else
                //        {
                //            AGVTool agv = objects.FirstOrDefault() as AGVTool;
                //            if (agv != null)
                //            {
                //                if (car.Rundistance != 0)
                //                {
                //                    AllSegment segment = SimulatorVar.AllSegs.FirstOrDefault(p => p.BeginLandMakCode == car.CurrLand.LandmarkCode && p.EndLandMarkCode == car.NextLand.LandmarkCode);
                //                    if (segment != null && segment.SegmentType == 0)//直线
                //                    {
                //                        double runrate = car.Rundistance / segment.Length;//当前线段行走百分比
                //                        car.X = (float)(car.CurrLand.LandX + (car.NextLand.LandX - car.CurrLand.LandX) * runrate);
                //                        car.Y = (float)(car.CurrLand.LandY + (car.NextLand.LandY - car.CurrLand.LandY) * runrate);
                //                        agv.Position = new UnitPoint(car.X / ScalingRate, car.Y / ScalingRate);
                //                    }
                //                    else if (segment != null && segment.SegmentType == 1)
                //                    {
                //                        double runrate = car.Rundistance / segment.Length;//当前线段行走百分比
                //                        if (!BezierPoints.Keys.Contains(car.CurrLand + "|" + car.CurrLand))
                //                        {
                //                            IDrawObject LandDraw1 = m_data.ActiveLayer.Objects.Where(p => p.Id == "LandMark" && (p as LandMarkTool).LandCode == car.CurrLand.LandmarkCode).FirstOrDefault();
                //                            IDrawObject LandDraw2 = m_data.ActiveLayer.Objects.Where(p => p.Id == "LandMark" && (p as LandMarkTool).LandCode == car.NextLand.LandmarkCode).FirstOrDefault();
                //                            IList<IDrawObject> Draws = m_data.ActiveLayer.Objects.Where(p => p.Id == "BezierTool" && (p as BezierTool).Type == BezierType.PointBezier).ToList();
                //                            IDrawObject CurrDraw = Draws.Where(q => Math.Abs(Math.Round((q as BezierTool).P1.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02
                //                             && Math.Abs(Math.Round((q as BezierTool).P1.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02 &&
                //                             Math.Abs(Math.Round((q as BezierTool).P2.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 &&
                //                             Math.Abs(Math.Round((q as BezierTool).P2.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02).FirstOrDefault();
                //                            BezierTool nextSeg = CurrDraw as BezierTool;
                //                            if (nextSeg == null)
                //                            { return; }
                //                            UnitPoint[] pointList = new UnitPoint[] { nextSeg.P1, nextSeg.P4, nextSeg.P3, nextSeg.P2 };
                //                            UnitPoint[] unitlist = draw_bezier_curves(pointList, pointList.Length, 0.001F); // 在起点和终点之间
                //                            BezierPoints[car.CurrLand.LandmarkCode + "|" + car.NextLand.LandmarkCode] = unitlist;
                //                        }
                //                        UnitPoint[] aa = BezierPoints[car.CurrLand.LandmarkCode + "|" + car.NextLand.LandmarkCode];
                //                        int currindex = (int)Math.Floor(aa.Length * runrate);
                //                        if (currindex >= aa.Length - 2)
                //                        {
                //                            currindex = aa.Length - 2;
                //                        }
                //                        car.X = (float)(aa[currindex].X) - 0.1f;
                //                        car.Y = (float)(aa[currindex].Y) + 0.1f;
                //                        agv.Position = new UnitPoint(car.X / ScalingRate, car.Y / ScalingRate);
                //                    }
                //                }

                //                //lblInflection.Caption = "";
                //                //if (car.CurrLand.sway == SwayEnum.Left)
                //                //{ lblInflection.Caption = "左转"; }
                //                //else if (car.CurrLand.sway == SwayEnum.Right)
                //                //{ lblInflection.Caption = "右转"; }
                //                //else if (car.CurrLand.sway == SwayEnum.ExcuteLand)
                //                //{ lblInflection.Caption = "强制地标"; }
                //                //if (car.CurrLand.sway != SwayEnum.None)
                //                //{ lblInflection.Caption += car.CurrLand.Angle.ToString(); }
                //                FreshCanvas();
                //            }
                //        }
                //    }));
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private async void FreshCanvas()
        {
            try
            {
                //await Task.Factory.StartNew(() =>
                //{
                //    //Thread.Sleep(1);
                //    m_canvas.DoInvalidate(true);
                //});
            }
            catch
            { }
        }

        private void FreshDate_Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                //new Thread(new ThreadStart(UpdateStock)) { IsBackground = true }.Start();
                //ClearMemory();
            }
            catch (Exception ex)
            { }
        }
        #endregion

    }
}
