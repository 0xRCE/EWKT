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
            var first = true;
            foreach (var geometry in Children)
            {
                if (!first)
                {
                    converter.NewPart();
                }

                geometry.Convert(converter);
                first = false;
            }
        }

        public override void Serialize(IGeometrySerializer writer)
        {
            SerializeWithCommaSeparatedChildren(writer);
        }


    }
}
