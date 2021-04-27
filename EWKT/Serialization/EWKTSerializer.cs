using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Serialization
{
    public class EWKTSerializer
    {
        private EWKTSerializer()
        {     
        }

        public static string Serialize(IGeometry geometry)
        {
            var sb = new StringBuilder();
            var writer = new GeometryStreamwriter((value) => sb.Append(value));
            writer.Serialize(geometry);

            return sb.ToString();
        }
    }
}
