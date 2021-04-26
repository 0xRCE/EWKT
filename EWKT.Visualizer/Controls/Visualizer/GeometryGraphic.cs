using EWKT.Visualizer.Controls.Visualizer.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWKT.Visualizer.Controls.Visualizer
{
    public class GeometryGraphic : List<GraphicsPath>, IDisposable
    {
        private readonly Brush voidBrush = new HatchBrush(HatchStyle.Percent05, Color.Black, Color.White);
        public GeometryGraphic()
        {
            //intentionally left blank
        }

        public GeometryGraphic(GeometryGraphic graphic)
        {
            AddRange(graphic);
        }

        public void Paint(GraphicsPainter painter)
        {
            var outerRing = this.FirstOrDefault();
            if (outerRing != null)
            {
                Paint(painter, outerRing, Brushes.Pink);
            }

            foreach (var innerRing in this.Skip(1))
            {
                Paint(painter, innerRing, voidBrush);
            }
        }

        private static void Paint(GraphicsPainter painter, GraphicsPath path, Brush brush)
        {
            var isClosed = IsClosed(path.PathPoints[0], path.PathPoints[path.PointCount - 1]);
            painter.DrawPath(path);
            if (isClosed)
            {
                painter.FillPath(brush, path);
            }
        }

        private static bool IsClosed(PointF p1, PointF p2)
        {
            return Equals(p1.X, p2.X) && Equals(p1.Y, p2.Y);
        }

        private static bool Equals(float a, float b)
        {
            const float ypsilon = 0.00000000001f;
            return Math.Abs(a - b) <= ypsilon;
        }

        public void Dispose()
        {
            voidBrush.Dispose();
        }
    }
}
