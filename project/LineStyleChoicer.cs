using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Paint
{
    public delegate void OnChangeLineStyle(DashStyle style);

    public partial class LineStyleChoicer : UserControl
    {
        private int offset = 5;
        private DashStyle[] lineStyleArr = {
            DashStyle.Solid,
            DashStyle.Dash,
            DashStyle.DashDot,
            DashStyle.DashDotDot,
            DashStyle.Dot
        };

        private int selected = 0;
        private int SelectedIndex
        {
            get
            {
                return this.selected;
            }
            set
            {
                if (0 <= value && value < this.lineStyleArr.Length)
                {
                    this.selected = value;
                }
                else
                {
                    this.selected = 0;
                }
            }
        }

        public event OnChangeLineStyle LineStyleChanged;

        public DashStyle SelectedStyle
        {
            get 
            {
                return this.lineStyleArr[this.selected];
            }
        }

        public LineStyleChoicer()
        {
            InitializeComponent();

            this.Paint += LineStyleChoiser_Paint;
            this.SizeChanged += LineStyleChoiser_SizeChanged;
            this.MouseClick += LineStyleChoiser_MouseClick;
        }

        public void SetLineStyle(DashStyle style)
        {
            for (int i = 0; i < this.lineStyleArr.Length; i++)
            {
                if(this.lineStyleArr[i] == style)
                {
                    this.selected = i;
                }
            }
            this.Invalidate();
        }

        void LineStyleChoiser_Paint(object sender, PaintEventArgs e)
        {
            int H = this.Height / this.lineStyleArr.Length;
            Graphics g = this.CreateGraphics();
            g.Clear(SystemColors.ButtonFace);

            Pen currentPen = new Pen(Color.Black, 3);
            Pen border = null;
            for (int i = 0; i < lineStyleArr.Length; i++)
            {
                if (i == this.selected)
                {
                    border = SystemPens.ActiveBorder;
                    g.FillRectangle(SystemBrushes.ButtonHighlight, 0, i * H, this.Width, H);
                }
                else
                {
                    border = SystemPens.InactiveBorder;
                }

                g.DrawRectangle(border, 0, i * H, this.Width, H);

                currentPen.DashStyle = lineStyleArr[i];
                g.DrawLine(currentPen, this.offset, (int)(H * (i + 0.5)), this.Width - this.offset, (int)(H * (i + 0.5)));
            }
        }

        void LineStyleChoiser_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        void LineStyleChoiser_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            int H = this.Height / this.lineStyleArr.Length;
            Rectangle R;
            for (int i = 0; i < this.lineStyleArr.Length; i++)
            {
                R = new Rectangle(0, i * H, this.Width, H);
                if (R.Contains(e.Location))
                {
                    this.selected = i;
                    if (LineStyleChanged != null)
                    {
                        LineStyleChanged(this.SelectedStyle);
                    }
                    break;
                }
            }
            this.Invalidate();
        }
    }
}