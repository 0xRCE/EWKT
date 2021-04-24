using EWKT.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWKT.Parsers.Geometry.Builders
{
    public class PolygonZBuilder : GeometryBuilder
    {
        public override IGeometry Build()
        {
            var polygon = new PolygonZ();
            ProcessRingGeometry(polygon);
            return polygon;
        }

        protected void ProcessRingGeometry(PolygonZ polygon)
        {
            var exteriorGeom = Primitive.Children.First();
            var exteriorRing = BuildChild(exteriorGeom);
            polygon.SetExteriorRing(exteriorRing);


            var interiorGeometries = Primitive.Children.Skip(1);
            foreach (var ringGeom in interiorGeometries)
            {
                var interiorRing = BuildChild(ringGeom);
                polygon.AddInteriorRing(interiorRing);
            }
        }

    }
}
