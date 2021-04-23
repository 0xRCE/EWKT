using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWKT
{
    public class BoundingBox
    {
        private CoordinateModel[] vertices;

        private BoundingBox()
        {
            //intentionally left blank
        }

        public BoundingBox(double minX, double minY, double maxX, double maxY)
            : this()
        {
            MinX = minX;
            MaxX = maxX;

            MinY = minY;
            MaxY = maxY;

            vertices = ToPolygon(this);
        }

        private CoordinateModel[] ToPolygon(BoundingBox box)
        {
            return new[]
            {
                new CoordinateModel { X = box.MinX, Y = box.MinY },
                new CoordinateModel { X = box.MaxX, Y = box.MinY },
                new CoordinateModel { X = box.MaxX, Y = box.MaxY },
                new CoordinateModel { X = box.MinX, Y = box.MaxY },
                new CoordinateModel { X = box.MinX, Y = box.MinY }
            };
        }

        public double MinX { get; set; }
        public double MaxX { get; set; }
        public double MinY { get; set; }
        public double MaxY { get; set; }

        public bool IntersectsWith(BoundingBox other)
        {
            var otherVertices = ToPolygon(other);
            return otherVertices.Any(Inside) || otherVertices.Any(OnEgde);
        }

        public bool OnEgde(CoordinateModel p)
        {
            for (int i = 0, j = 1; j < vertices.Length; i++, j++)
            {
                var v1 = vertices[i];
                var v2 = vertices[j];
                bool locatedOnEdge = OnEdge(p, v1, v2);

                if (locatedOnEdge)
                {
                    return true;
                }
            }

            return false;
        }

        public BoundingBox Merge(BoundingBox other)
        {
            if (other == null)
            {
                return this;
            }

            var bounds = new BoundingBox();
            bounds.MinX = Math.Min(MinX, other.MinX);
            bounds.MaxX = Math.Max(MaxX, other.MaxX);

            bounds.MinY = Math.Min(MinY, other.MinY);
            bounds.MaxY = Math.Max(MaxY, other.MaxY);

            bounds.vertices = ToPolygon(bounds);
            return bounds;
        }

        private bool OnEdge(CoordinateModel p, CoordinateModel v1, CoordinateModel v2)
        {
            var dy = v2.Y - v1.Y;

            if (Equals(dy, 0d)) //horizontal line (p.x must be within (including) x interval v1 and v2
            {
                var xMin = Math.Min(v1.X, v2.X);
                var xMax = Math.Max(v1.X, v2.X);
                return Equals(p.Y, v1.Y) && (xMin <= p.X && p.X <= xMax);
            }

            var dx = v2.X - v1.X;
            if (Equals(dx, 0d)) //vertical line (p.y must be within (including) y interval v1 and v2
            {
                var yMin = Math.Min(v1.Y, v2.Y);
                var yMax = Math.Max(v1.Y, v2.Y);
                return Equals(p.X, v1.X) && (yMin <= p.Y && p.Y <= yMax);
            }

            //fail-safe, should not occur, because the boundingbox is a rectangle

            //solve f(x) = ax + b for p
            var a = dy / dx;
            var y = a * (p.X - v1.X) + v1.Y;
            return Equals(y, p.Y);
        }

        private bool Equals(double a, double b)
        {
            const double ypsilon = 0.00000000001;
            return Math.Abs(a - b) <= ypsilon;
        }

        public bool Inside(CoordinateModel p)
        {
            //point is outside the rectangular representation of this instance
            if (p.X < MinX || p.X > MaxX || p.Y < MinY || p.Y > MaxY)
            {
                return false;
            }

            // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
            bool inside = false;
            for (int i = 0, j = vertices.Length - 1; i < vertices.Length; j = i++)
            {
                var vi = vertices[i];
                var vj = vertices[j];
                if ((vi.Y > p.Y) != (vj.Y > p.Y) &&
                     p.X < (vj.X - vi.X) * (p.Y - vi.Y) / (vj.Y - vi.Y) + vi.X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

    }
}
