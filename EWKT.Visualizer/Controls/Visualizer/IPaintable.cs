using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWKT.Visualizer.Controls.Visualizer
{
    public interface IPaintable
    {
        IEnumerable<GeometryData> Geometry { get; set; }
        bool ShowLabels { get; set; }
        void Paint();
    }
}
