using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EWKT.Visualizer.Controls.Visualizer
{
    public static class Constants
    {
        public static readonly float ZoomFactor = 1.4f;
        public static readonly Color LineColor = Color.Black;
        public static readonly Color FillColor = Color.GreenYellow;
        public static readonly System.Drawing.Drawing2D.DashStyle AreaDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        public static readonly float ZoomLowerBound = 0.02f;
        public static readonly float ZoomUpperBound = 300f;
    }
}
