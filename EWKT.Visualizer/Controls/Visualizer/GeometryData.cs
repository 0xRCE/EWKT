using System.Collections.Generic;
using System.Drawing;

namespace EWKT.Visualizer.Controls.Visualizer
{
    public class GeometryData
    {
        public GeometryGraphic GraphicPath { get; internal set; }
        public IEnumerable<PointF> Points { get; internal set; }
    }
}