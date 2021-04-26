using EWKT.Calculus;
using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EWKT.Calculus.CircleCalculator;

namespace EWKT.Visualizer.Controls.Visualizer
{
    class GeometryToGraphicsConverter : IGeometryConverter, IDisposable
    {
        private GraphicsPath currentPath;
        private readonly GeometryGraphic graphic;
        private readonly List<PointF> points;

        private GeometryToGraphicsConverter()
        {
            points = new List<PointF>();
            graphic = new GeometryGraphic();
            NewInteriorRing();
        }

        public GeometryData Result { get; private set; }

        public static GeometryData Convert(IGeometry geometry)
        {
            var converter = new GeometryToGraphicsConverter();
            converter.ConvertFrom(geometry).Commit();
            return converter.Result;
        }

        public IGeometryConverter AddPropertyInfo(string key, string value)
        {
            return this;
        }

        public void AddSegmentArc(IEnumerable<CoordinateModel> coordinates)
        {
            var arcPoints = coordinates.ToList();
            var start = arcPoints[0];
            var middle = arcPoints[1];
            var end = arcPoints[2];

            var calc = new CircleCalculator();
            var circle = calc.Center(start, middle, end);
            var box = DetermineBoundingBox(circle);


            var startAngle = calc.AngleToXAxis(circle, start);
            var endAngle = calc.AngleToXAxis(circle, end);

            if (circle.Direction == CircleDirection.CW)
            {
                var tmp = startAngle;
                startAngle = endAngle;
                endAngle = tmp;
            }

            double sweepAngle = endAngle - startAngle;
            sweepAngle = calc.NormalizeAngle(sweepAngle);

            currentPath.AddArc(box, (float)startAngle, (float)sweepAngle);
            points.AddRange(coordinates.Select(ConvertCoordinate));
        }

        private RectangleF DetermineBoundingBox(Circle point)
        {
            var box = new RectangleF();
            box.X = (float)(point.X - point.Radius);
            box.Y = (float)(point.Y - point.Radius);
            box.Width = (float)point.Radius * 2;
            box.Height = box.Width;

            return box;
        }

        public void AddSegmentPoints(IEnumerable<CoordinateModel> coordinates)
        {
            var convertedPoints = coordinates.Select(ConvertCoordinate).ToList();
            currentPath.AddLines(convertedPoints.ToArray());
            points.AddRange(convertedPoints);
        }

        private PointF ConvertCoordinate(CoordinateModel coordinate)
        {
            return new PointF((float)coordinate.X, (float)coordinate.Y);
        }

        public void Commit()
        {
            var result = new GeometryData();
            result.Points = new List<PointF>(points);
            result.GraphicPath = new GeometryGraphic(graphic);

            Result = result;
        }

        public IGeometryConverter ConvertFrom(IGeometry geometrie)
        {
            geometrie?.Convert(this);
            return this;
        }

        public void NewInteriorRing()
        {
            currentPath = new GraphicsPath();
            graphic.Add(currentPath);
        }

        public IGeometryConverter SetHoogte(double? value)
        {
            return this;
        }

        public void Dispose()
        {
            foreach (var path in graphic)
            {
                path.Dispose();
            }
        }
    }
}
