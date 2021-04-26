using EWKT.Parsers;
using EWKT.Visualizer.Controls.Visualizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EWKT.Visualizer
{
    public partial class MainForm : Form
    {
        private EWKTControl geometryVisualizer;

        public MainForm()
        {
            InitializeComponent();
            InitGeometryVisualizer();

            chkShowLabels_CheckedChanged(this, EventArgs.Empty);
            txtEWKT.Text = @"MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)),
((20 35, 10 30, 10 10, 30 5, 45 20, 20 35),
(30 20, 20 15, 20 25, 30 20)))";
            btnEWKT_Click(this, EventArgs.Empty);
        }

        private void InitGeometryVisualizer()
        {
            geometryVisualizer = new EWKTControl();
            geometryVisualizer.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(geometryVisualizer, 0, 2);
            tableLayoutPanel1.SetColumnSpan(geometryVisualizer, 2);
        }

        private void btnEWKT_Click(object sender, EventArgs e)
        {
            var ewkt = txtEWKT.Text;
            try
            {
                var geometry = EWKTParser.Convert(ewkt);
                geometryVisualizer.Geometry = new[] { geometry };
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message,  "Error");
            }
        }

        private void txtEWKT_KeyDown(object sender, KeyEventArgs e)
        {
            //fix: control+a does not work when multiline is enabled
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (sender != null)
                    ((TextBox)sender).SelectAll();
            }
        }

        private void chkShowLabels_CheckedChanged(object sender, EventArgs e)
        {
            geometryVisualizer.ShowLabels = chkShowLabels.Checked;
            Invalidate();
        }
    }
}
