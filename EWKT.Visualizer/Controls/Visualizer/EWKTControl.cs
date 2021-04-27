using EWKT.Visualizer.Controls.Visualizer.Painters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EWKT.Visualizer.Controls.Visualizer
{
    [DesignTimeVisible(false)]
    public partial class EWKTControl : UserControl
    {
        private IEnumerable<IGeometry> geometry;
        private List<GeometryData> geometryGraphics;
        private readonly float defaultMetersPerPixel = 1.0f;

        private View currentView;
        private readonly Graphics LayoutGraphics;
        private GraphicsPainter painter;

        private IList<IPaintable> layers = new List<IPaintable>();
        private Pen pen;
        private Font font;
        private SolidBrush brush;
        private Color defaultPenColor = Color.Black;
        private Point lastMousePos;
        private bool panning;

        public EWKTControl()
        {
            InitializeComponent();
            LayoutGraphics = Graphics.FromImage(new Bitmap(1, 1));
            geometryGraphics = new List<GeometryData>();
            painter = new GraphicsPainter();
            painter.Graphics = LayoutGraphics;

            pen = new Pen(defaultPenColor);
            font = new Font("Helvetica", 6);
            brush = new SolidBrush(defaultPenColor);
            lastMousePos = new Point();

            layers.Add(new GeometryPainter(painter));
            layers.Add(new CoordinatePainter(painter));
            layers.Add(new DebugPainter(painter));

            currentView = new View();

            currentView.MetersPerPixel = defaultMetersPerPixel;
            MetersPerPixel = defaultMetersPerPixel;

            MouseWheel += GeometryVisualizer_MouseWheel;
            MouseDown += GeometryVisualizer_MouseDown;
            MouseMove += GeometryVisualizer_MouseMove;
            MouseUp += GeometryVisualizer_MouseUp;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw, true);
        }

        public bool ShowLabels
        {
            get { return painter.LabelsEnabled; }
            set { painter.LabelsEnabled = value; }
        }

        private void GeometryVisualizer_MouseUp(object sender, MouseEventArgs e)
        {
            panning = false;
        }

        private void GeometryVisualizer_MouseMove(object sender, MouseEventArgs e)
        {
            if (panning)
            {
                currentView.Left = currentView.Left + (lastMousePos.X - e.X) * MetersPerPixel;
                currentView.Top = currentView.Top + (e.Y - lastMousePos.Y) * MetersPerPixel;

                Invalidate();
            }

            lastMousePos = e.Location;
        }

        private void GeometryVisualizer_MouseDown(object sender, MouseEventArgs e)
        {
            panning = true;
            lastMousePos = e.Location;
        }

        private void GeometryVisualizer_MouseWheel(object sender, MouseEventArgs e)
        {
            var point = new Point(lastMousePos.X, lastMousePos.Y - Height);
            if (e.Delta > 0)
            {
                ZoomInOnPoint(point);
            }
            else
            {
                ZoomOutOnPoint(point);
            }
        }




        public IEnumerable<IGeometry> Geometry
        {
            get { return geometry; }
            set
            {
                geometry = value;
                geometryGraphics.Clear();
                if (geometry != null)
                {
                    foreach (IGeometry geoItem in geometry)
                    {
                        var convertedGeometry = GeometryToGraphicsConverter.Convert(geoItem);
                        foreach (var geo in convertedGeometry)
                        {
                            geometryGraphics.Add(geo);
                        }
                    }
                    ZoomTo(geometryGraphics);
                }
                Invalidate();
            }
        }

        /// <summary>
        /// MetersPerPixel determines zoomLevel
        /// </summary>
        public float MetersPerPixel
        {
            get
            {
                return currentView.MetersPerPixel;
            }
            set
            {
                if (Math.Abs(currentView.MetersPerPixel - value) < 0.000001 ||
                    value < Constants.ZoomLowerBound ||
                    value > Constants.ZoomUpperBound)
                {
                    return;
                }

                float zoomFactor = value / currentView.MetersPerPixel;
                Zoom(zoomFactor);
                currentView.MetersPerPixel = value;

                UpdateCurrentView();
                Invalidate();
            }
        }

        private View UpdateCurrentView()
        {
            var view = currentView;
            view.PixelPerMeter = 1 / MetersPerPixel;
            view.Width = (int)(Width * MetersPerPixel);
            view.Height = (int)(Height * MetersPerPixel);

            return view;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Geometry != null)
            {
                try
                {
                    try
                    {
                        UpdatePainter(e.Graphics);
                        PaintLayers(layers);
                    }
                    finally
                    {
                        RestorePainter();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        private void RestorePainter()
        {
            painter.Graphics = LayoutGraphics;
        }

        private void UpdatePainter(Graphics g)
        {
            painter.Graphics = g;

            var view = UpdateCurrentView();

            //Change coordinates from screen to real world coordinates
            g.ResetTransform();
            g.TranslateTransform(-view.Left, -view.Top, System.Drawing.Drawing2D.MatrixOrder.Append);
            //flip y as
            g.ScaleTransform(view.PixelPerMeter, -view.PixelPerMeter, System.Drawing.Drawing2D.MatrixOrder.Append);

            //0,0 starts left bottom 
            g.TranslateTransform(0, Height, System.Drawing.Drawing2D.MatrixOrder.Append);

            //change with according to zoom
            pen.Width = MetersPerPixel;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            painter.View = currentView;
            painter.Window = new Size(Width, Height);
            painter.Font = font;
            painter.Pen = pen;
        }


        private void PaintLayers(IList<IPaintable> layers)
        {
            foreach (var layer in layers)
            {
                layer.ShowLabels = ShowLabels;
                layer.Geometry = geometryGraphics;
                layer.Paint();
            }
        }

        public void ZoomIn()
        {
            MetersPerPixel = MetersPerPixel / Constants.ZoomFactor;
        }

        private void ZoomTo(List<GeometryData> geometryGraphics)
        {
            if (geometryGraphics == null)
            {
                return;
            }

            var smallestX = float.MaxValue;
            var biggestX = 0.0f;
            var smallestY = float.MaxValue;
            var biggestY = 0.0f;
            foreach (GeometryData geometryGraphic in geometryGraphics)
            {
                var path = geometryGraphic.GraphicPath.FirstOrDefault();
                if (path != null)
                {
                    var bounds = path.GetBounds();
                    smallestX = System.Math.Min(bounds.Left, smallestX);
                    biggestX = System.Math.Max(bounds.Right, biggestX);
                    smallestY = System.Math.Min(bounds.Top, smallestY);
                    biggestY = System.Math.Max(bounds.Bottom, biggestY);
                }
            }
            var resolution = System.Math.Max((biggestX - smallestX) / Width, (biggestY - smallestY) / Height); // pixels per unit

            MetersPerPixel = resolution;
            currentView.Left = smallestX;
            currentView.Top = smallestY;
            UpdateCurrentView();
        }
        
        public void ZoomOut()
        {
            MetersPerPixel = MetersPerPixel * Constants.ZoomFactor;
        }
     
        private void Zoom(float factor)
        {
            float widthMeters = Width * MetersPerPixel;
            float heightMeters = Height * MetersPerPixel;
            float newWidth = widthMeters * factor;
            float newHeight = heightMeters * factor;
            currentView.Left -= (newWidth - widthMeters) / 2;
            currentView.Top += (newHeight - heightMeters) / 2;
            Invalidate();
        }

        public void ZoomInOnPoint(Point clientCoordinate)
        {
            var world = ClientToWorld(clientCoordinate);
            ZoomIn();

            var newWorld = ClientToWorld(clientCoordinate);
            currentView.Left = currentView.Left + world.X - newWorld.X;
            currentView.Top = currentView.Top + world.Y - newWorld.Y;
            Invalidate();
        }

        public void ZoomOutOnPoint(System.Drawing.Point clientCoordinate)
        {
            var world = ClientToWorld(clientCoordinate);
            ZoomOut();
            var newWorld = ClientToWorld(clientCoordinate);

            currentView.Left = currentView.Left + world.X - newWorld.X;
            currentView.Top = currentView.Top + world.Y - newWorld.Y;
            Invalidate();
        }

        public Point ClientToWorld(Point point)
        {
            return currentView.ClientToWorld(point);
        }
    }
}

