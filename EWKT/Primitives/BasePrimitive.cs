using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWKT.Primitives
{
    public abstract class BaseGeometry : IGeometry
    {
        protected List<IGeometry> children = new List<IGeometry>();

        public abstract string PrimitiveType { get; }

        public IGeometry Parent { get; set; }

        public IEnumerable<IGeometry> Children { get { return children; } }

        public void AddChild(IGeometry geometry)
        {
            geometry.Parent = this;
            children.Add(geometry);
        }

        public abstract void Convert(IGeometryConverter converter);

        public abstract void Serialize(IGeometrySerializer writer);

        public virtual double? GetZValue()
        {
            var zValue = children.Select(n => n.GetZValue()).FirstOrDefault(n => n.HasValue);
            if (zValue.HasValue)
            {
                return zValue.Value;
            }

            return null;
        }

        public virtual BoundingBox GetBounds()
        {
            BoundingBox bounds = null;
            foreach (var b in Children.Select(n => n.GetBounds()).Where(n => n != null))
            {
                bounds = b.Merge(bounds);
            }

            return bounds;
        }

        protected void SerializeWithCommaSeparatedChildren(IGeometrySerializer writer)
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

        //protected void ConvertChildren(IGeometryConverter converter)
        //{
        //    bool first = true;
        //    foreach (var geometry in Children)
        //    {
        //        if (!first)
        //        {
        //            converter.NewInteriorRing();
        //        }

        //        geometry.Convert(converter);
        //        first = false;
        //    }
        //}

    }
}
