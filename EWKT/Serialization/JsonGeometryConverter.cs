using EWKT.Parsers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Serialization
{
    public class JsonGeometryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IGeometry).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var ewkt = (reader.Value ?? string.Empty).ToString();
            return EWKTParser.Convert(ewkt);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //hack: this is needed to generate valid JSONregel
            //todo: check why
            writer.WriteRawValue("");

            IGeometry geometry = value as IGeometry;
            if (geometry != null)
            {
                writer.WriteRaw("\"");
                new GeometryStreamwriter(writer.WriteRaw).Serialize(geometry);
                writer.WriteRaw("\"");
            }
        }
    }
}
