using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Paint
{
    public delegate void OnChangeColor(Color fore, Color back);
    public delegate void OnChangeForeColor(Color fClr);
    public delegate void OnChangeBackColor(Color bClr);

    class ColorChoicer : Control
    {
        private int offset = 4;                 

        private Color bColor;
        public Color BColor 
        {
            get
            {
                return this.bColor;
            }
            set
            {
                if (!value.IsEmpty)
                {
                    bColor = value;
                }
                else
                {
                    bColor = Color.Black;
                }
            }
        }

        private Color fColor;
        public Color FColor
        {
            get
            {
                return this.fColor;
            }
            set
            {
                if (!value.IsEmpty)
                {
                    fColor = value;
                }
                else
                {
                    fColor = Color.White;
                }
            }
        }

        private static Color[] arrClr = { 
            Color.White, 
            Color.Black, 
            Color.Red, 
            Color.Green, 
            Color.Blue, 
            Color.Yellow, 
            Color.Magenta, 
            Color.Coral, 
            Color.Cyan,
            Color.DeepSkyBlue,
            Color.Gray,
            Color.Indigo,
            Color.Lime,
            Color.Brown,
            Color.MistyRose,
            Color.Purple,
        };

        public event OnChangeColor ColorChanged;
        public event OnChangeForeColor FColorChanged;
        public event OnChangeBackColor BColorChanged;

        public ColorChoicer()
        {
            this.bColor = arrClr[0];
            this.fColor = arrClr[1];

            this.Paint += ColorChoicer_Paint;
            this.SizeChanged += ColorChoicer_SizeChanged;
            this.MouseClick += ColorChoicer_MouseClick;
        }

        public void SetParametersFromShape(IShape shape)
        {
            //Фигура

            if (shape is Shape)
            {
                Shape currentShape = shape as Shape;
                
                //Fore
                
                Pen shapePen = currentShape.Pen;
                this.FColor = shapePen.Color;
                
                //Back, если фигура заполнена

                if (currentShape.IsFill)
                {
                    Brush shapeBrush = currentShape.Brush;
                    if (shapeBrush is SolidBrush)
                    {
                        this.BColor = ((SolidBrush)shapeBrush).Color;
                    }
                    else
                    {
                        this.BColor = ((HatchBrush)shapeBrush).BackgroundColor;
                    }
                }
            }
            
            //Заливка
            
            else
            {
                MyFill currentFill = shape as MyFill;
                this.FColor = currentFill.FillColor;
            }

            this.Invalidate();
        }

        void ColorChoicer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(SystemColors.ButtonFace);

            int W = this.Width / 4;
            int H = this.Height / 6;

            //Рамка вокруг элемента

            this.DrawBorder(new Point(0, 0), new Point(this.Width, this.Height), false);

            //Цвет фона

            g.FillRectangle(new SolidBrush(this.bColor), (int)(1.5 * W), (int)(0.75 * H), 2 * W, H);   
            g.DrawRectangle(SystemPens.ButtonShadow, (int)(1.5 * W), (int)(0.75 * H), 2 * W, H);

            //Цвет кисти

            g.FillRectangle(new SolidBrush(this.fColor), (int)(0.5 * W), (int)(0.25 * H), 2 * W, H);
            g.DrawRectangle(SystemPens.ButtonShadow, (int)(0.5 * W), (int)(0.25 * H), 2 * W, H);
            
            //Палитра цветов
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    g.FillRectangle(new SolidBrush(arrClr[i * 4 + j]), i * W + this.offset, 2 * H + j * H + this.offset, W - 2 * this.offset, H - 2 * this.offset);
                    g.DrawRectangle(SystemPens.ButtonShadow, i * W + this.offset, 2 * H + j * H + this.offset, W - 2 * this.offset, H - 2 * this.offset);
                }
            }
        }

        void ColorChoicer_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        void ColorChoicer_MouseClick(object sender, MouseEventArgs e)
        {
            int W = this.Width / 4;
            int H = this.Height / 6;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rectangle R = new Rectangle(i * W + 2, 2 * H + j * H + 2, W - 4, H - 4);
                    if (R.Contains(e.Location))
                    {
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            this.fColor = arrClr[i * 4 + j];
                            this.Invalidate(new Rectangle((int)(0.5 * W), (int)(0.25 * H), 2 * W, H));
                        }
                        else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                        {
                            this.bColor = arrClr[i * 4 + j];
                            this.Invalidate(new Rectangle((int)(1.5 * W), (int)(0.75 * H), 2 * W, H));
                        }

                        //Генерируем события

                        if (this.ColorChanged != null) { this.ColorChanged(this.FColor, this.BColor); }
                        if (this.FColorChanged != null) { this.FColorChanged(this.FColor); }
                        if (this.BColorChanged != null) { this.BColorChanged(this.BColor); }

                        return;
                    }
                }
            }
        }

        void DrawBorder(Point p1, Point p2, bool isOut)
        {
            Graphics g = this.CreateGraphics();

            if (isOut)
            {
                g.DrawRectangle(SystemPens.ControlDarkDark, p1.X, p1.Y, p2.X - 1, p2.Y - 1);
                g.DrawLine(SystemPens.ButtonHighlight, p1, new Point(p2.X, p1.Y));
                g.DrawLine(SystemPens.ButtonHighlight, p1, new Point(p1.X, p2.Y));
                g.DrawLine(SystemPens.ButtonShadow, new Point(p1.X + 1, p2.Y - 2), new Point(p2.X - 2, p2.Y - 2));
                g.DrawLine(SystemPens.ButtonShadow, new Point(p2.X - 2, p1.Y + 1), new Point(p2.X - 2, p2.Y - 2));
            }
            else
            {
                g.DrawRectangle(SystemPens.ButtonShadow, p1.X, p1.Y, p2.X - 1, p2.Y - 1);
                g.DrawLine(SystemPens.ButtonHighlight, new Point(p1.X - 1, p2.Y - 1), new Point(p2.X - 1, p2.Y - 1));
                g.DrawLine(SystemPens.ButtonHighlight, new Point(p2.X - 1, p1.Y - 1), new Point(p2.X - 1, p2.Y - 1));
                g.DrawLine(SystemPens.ControlDarkDark, new Point(p1.X + 1, p1.Y + 1), new Point(p2.X - 2, p1.Y + 1));
                g.DrawLine(SystemPens.ControlDarkDark, new Point(p1.X + 1, p1.Y + 1), new Point(p1.X + 1, p2.Y - 2));
            }
        }
    }
}