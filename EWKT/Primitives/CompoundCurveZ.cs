using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Primitives
{
    public class CompoundCurveZ : BaseGeometry
    {
        public override string PrimitiveType
        {
            get
            {
                return GeometryTypes.COMPOUNDCURVEZ;
            }
        }

        public override void Convert(IGeometryConverter converter)
        {
            foreach (var child in Children)
            {
                child.Convert(converter);
            }
        }

        public override void Serialize(IGeometrySerializer writer)
        {
            writer.GeometryType(PrimitiveType);
            writer.OpenParenthesis();


            bool separator = false;

            foreach (var child in Children)
            {
                if (separator)
                {
                    writer.Seperator();
                }

                child.Serialize(writer);
                separator = true;
            }

            writer.ClosingParenthesis();
        }
    }
}
