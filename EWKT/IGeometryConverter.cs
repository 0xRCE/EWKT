using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT
{
    public interface IGeometryConverter
    {
        void Commit();
        IGeometryConverter ConvertFrom(IGeometry geometry);
        void AddSegmentPoints(IEnumerable<CoordinateModel> coordinates);
        void AddSegmentArc(IEnumerable<CoordinateModel> coordinates);
        void NewInteriorRing();
    }
}
