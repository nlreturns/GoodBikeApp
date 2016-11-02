namespace Doctor
{
    partial class Graphs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.BPM = new System.Windows.Forms.Button();
            this.RPM = new System.Windows.Forms.Button();
            this.Speed = new System.Windows.Forms.Button();
            this.Distance = new System.Windows.Forms.Button();
            this.linechart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.linechart)).BeginInit();
            this.SuspendLayout();
            // 
            // BPM
            // 
            this.BPM.Location = new System.Drawing.Point(835, 12);
            this.BPM.Name = "BPM";
            this.BPM.Size = new System.Drawing.Size(75, 23);
            this.BPM.TabIndex = 0;
            this.BPM.Text = "Hartslag";
            this.BPM.UseVisualStyleBackColor = true;
            this.BPM.Click += new System.EventHandler(this.BPM_Click);
            // 
            // RPM
            // 
            this.RPM.Location = new System.Drawing.Point(916, 12);
            this.RPM.Name = "RPM";
            this.RPM.Size = new System.Drawing.Size(92, 23);
            this.RPM.TabIndex = 1;
            this.RPM.Text = "Omwentelingen";
            this.RPM.UseVisualStyleBackColor = true;
            this.RPM.Click += new System.EventHandler(this.RPM_Click);
            // 
            // Speed
            // 
            this.Speed.Location = new System.Drawing.Point(835, 39);
            this.Speed.Name = "Speed";
            this.Speed.Size = new System.Drawing.Size(75, 23);
            this.Speed.TabIndex = 2;
            this.Speed.Text = "Snelheid";
            this.Speed.UseVisualStyleBackColor = true;
            this.Speed.Click += new System.EventHandler(this.Speed_Click);
            // 
            // Distance
            // 
            this.Distance.Location = new System.Drawing.Point(933, 39);
            this.Distance.Name = "Distance";
            this.Distance.Size = new System.Drawing.Size(75, 23);
            this.Distance.TabIndex = 3;
            this.Distance.Text = "Afstand";
            this.Distance.UseVisualStyleBackColor = true;
            this.Distance.Click += new System.EventHandler(this.Distance_Click);
            // 
            // linechart
            // 
            chartArea3.Name = "ChartArea1";
            this.linechart.ChartAreas.Add(chartArea3);
            this.linechart.Dock = System.Windows.Forms.DockStyle.Left;
            legend3.Name = "Legend1";
            this.linechart.Legends.Add(legend3);
            this.linechart.Location = new System.Drawing.Point(0, 0);
            this.linechart.Name = "linechart";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.linechart.Series.Add(series3);
            this.linechart.Size = new System.Drawing.Size(821, 586);
            this.linechart.TabIndex = 4;
            this.linechart.Text = "chart1";
            // 
            // Graphs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 586);
            this.Controls.Add(this.linechart);
            this.Controls.Add(this.Distance);
            this.Controls.Add(this.Speed);
            this.Controls.Add(this.RPM);
            this.Controls.Add(this.BPM);
            this.Name = "Graphs";
            this.Text = "Graphs";
            this.Load += new System.EventHandler(this.Graphs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.linechart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BPM;
        private System.Windows.Forms.Button RPM;
        private System.Windows.Forms.Button Speed;
        private System.Windows.Forms.Button Distance;
        private System.Windows.Forms.DataVisualization.Charting.Chart linechart;
    }
}