using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Primitives
{
    /// <summary>
    /// A CurvePolygon is a topologically closed surface defined by
    /// an exterior bounding ring and zero or more interior rings
    /// 
    /// https://docs.microsoft.com/en-us/sql/relational-databases/spatial/curvepolygon
    /// 
    /// 1. Is an accepted LineString, CircularString, or CompoundCurve instance. 
    /// 2. Has at least four points.
    /// 3. The start and end point have the same X and Y coordinates.
    /// </summary>
    public class CurvedPolygonZ : PolygonZ
    {
        public override string PrimitiveType
        {
            get
            {
                return GeometryTypes.CURVEPOLYGONZ;
            }
        }
    }
}
