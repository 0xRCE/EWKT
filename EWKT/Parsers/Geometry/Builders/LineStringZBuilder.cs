using EWKT.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWKT.Parsers.Geometry.Builders
{
    class LineStringZBuilder : GeometryBuilder
    {
        public override IGeometry Build()
        {
            var coordinates = ParseCoordinates(Primitive.Coordinates.First());
            var explicitDeserialization = DetermineExplicitDeserialization(Primitive);

            return new LineStringZ(coordinates, explicitDeserialization);
        }

        private bool DetermineExplicitDeserialization(PrimitiveNode primitive)
        {
            var parent = primitive.Parent;
            return parent == null;
        }
    }
}
