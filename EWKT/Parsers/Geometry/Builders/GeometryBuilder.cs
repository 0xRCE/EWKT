using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EWKT.Parsers.Geometry.Builders
{
    public abstract class GeometryBuilder
    {
        internal PrimitiveNode Primitive { get; set; }

        public abstract IGeometry Build();

        protected IGeometry BuildChild(PrimitiveNode child)
        {
            //todo: remove circular dependency to the factory (factory also creates builders) (remove static in class)
            var childGeometryBuilder = GeometryFactory.FromPrimitive(child);
            var childGeometry = childGeometryBuilder.Build();
            return childGeometry;
        }

        protected IEnumerable<CoordinateModel> ParseCoordinates(PrimitiveCoordinate coordinates)
        {
            var pairs = coordinates.Set.Split(',');
            foreach (var pair in pairs)
            {
                var points = pair.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var containsXY = points.Length >= 2;
                var containsZ = points.Length > 2;
                if (!containsXY)
                {
                    throw new NotSupportedException("Formaat niet ondesteunt voor coordinaten: " + pair);
                }

                var coordinaat = new CoordinateModel();
                coordinaat.X = ToDouble(points[0]);
                coordinaat.Y = ToDouble(points[1]);
                if (containsZ)
                {
                    coordinaat.Z = ToDouble(points[2]);
                }

                yield return coordinaat;
            }
        }

        private double ToDouble(string value)
        {
            double result;
            if (!double.TryParse(value,
                NumberStyles.Integer | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent,
                CultureInfo.InvariantCulture, out result))
            {
                throw new FormatException(string.Format("Coordinaat niet in ondersteunt formaat #.##; ({0})", value));
            }

            return result;
        }
    }
}