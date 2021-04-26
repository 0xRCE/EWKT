using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EWKT.Visualizer.Controls.Visualizer.Painters
{
    internal class CoordinatePainter : IPaintable
    {
        private readonly GraphicsPainter painter;
        private readonly int width;
        private readonly int halfWidth;


        public CoordinatePainter(GraphicsPainter painter)
        {
            this.painter = painter;
            width = 6;
            halfWidth = width / 2;
        }

        public IEnumerable<GeometryData> Geometry { get; set; }

        public bool ShowLabels { get; set; }

        public void Paint()
        {
            foreach (var geometry in Geometry)
            {
                var points = geometry.Points;

                int index = 0;
                foreach (var point in points)
                {
                    PaintMarker(point);
                    if (ShowLabels && painter.View.PixelPerMeter > 0.005f)
                    {
                        PaintLabel(index++, point);
                    }
                }
            }
        }

        private void PaintLabel(int index, PointF point)
        {
            var labelText = string.Format("({2}) {0}, {1}", point.X, point.Y, index);
            var size = painter.MeasureString(labelText);
            var padding = 2;
            var offsetX = 10;
            var offsetY = 6;
            var outline = new Rectangle((int)point.X, (int)point.Y, (int)size.Width + padding, (int)size.Height + padding);

            if ((index % 2) == 0)
            {
                //make offset negative to remove overlap
                offsetY = offsetY * -1;
            }
            painter.DrawRectangleScreen(outline, Pens.Black, offsetX, offsetY);
            painter.FillRectangleScreen(Brushes.White, outline, offsetX, offsetY);

            var location = new PointF(outline.X, outline.Y);
            painter.DrawString(labelText, location, offsetX + 3, offsetY);
        }

        private void PaintMarker(PointF point)
        {
            var rectangleF = new RectangleF(point.X, point.Y, width, width);
            Rectangle rectangle = Rectangle.Round(rectangleF);
            painter.DrawRectangleScreen(rectangle, Pens.Black, halfWidth * -1, halfWidth * -1);
        }
    }
}
