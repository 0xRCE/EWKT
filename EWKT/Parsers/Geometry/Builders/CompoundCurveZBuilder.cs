using EWKT.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Parsers.Geometry.Builders
{
    class CompoundCurveZBuilder : GeometryBuilder
    {
        public override IGeometry Build()
        {
            var compound = new CompoundCurveZ();

            foreach (var child in Primitive.Children)
            {
                var geom = BuildChild(child);
                compound.AddChild(geom);
            }

            return compound;
        }
    }
}
