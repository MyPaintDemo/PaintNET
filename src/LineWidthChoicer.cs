using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public delegate void OnLineWidthChange(int width);

    public partial class LineWidthChoicer : UserControl
    {
        public int LineWidth { get; set; }
        
        private int offset = 10;    //отступ образца линии от краёв панели для образца
        private int min = 1;
        private int max = 10;

        private DashStyle style = DashStyle.Solid;

        public event OnLineWidthChange LineWidthChanged;

        public LineWidthChoicer()
        {
            InitializeComponent();

            this.trackBar1.Minimum = this.min;
            this.trackBar1.Maximum = this.max;

            this.panel1.Paint += panel1_Paint;
            this.panel1.SizeChanged += panel1_SizeChanged;
            this.Load += LineWidthChoicer_Load;
        }

        public LineWidthChoicer(int min, int max)
            : this()
        {
            if (min <= 0 || min >= max)
            {
                return;
            }

            this.min = min;
            this.max = max;

            this.trackBar1.Minimum = min;
            this.trackBar1.Maximum = max;
        }

        public void SetLineWidth(int width)
        {
            if (this.min <= width && width <= this.max)
            {
                this.LineWidth = width;
                this.trackBar1.Value = width;
                this.panel1.Invalidate();
            }
        }

        public void SetLineStyle(DashStyle ds)
        {
            this.style = ds;
            this.panel1.Invalidate();
        }

        void LineWidthChoicer_Load(object sender, EventArgs e)
        {
            this.LineWidth = this.trackBar1.Value;
        }

        void panel1_SizeChanged(object sender, EventArgs e)
        {
            this.panel1.Invalidate();
        }

        void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gPanel = e.Graphics;
            Pen pen = new Pen(Color.Black, this.LineWidth);
            pen.DashStyle = this.style;
            gPanel.DrawLine(pen, new Point(this.offset, this.panel1.Height / 2), new Point(this.panel1.Width - this.offset, this.panel1.Height / 2));
            gPanel.Dispose();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (this.LineWidthChanged != null)
            {
                LineWidthChanged(this.LineWidth);
            }
            this.LineWidth = ((TrackBar)sender).Value;
            this.panel1.Invalidate();
            this.Invalidate();
        }
    }
}