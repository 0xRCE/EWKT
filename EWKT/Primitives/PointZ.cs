using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Primitives
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/sql/relational-databases/spatial/point
    /// </summary>
    public class PointZ : BaseGeometry
    {
        public PointZ(double x, double y, double z)
            : this(new CoordinateModel { X = x, Y = y, Z = z })
        {
        }

        public PointZ(CoordinateModel coord)
        {
            Coordinate = coord;
        }

        public override string PrimitiveType
        {
            get
            {
                return GeometryTypes.POINTZ;
            }
        }

        public CoordinateModel Coordinate { get; private set; }

        public override void Convert(IGeometryConverter converter)
        {
            converter.AddSegmentPoints(new[] { Coordinate });
        }

        public override void Serialize(IGeometrySerializer writer)
        {
            writer.GeometryType(PrimitiveType);
            writer.OpenParenthesis();
            writer.Coordinate(Coordinate);
            writer.ClosingParenthesis();
        }

        public override BoundingBox GetBounds()
        {
            var x = Coordinate.X;
            var y = Coordinate.Y;

            return new BoundingBox(x, y, x, y);
        }
    }
}
