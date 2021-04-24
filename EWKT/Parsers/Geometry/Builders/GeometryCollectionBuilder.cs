using EWKT.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Parsers.Geometry.Builders
{
    class GeometryCollectionBuilder : GeometryBuilder
    {
        protected virtual IGeometry CreateGeometryContainer()
        {
            var collection = new GeometryCollection();
            return collection;
        }

        public override IGeometry Build()
        {
            var container = CreateGeometryContainer();

            foreach (var child in Primitive.Children)
            {
                var geom = BuildChild(child);
                container.AddChild(geom);
            }

            return container;
        }
    }
}
