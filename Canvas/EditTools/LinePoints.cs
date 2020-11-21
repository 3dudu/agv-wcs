using Canvas.DrawTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.EditTools
{
    public class LinePoints
    {
        #region 属性
        LineTool m_line;
        UnitPoint m_p1;
        UnitPoint m_p2;
        public UnitPoint MousePoint;

        public DrawTools.LineTool Line
        {
            get { return m_line; }
        }
        #endregion

        #region 方法函数
        public void SetLine(LineTool l)
        {
            try
            {
                m_line = l;
                m_p1 = l.P1;
                m_p2 = l.P2;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void ResetLine()
        {
            try
            {
                m_line.P1 = m_p1;
                m_line.P2 = m_p2;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 处理鼠标的点击的坐标是否靠近吸附点上
        /// </summary>
        public void SetNewPoints(LineTool l, UnitPoint hitpoint, UnitPoint intersectpoint)
        {
            try
            {
                SetLine(l);
                double hitToVp1 = HitUtil.Distance(hitpoint, l.P1);
                double ispToVp1 = HitUtil.Distance(intersectpoint, l.P1);
                if (hitToVp1 <= ispToVp1)
                { m_p2 = intersectpoint; }
                else
                { m_p1 = intersectpoint; }
                ResetLine();
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion


    }
}
