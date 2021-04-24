using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT
{
    public interface IGeometrySerializer
    {
        void GeometryType(string primitiveType);
        void Seperator();
        void OpenParenthesis();
        void ClosingParenthesis();
        void Coordinate(CoordinateModel coordinate);
        void Coordinates(IEnumerable<CoordinateModel> coordinates);
    }
}
