using EWKT.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWKT.Parsers.Geometry.Builders
{
    class CircularStringZBuilder : GeometryBuilder
    {
        public override IGeometry Build()
        {
            var coordinates = ParseCoordinates(Primitive.Coordinates.First());
            return new CircularStringZ(coordinates);
        }
    }
}
