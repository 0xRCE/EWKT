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
            painter.DrawPath(path);
            painter.FillPath(brush, path);
        }

        public void Dispose()
        {
            voidBrush.Dispose();
        }
    }
}
