using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Serialization
{
    public class GeometryStreamwriter : IGeometrySerializer
    {
        private readonly Action<string> write;

        public GeometryStreamwriter(Action<string> writer)
        {
            write = writer;
        }

        public void Serialize(IGeometry geometry)
        {
            geometry.Serialize(this);
        }

        public void GeometryType(string type)
        {
            write(type);
            Space();
        }

        public void Space()
        {
            const string SPACE = " ";
            write(SPACE);
        }

        public void OpenParenthesis()
        {
            write("(");
        }

        public void ClosingParenthesis()
        {
            write(")");
        }

        public void Seperator()
        {
            write(",");
        }

        public void Coordinates(IEnumerable<CoordinateModel> coordinates)
        {
            var writeSeperator = false;
            OpenParenthesis();
            foreach (var coordinate in coordinates)
            {
                if (writeSeperator)
                {
                    Seperator();
                }

                Coordinate(coordinate);
                writeSeperator = true;
            }

            ClosingParenthesis();
        }

        public void Coordinate(CoordinateModel coordinate)
        {
            write(string.Format("{0} {1} {2}", DoubleToString(coordinate.X), DoubleToString(coordinate.Y), DoubleToString(coordinate.Z)));
        }

        private string DoubleToString(double value)
        {
            return value.ToString("0.#######", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}

