using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Visualizer.Controls.Visualizer.Painters
{
    public class GeometryPainter : IPaintable
    {
        private readonly GraphicsPainter painter;

        public GeometryPainter(GraphicsPainter painter)
        {
            this.painter = painter;
        }

        public bool ShowLabels { get; set; }

        public IEnumerable<GeometryData> Geometry { get; set; }

        public void Paint()
        {
            foreach (var geometry in Geometry)
            {
                geometry.GraphicPath.Paint(painter);
            }
        }
    }
}
