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
    }
}
