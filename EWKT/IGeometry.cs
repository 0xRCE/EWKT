using EWKT.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EWKT
{
    [JsonConverter(typeof(JsonGeometryConverter))]
    public interface IGeometry
    {
        string PrimitiveType { get; }
        IGeometry Parent { get; set; }
        void AddChild(IGeometry geometry);
        IEnumerable<IGeometry> Children { get; }
        double? GetZValue();
        BoundingBox GetBounds();
        void Convert(IGeometryConverter converter);
        void Serialize(IGeometrySerializer writer);
    }
}
