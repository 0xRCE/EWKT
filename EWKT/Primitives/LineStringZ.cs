using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWKT.Primitives
{
    public class LineStringZ : BaseGeometry
    {
        private readonly bool explicitSerizalization;
        private readonly IEnumerable<CoordinateModel> coordinates;

        public LineStringZ(IEnumerable<CoordinateModel> coordinates)
            : this(coordinates, true)
        {
        }

        public LineStringZ(IEnumerable<CoordinateModel> coordinates, bool explicitSerizalization)
        {
            this.explicitSerizalization = explicitSerizalization;
            this.coordinates = coordinates;
        }

        public IEnumerable<CoordinateModel> Coordinates { get { return coordinates ?? Enumerable.Empty<CoordinateModel>(); } }

        public override string PrimitiveType
        {
            get
            {
                return GeometryTypes.LINESTRINGZ;
            }
        }

        public override void Convert(IGeometryConverter converter)
        {
            converter.AddSegmentPoints(Coordinates);
        }

        public override void Serialize(IGeometrySerializer writer)
        {
            if (explicitSerizalization)
            {
                writer.GeometryType(PrimitiveType);
            }

            writer.Coordinates(Coordinates);
        }

        public override double? GetZValue()
        {
            var c = coordinates.FirstOrDefault();
            if (c != null)
            {
                return c.Z;
            }

            return null;
        }

        public override BoundingBox GetBounds()
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;

            double maxX = double.MinValue;
            double maxY = double.MinValue;

            foreach (var point in Coordinates)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);

                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }

            return new BoundingBox(minX, minY, maxX, maxY);
        }
    }
}
