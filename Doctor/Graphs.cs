using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Doctor
{
    public partial class Graphs : Form
    {
        List<List<string>> data = new List<List<string>>();

        public Graphs(List<List<string>> data)
        {
            this.data = data;
            InitializeComponent();
        }

        private void Graphs_Load(object sender, EventArgs e)
        {
            //linechart.ChartAreas[0].CursorX.IsUserEnabled = true;
            linechart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            linechart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            linechart.Series[0].ChartType = SeriesChartType.Line;
        }

        private void Draw(int x, int y)
        {
            linechart.Series[0].Points.AddXY(x, y);
        }

        private double test(double x)
        {
            return (Math.Pow(x, 2) + 2*Math.Sin(2*x));
        }

        private void BPM_Click(object sender, EventArgs e)
        {
            linechart.Series[0].Points.Clear();
            double i = 0;
            foreach (List<string> array in data)
            {
                linechart.Series[0].Points.AddXY(i, array[Settings.Data.BPM]);
                i++;
            }
        }

        private void RPM_Click(object sender, EventArgs e)
        {
            linechart.Series[0].Points.Clear();
            double i = 0;
            foreach (List<string> array in data)
            {
                linechart.Series[0].Points.AddXY(i, array[Settings.Data.RPM]);
                i++;
            }
        }

        private void Speed_Click(object sender, EventArgs e)
        {
            linechart.Series[0].Points.Clear();
            double i = 0;
            foreach (List<string> array in data)
            {
                double speed;
                Double.TryParse(array[Settings.Data.SPEED], out speed);
                linechart.Series[0].Points.AddXY(i, speed);
                i++;
            }
        }

        private void Distance_Click(object sender, EventArgs e)
        {
            linechart.Series[0].Points.Clear();
            double i = 0;
            foreach (List<string> array in data)
            {
                double dist;
                Double.TryParse(array[Settings.Data.DISTANCE], out dist);
                linechart.Series[0].Points.AddXY(array[Settings.Data.INDEX], dist);
                i++;
            }
        }
    }
}
