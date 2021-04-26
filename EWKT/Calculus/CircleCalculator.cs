using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Calculus
{
    public class CircleCalculator
    {
        public CircleCalculator()
        {
            //intentionally left blank
        }

        /// <summary>
        /// http://www.qc.edu.hk/math/Advanced%20Level/circle%20given%203%20points.htm
        /// See method 3
        /// This function will choke when the three points are colinear and
        /// possibly when point p and r are the same. Try method 7 (the determinant method)
        /// to handle such cases better.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public Circle Center(CoordinateModel p, CoordinateModel q, CoordinateModel r)
        {
            // The formulas divide by (q.X - p.X), so this may not be 0.
            // We assume that we are not fed straight lines or full circles, so if p and q have
            // the same X coordinate, r and q cannot have the same X coordinate.
            // Note that there may still be loss of precision and maybe errors
            // if p.X, q.X and r.X are almost equal.
            double a, b;
            if (System.Math.Abs(p.X - q.X) > System.Math.Abs(r.X - q.X))
            {
                b = Y(p, q, r);
                a = X(p, q, b);
            }
            else
            {
                b = Y(r, q, p);
                a = X(r, q, b);
            }
            var radius = Radius(p, a, b);
            var direction = DetermineDirection(new CoordinateModel { X = a, Y = b }, p, q);
            return new Circle { X = a, Y = b, Radius = radius, Direction = direction };
        }

        private CircleDirection DetermineDirection(CoordinateModel center, CoordinateModel p, CoordinateModel q)
        {
            double sum = 0.0;
            var vertices = new[] { center, p, q, center };
            for (int i = 0; i < vertices.Length; i++)
            {
                var v1 = vertices[i];
                var v2 = vertices[(i + 1) % vertices.Length];
                sum += (v2.X - v1.X) * (v2.Y + v1.Y);
            }

            if (sum > 0.0)
            {
                return CircleDirection.CW;
            }

            return CircleDirection.CCW;
        }

        public double NormalizeAngle(double angle)
        {
            return ((angle + 360f) % 360f);
        }

        public double AngleToXAxis(Circle c, CoordinateModel p)
        {
            return NormalizeAngle(Angle(c.X, c.Y, p.X, p.Y));
        }

        private double Angle(double cx, double cy, double px, double py)
        {
            var dx = px - cx;
            var dy = py - cy;

            var radians = System.Math.Atan2(dy, dx);
            var angle = radians * (180 / System.Math.PI);
            return angle;
        }

        /// <summary>
        /// Use Pythagorean theorem (https://en.wikipedia.org/wiki/Pythagorean_theorem)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private double Radius(CoordinateModel p, double a, double b)
        {
            var dx2 = Square(p.X - a);
            var dy2 = Square(p.Y - b);

            var length = System.Math.Sqrt(dx2 + dy2);
            return length;
        }

        private double X(CoordinateModel p, CoordinateModel q, double b)
        {
            var alpha = Square(q.X) + Square(q.Y) - Square(p.X) - Square(p.Y);
            var beta = 2 * b * (q.Y - p.Y);
            var gamma = 2 * (q.X - p.X);

            var a = (alpha - beta) / gamma;
            return a;
        }

        private double Y(CoordinateModel p, CoordinateModel q, CoordinateModel r)
        {
            var alpha = Square(r.X) + Square(r.Y) - Square(q.X) - Square(q.Y);
            var beta = ((Square(q.X) + Square(q.Y) - Square(p.X) - Square(p.Y)) * (r.X - q.X)) / (q.X - p.X);
            var gamma = r.Y - q.Y;
            var omega = ((q.Y - p.Y) * (r.X - q.X)) / (q.X - p.X);

            var b = (alpha - beta) / (2 * (gamma - omega));
            return b;
        }

        private double Square(double val)
        {
            return val * val;
        }

        public class Circle
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Radius { get; set; }
            public CircleDirection Direction { get; set; }
        }

        public enum CircleDirection
        {
            CCW,
            CW
        }
    }
}
