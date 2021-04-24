using EWKT.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Parsers.Geometry.Builders
{
    class MultiPolygonZBuilder : GeometryCollectionBuilder
    {
        protected override IGeometry CreateGeometryContainer()
        {
            return new MultiPolygon();
        }
    }
}
