using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Visualizer.Controls.Visualizer.Painters
{
    class DebugPainter : IPaintable
    {
        private readonly GraphicsPainter painter;

        public DebugPainter(GraphicsPainter painter)
        {
            this.painter = painter;
        }

        public bool ShowLabels { get; set; }
        public IEnumerable<GeometryData> Geometry { get; set; }
        public void Paint()
        {
            using (painter.SaveState())
            {
                painter.Graphics.ResetTransform();
                var coord = string.Format("{0}, {1}", painter.View.Left, painter.View.Top);
                var size = painter.MeasureString(coord);

                painter.DrawString(coord, new System.Drawing.PointF(0, painter.Window.Height - size.Height));
            }
        }
    }
}
