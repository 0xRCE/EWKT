using EWKT.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Parsers.Geometry.Builders
{
    public class CurvedPolygonZBuilder : PolygonZBuilder
    {
        public override IGeometry Build()
        {
            var polygon = new CurvedPolygonZ();
            ProcessRingGeometry(polygon);
            return polygon;
        }
    }
}
