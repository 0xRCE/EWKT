using EWKT.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWKT.Parsers.Geometry.Builders
{
    public class PointZBuilder : GeometryBuilder
    {
        public override IGeometry Build()
        {
            var coord = ParseCoordinates(Primitive.Coordinates.First()).First();

            return new PointZ(coord);
        }
    }
}
