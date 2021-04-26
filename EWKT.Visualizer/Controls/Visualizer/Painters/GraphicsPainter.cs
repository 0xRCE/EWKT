using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace EWKT.Visualizer.Controls.Visualizer.Painters
{
    public class GraphicsPainter
    {
        public GraphicsPainter()
        {
            //intentionally left blank
        }

        public bool LabelsEnabled { get; set; }

        public Graphics Graphics { get; set; }

        public Font Font { get; set; }

        public Pen Pen { get; set; }

        public View View { get; set; }

        public Size Window { get; set; }

        public void DrawRectangle(Rectangle rectangle)
        {
            Graphics.DrawRectangle(Pen, rectangle);
        }

        public void DrawRectangleScreen(Rectangle rectangle)
        {
            var state = Graphics.Save();
            try
            {
                rectangle.Location = TransformPoint(rectangle.Location);
                ResetTransform();
                DrawRectangle(rectangle);
            }
            finally
            {
                Graphics.Restore(state);
            }
        }

        public void DrawRectangleScreen(Rectangle rectangle, Pen pen)
        {
            DrawRectangleScreen(rectangle, pen, 0, 0);
        }

        public void DrawRectangleScreen(Rectangle rectangle, Pen pen, int offsetX, int offsetY)
        {
            var state = Graphics.Save();
            try
            {
                rectangle.Location = TransformPoint(rectangle.Location);
                ResetTransform();
                rectangle.Location = new Point(rectangle.Location.X + offsetX, rectangle.Location.Y + offsetY);
                Graphics.DrawRectangle(pen, rectangle);
            }
            finally
            {
                Graphics.Restore(state);
            }
        }

        public void FillRectangle(Brush brush, Rectangle rectangle)
        {
            Graphics.FillRectangle(brush, rectangle);
        }

        public void FillRectangleScreen(Brush brush, Rectangle rectangle)
        {
            FillRectangleScreen(brush, rectangle, 0, 0);
        }

        public void FillRectangleScreen(Brush brush, Rectangle rectangle, int offsetX, int offsetY)
        {
            var state = Graphics.Save();
            try
            {
                rectangle.Location = TransformPoint(rectangle.Location);
                ResetTransform();
                rectangle.Location = new Point(rectangle.Location.X + offsetX, rectangle.Location.Y + offsetY);
                FillRectangle(brush, rectangle);
            }
            finally
            {
                Graphics.Restore(state);
            }
        }

        public void DrawLine(Point p1, Point p2)
        {
            Graphics.DrawLine(Pen, p1, p2);
        }

        public void DrawLineScreen(Point p1, Point p2)
        {
            var state = Graphics.Save();
            try
            {
                p1 = TransformPoint(p1);
                p2 = TransformPoint(p2);
                ResetTransform();
                DrawLine(p1, p2);
            }
            finally
            {
                Graphics.Restore(state);
            }
        }

        public void ResetTransform()
        {
            Graphics.ResetTransform();
        }

        public void TranslateTransform(float dx, float dy)
        {
            Graphics.TranslateTransform(dx, dy, System.Drawing.Drawing2D.MatrixOrder.Append);
        }

        public SizeF MeasureString(string text)
        {
            return Graphics.MeasureString(text, Font);
        }

        public void FillPath(Brush brush, GraphicsPath path)
        {
            Graphics.FillPath(brush, path);
        }

        public void DrawString(string text, PointF location)
        {
            DrawString(text, location, 0, 0);
        }

        public void DrawString(string text, PointF location, int offsetX, int offsetY)
        {
            var state = Graphics.Save();
            try
            {
                location = TransformPoint(location);
                Graphics.ResetTransform();
                location = new PointF(location.X + offsetX, location.Y + offsetY);
                Graphics.DrawString(text, Font, Brushes.Black, location);
            }
            finally
            {
                Graphics.Restore(state);
            }
        }

        private PointF TransformPoint(PointF point)
        {
            using (SaveState())
            {
                var transform = Graphics.Transform.Clone();
                Graphics.ResetTransform();
                var points = new PointF[] { point };
                //get the position from last transform before reset
                transform.TransformPoints(points);
                return points[0];
            }
        }

        private Point TransformPoint(Point point)
        {
            using (SaveState())
            {
                var transform = Graphics.Transform.Clone();
                Graphics.ResetTransform();
                var points = new Point[] { point };
                //get the position from last transform before reset
                transform.TransformPoints(points);
                return points[0];
            }
        }

        public void DrawPath(GraphicsPath path)
        {
            Graphics.DrawPath(Pen, path);
        }

        public IDisposable SaveState()
        {
            return new State(Graphics);
        }


        private class State : IDisposable
        {
            private readonly Graphics graphics;
            private readonly GraphicsState state;

            internal State(Graphics g)
            {
                graphics = g;
                state = graphics.Save();
            }

            public void Dispose()
            {
                graphics.Restore(state);
            }
        }
    }
}
