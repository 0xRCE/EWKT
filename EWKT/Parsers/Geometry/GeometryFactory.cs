using EWKT.Parsers.Geometry.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Parsers.Geometry
{
    static class GeometryFactory
    {
        static IDictionary<string, Func<GeometryBuilder>> primitiveBuilderMapping = new Dictionary<string, Func<GeometryBuilder>>()
        {
            { GeometryTypes.POINT, () => new PointZBuilder() },
            { GeometryTypes.POINTZ, () => new PointZBuilder() },
            { GeometryTypes.LINESTRING, () => new LineStringZBuilder() },
            { GeometryTypes.LINESTRINGZ, () => new LineStringZBuilder() },
            { GeometryTypes.CIRCULARSTRING, () => new CircularStringZBuilder() },
            { GeometryTypes.CIRCULARSTRINGZ, () => new CircularStringZBuilder() },
            { GeometryTypes.POLYGON, () => new PolygonZBuilder() },
            { GeometryTypes.POLYGONZ, () => new PolygonZBuilder() },
            { GeometryTypes.CURVEPOLYGON, () => new CurvedPolygonZBuilder() },
            { GeometryTypes.CURVEPOLYGONZ, () => new CurvedPolygonZBuilder() },
            { GeometryTypes.COMPOUNDCURVE, () => new CompoundCurveZBuilder() },
            { GeometryTypes.COMPOUNDCURVEZ, () => new CompoundCurveZBuilder() },
            { GeometryTypes.GEOMETRYCOLLECTION, () => new GeometryCollectionBuilder() },
            { GeometryTypes.MULTIPOLYGON, () => new MultiPolygonZBuilder() }
    
            //todo: Implement remaining geometries
            //case "MULTIPOINT":
            //case "MULTILINESTRING":
            //case "CURVE":
            //case "MULTICURVE":
        };

        public static GeometryBuilder FromPrimitive(PrimitiveNode node)
        {
            var primitive = node.Name;
            Func<GeometryBuilder> newBuilder;
            if(primitiveBuilderMapping.TryGetValue(primitive.ToUpperInvariant().TrimEnd(), out newBuilder))
            {
                var builder = newBuilder();
                builder.Primitive = node;
                return builder;
            }

            throw new NotImplementedException(String.Format("Geometrytype '{0}' not implemented", primitive));
        }
    }
}
