using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Primitives
{
    public class PolygonZ : BaseGeometry
    {
        private IGeometry exteriorRing;
        private readonly List<IGeometry> interiorRings = new List<IGeometry>();

        public override string PrimitiveType
        {
            get
            {
                return GeometryTypes.POLYGONZ;
            }
        }

        public void SetExteriorRing(IGeometry ring)
        {
            AddChild(ring);
            exteriorRing = ring;
        }

        public void AddInteriorRing(IGeometry ring)
        {
            AddChild(ring);
            interiorRings.Add(ring);
        }

        public override void Convert(IGeometryConverter converter)
        {
            exteriorRing.Convert(converter);
            foreach (var ring in interiorRings)
            {
                converter.NewInteriorRing();
                ring.Convert(converter);
            }
        }

        public override void Serialize(IGeometrySerializer writer)
        {
            writer.GeometryType(PrimitiveType);
            writer.OpenParenthesis();

            exteriorRing.Serialize(writer);

            foreach (var ring in interiorRings)
            {
                writer.Seperator();
                ring.Serialize(writer);
            }

            writer.ClosingParenthesis();
        }

        public override BoundingBox GetBounds()
        {
            if (exteriorRing != null)
            {
                return exteriorRing.GetBounds();
            }

            return null;
        }
    }
}
