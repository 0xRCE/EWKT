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
        private GeometryGraphic graphicCurrentPart;
        private List<PointF> pointsCurrentPart;
        private List<Tuple<GeometryGraphic, IEnumerable<PointF>>> intermediateResult;

        private GeometryToGraphicsConverter()
        {
            intermediateResult = new List<Tuple<GeometryGraphic, IEnumerable<PointF>>>();
            //points = new List<PointF>();
            //graphic = new GeometryGraphic();
            //NewInteriorRing();
            NewPart();
        }

        public IEnumerable<GeometryData> Result { get; private set; }

        public static IEnumerable<GeometryData> Convert(IGeometry geometry)
        {
            var converter = new GeometryToGraphicsConverter();
            geometry?.Convert(converter);
            converter.Commit();

            return converter.Result;
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
            pointsCurrentPart.AddRange(coordinates.Select(ConvertCoordinate));
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
            pointsCurrentPart.AddRange(convertedPoints);
        }

        private PointF ConvertCoordinate(CoordinateModel coordinate)
        {
            return new PointF((float)coordinate.X, (float)coordinate.Y);
        }

        public void Commit()
        {
            var result = new List<GeometryData>();
            foreach (var interResult in intermediateResult)
            {
                var data = new GeometryData();
                data.Points = new List<PointF>(interResult.Item2);
                data.GraphicPath = new GeometryGraphic(interResult.Item1);

                result.Add(data);
            }

            Result = result;
        }

        public void NewPart()
        {
            pointsCurrentPart = new List<PointF>();
            graphicCurrentPart = new GeometryGraphic();
            intermediateResult.Add(new Tuple<GeometryGraphic, IEnumerable<PointF>>(graphicCurrentPart, pointsCurrentPart));
            NewInteriorRing();
        }

        public void NewInteriorRing()
        {
            currentPath = new GraphicsPath();
            //graphic.Add(currentPath);
            graphicCurrentPart.Add(currentPath);
        }

        public void Dispose()
        {
            foreach (var result in intermediateResult)
            {
                var graphic = result.Item1;
                foreach (var path in graphic)
                {
                    path.Dispose();
                }
            }
        }
    }
}
