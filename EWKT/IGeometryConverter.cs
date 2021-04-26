using EWKT.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT
{
    public interface IGeometryConverter
    {
        void NewPart();
        void AddSegmentPoints(IEnumerable<CoordinateModel> coordinates);
        void AddSegmentArc(IEnumerable<CoordinateModel> coordinates);
        void NewInteriorRing();
        void Commit();
    }
}
