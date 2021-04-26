using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Primitives
{
    public class MultiPolygon : GeometryCollection
    {
        public override string PrimitiveType
        {
            get
            {
                return GeometryTypes.MULTIPOLYGON;
            }
        }

        public override void Convert(IGeometryConverter converter)
        {
            var first = true;
            foreach (var polygon in Children)
            {
                if (!first)
                {
                    converter.NewPart();
                }

                polygon.Convert(converter);
                first = false;
            }
        }
    }
}
