using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Paint
{
    public delegate void OnChangeHatchStyle(Object style);

    public partial class HatchChoicer : UserControl
    {
        private ArrayList hatchArr = new ArrayList {
            null,
            HatchStyle.BackwardDiagonal,
            HatchStyle.ForwardDiagonal,
            HatchStyle.Horizontal,
            HatchStyle.Vertical,
            HatchStyle.Cross,
            HatchStyle.DiagonalCross,
            HatchStyle.Sphere,
        };

        public event OnChangeHatchStyle HatchStyleChanged;

        private int offset = 6;

        private int selected = 0;

        /// <summary>
        /// Выбранный стиль HatchStyle, null в случае сплошной кисти
        /// </summary>
        public Object SelectedHatch
        {
            get
            {
                return this.hatchArr[this.selected];
            }
        }

        public HatchChoicer()
        {
            InitializeComponent();

            this.Paint += HatchChoiser_Paint;
            this.SizeChanged += HatchChoiser_SizeChanged;
            this.MouseClick += HatchChoiser_MouseClick;
        }

        public void SetHatchStyle(Brush brush)
        {
            if (brush is SolidBrush)
            {
                this.selected = 0;
            }
            else if (brush is HatchBrush)
            {
                for (int i = 1; i < this.hatchArr.Count; i++)
                {
                    if (((HatchBrush)brush).HatchStyle == (HatchStyle)this.hatchArr[i])
                    {
                        this.selected = i;
                    }
                }
            }
            this.Invalidate();
        }

        void HatchChoiser_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        void HatchChoiser_Paint(object sender, PaintEventArgs e)
        {
            int W = this.Width / 2;
            int H = this.Height / 4;

            //Прямоугольники для штрихофок

            Rectangle[] rectangles = new Rectangle[this.hatchArr.Count];
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    rectangles[row * 2 + col] = new Rectangle(col * W, row * H, W, H);
                }
            }

            //Отрисовка штриховок

            int index = 0;
            Graphics g = e.Graphics;

            foreach (Rectangle rect in rectangles)
            {
                //Фон активной штриховки

                if (index == this.selected)
                {
                    g.FillRectangle(SystemBrushes.ButtonHighlight, rect);
                }

                //Штриховка

                Brush foreBrush = null;

                if (index != 0)
                {
                    foreBrush = new HatchBrush((HatchStyle)this.hatchArr[index], Color.Black, SystemColors.ButtonHighlight);
                }
                else
                {
                    foreBrush = SystemBrushes.ButtonShadow;
                }

                Rectangle fillRectangle = new Rectangle(rect.X + this.offset, rect.Y + this.offset, rect.Width - 2 * this.offset, rect.Height - 2 * this.offset);
                g.FillRectangle(foreBrush, fillRectangle);

                //Рамка

                if (index == this.selected)
                {
                    g.DrawRectangle(SystemPens.ActiveBorder, rect);
                }
                else
                {
                    g.DrawRectangle(SystemPens.InactiveBorder, rect);
                }

                index++;
            }

            g.Dispose();
        }

        void HatchChoiser_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            int W = this.Width / 2;
            int H = this.Height / 4;

            Rectangle rect;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    rect = new Rectangle(col * W, row * H, W, H);
                    if (rect.Contains(e.Location))
                    {
                        selected = row * 2 + col;
                        if (HatchStyleChanged != null)
                        {
                            HatchStyleChanged(this.SelectedHatch);
                        }
                        this.Invalidate();
                        return;
                    }
                }
            }
        }
    }
}