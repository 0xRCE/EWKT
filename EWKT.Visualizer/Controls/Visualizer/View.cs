using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWKT.Visualizer.Controls.Visualizer
{
    public class View
    {
        public View()
        {
        }

        public float Left { get; set; }
        public float Top { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public float PixelPerMeter { get; set; }
        public float MetersPerPixel { get; set; }


        /// <summary>
        /// Converteert tussen Client coordinaten (pixels in control) naar wereld coordinaten
        /// (meters Rijksdriehoekstelsel)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point ClientToWorld(Point point)
        {
            var x = Convert.ToInt32(Left + point.X * MetersPerPixel);
            var y = Convert.ToInt32(Top - point.Y * MetersPerPixel);
            return new Point(x, y);
        }
        /// <summary>
        /// Converteert tussen wereld coordinaten (meters Rijksdriehoekstelsel)
        /// en Client coordinaten (pixels in control) 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point WorldToClient(Point point)
        {
            var x = Convert.ToInt32((point.X - Left) * (1 / MetersPerPixel));
            var y = Convert.ToInt32((Top - point.Y) * (1 / MetersPerPixel));
            return new Point(x, y);
        }

        /// <summary>
        /// Converteert tussen Client coordinaten (pixels in control) naar wereld coordinaten
        /// (meters Rijksdriehoekstelsel)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PointF ClientToWorld(PointF point)
        {
            var x = Convert.ToInt32(Left + point.X * MetersPerPixel);
            var y = Convert.ToInt32(Top - point.Y * MetersPerPixel);
            return new PointF(x, y);
        }
        /// <summary>
        /// Converteert tussen wereld coordinaten (meters Rijksdriehoekstelsel)
        /// en Client coordinaten (pixels in control) 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PointF WorldToClient(PointF point)
        {
            var x = (point.X - Left) * (1 / MetersPerPixel);
            var y = (Top - point.Y) * (1 / MetersPerPixel);
            return new PointF(x, y);
        }
    }
}
