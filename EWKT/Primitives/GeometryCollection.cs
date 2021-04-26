using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Primitives
{
    public class GeometryCollection : BaseGeometry
    {
        public override string PrimitiveType
        {
            get
            {
                return GeometryTypes.GEOMETRYCOLLECTION;
            }
        }

        public override void Convert(IGeometryConverter converter)
        {
            foreach (var geometry in Children)
            {
                geometry.Convert(converter);
                //ConvertChildren(converter);
            }
        }

        public override void Serialize(IGeometrySerializer writer)
        {
            SerializeWithCommaSeparatedChildren(writer);
        }


    }
}
