using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWKT.Primitives
{
    public class CircularStringZ : LineStringZ
    {
        public CircularStringZ(IEnumerable<CoordinateModel> coordinates)
            : base(coordinates)
        {
        }

        public override string PrimitiveType
        {
            get
            {
                return GeometryTypes.CIRCULARSTRINGZ;
            }
        }

        public override void Convert(IGeometryConverter converter)
        {
            if (CanBeRepresentedAsLineSegment(Coordinates))
            {
                converter.AddSegmentPoints(Coordinates);
            }
            else
            {
                converter.AddSegmentArc(Coordinates);
            }

        }

        private bool CanBeRepresentedAsLineSegment(IEnumerable<CoordinateModel> coordinates)
        {
            var points = coordinates.ToArray();
            var start = points[0];
            var middle = points[1];
            var end = points[2];

            var x = (start.X + end.X) / 2;
            var y = (start.Y + end.Y) / 2;

            var deltaX = Math.Abs(middle.X - x);
            var deltaY = Math.Abs(middle.Y - y);

            var epsilon = 0.1d;

            return deltaX <= epsilon && deltaY <= epsilon;
        }

        public override void Serialize(IGeometrySerializer writer)
        {
            writer.GeometryType(PrimitiveType);
            writer.Coordinates(Coordinates);
        }
    }
}
