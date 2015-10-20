using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Paint
{
    class ShapeException : ApplicationException
    {
        public ShapeException(string p):base(p){ }
    }

    interface IShape : ICloneable
    {
        void Draw(Graphics g, Pen p, Brush b);
        void Draw(Graphics g);
    }

    abstract class Shape : IShape
    {
        private Point beginPoint = new Point();
        public Point BeginPoint
        {
            get 
            {
                return beginPoint;
            }
            set
            {
                if (value != null)
                {
                    beginPoint = value;
                }
                else
                {
                    throw new ShapeException("Point is null");
                }
            }
        }

        private Point endPoint = new Point();
        public Point EndPoint
        {
            get
            {
                return endPoint;
            }
            set
            {
                if (value != null)
                {
                    endPoint = value;
                }
                else
                {
                    throw new ShapeException("Point is null");
                }
            }
        }

        private bool isFill = false;
        public bool IsFill
        {
            get
            {
                return this.isFill;
            }
            set
            {
                this.isFill = value;
            }
        }

        private Pen pen = Pens.Black;
        public Pen Pen
        {
            get
            {
                return this.pen;
            }
            set
            {
                if (value != null)
                {
                    this.pen = value;
                }
                else
                {
                    throw new ShapeException("Pen is null");
                }
            }
        }

        private Brush brush = Brushes.Black;
        public Brush Brush
        {
            get
            {
                return this.brush;
            }
            set
            {
                if (value != null)
                {
                    this.brush = value;
                }
                else
                {
                    throw new ShapeException("Brush is null");
                }
            }
        }

        public Shape(Point BP, Point EP)
        {
            BeginPoint = BP;
            EndPoint = EP;
            this.NormalizeShape();
        }

        public Shape(Point BP, Point EP, bool fill)
        {
            BeginPoint = BP;
            EndPoint = EP;
            this.isFill = fill;
            this.NormalizeShape();
        }

        public abstract void Draw(Graphics g, Pen p, Brush b);

        public abstract void Draw(Graphics g);

        public abstract object Clone();

        public void SetForeColor(Color fore)
        {
            //Толщина линии фигуры

            int w = (int)this.Pen.Width;
            DashStyle ds = this.Pen.DashStyle;

            //Цвет линии фигуры
            
            this.Pen = new Pen(fore, w);
            this.Pen.DashStyle = ds;
            
            //Цвет штрихов, если заливка штриховая

            if (this.Brush is HatchBrush)
            {
                HatchStyle hs = ((HatchBrush)this.Brush).HatchStyle;
                Color backColorHatchBrush = ((HatchBrush)this.Brush).BackgroundColor;
                this.Brush = new HatchBrush(hs, fore, backColorHatchBrush);
            }
        }

        public void SetBackColor(Color back)
        {
            if (this.Brush is HatchBrush)
            {
                HatchStyle hs = ((HatchBrush)this.Brush).HatchStyle;
                Color foreColorHatchBrush = ((HatchBrush)this.Brush).ForegroundColor;
                this.Brush = new HatchBrush(hs, foreColorHatchBrush, back);
            }
            else
            {
                this.Brush = new SolidBrush(back);
            }
        }

        public void SetPenDashStyle(DashStyle ds)
        {
            try
            {
                this.Pen.DashStyle = ds;
            }
            catch
            { }
        }

        public void SetPenWidth(int width)
        {
            try
            {
                this.Pen.Width = width;
            }
            catch { }
        }

        public void SetBrushStyle(object style)
        {
            if(!this.IsFill){ return; }
            
            //Цвет кисти и линий
            
            Color penColor = this.Pen.Color;
            Color brushColor;
            if (this.Brush is SolidBrush)
            {
                brushColor = ((SolidBrush)this.Brush).Color;
            }
            else
            {
                brushColor = ((HatchBrush)this.Brush).BackgroundColor;
            }

            //Назначение выбранной кисти

            if (style == null)
            {
                this.Brush = new SolidBrush(brushColor);
            }
            else
            {
                this.Brush = new HatchBrush((HatchStyle)style, penColor, brushColor);
            }
        }

        public int GetWidth()
        {
            return Math.Abs(this.EndPoint.X - this.BeginPoint.X);
        }

        public int GetHeight()
        {
            return Math.Abs(this.EndPoint.Y - this.BeginPoint.Y);
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(this.BeginPoint.X, this.BeginPoint.Y, this.GetWidth(), this.GetHeight());
        }

        public void Editing(MainForm.EDITMODE mode, Point BP, Point EP, Point CurrentPosition)
        {
            switch (mode)
            {
                case MainForm.EDITMODE.NONE:
                    break;
                case MainForm.EDITMODE.NW:
                    {
                        int deltaX = CurrentPosition.X - BP.X;
                        int deltaY = CurrentPosition.Y - BP.Y;
                        this.BeginPoint = new Point(BP.X + deltaX, BP.Y + deltaY);
                    }
                    break;
                case MainForm.EDITMODE.N:
                    {
                        int deltaY = CurrentPosition.Y - BP.Y;
                        this.BeginPoint = new Point(BP.X, BP.Y + deltaY);
                    }
                    break;
                case MainForm.EDITMODE.NE:
                    {
                        int deltaX = CurrentPosition.X - EP.X;
                        int deltaY = CurrentPosition.Y - BP.Y;
                        this.BeginPoint = new Point(BP.X, BP.Y + deltaY);
                        this.EndPoint = new Point(EP.X + deltaX, EP.Y);
                    }
                    break;
                case MainForm.EDITMODE.W:
                    {
                        int deltaX = CurrentPosition.X - BP.X;
                        this.BeginPoint = new Point(BP.X + deltaX, BP.Y);
                    }
                    break;
                case MainForm.EDITMODE.MOVE:
                    {
                        int deltaX = CurrentPosition.X - (EP.X + BP.X) / 2;
                        int deltaY = CurrentPosition.Y - (EP.Y + BP.Y) / 2;
                        this.BeginPoint = new Point(BP.X + deltaX, BP.Y + deltaY);
                        this.EndPoint = new Point(EP.X + deltaX, EP.Y + deltaY);
                    }
                    break;
                case MainForm.EDITMODE.E:
                    {
                        int deltaX = CurrentPosition.X - EP.X;
                        this.EndPoint = new Point(EP.X + deltaX, EP.Y);
                    }
                    break;
                case MainForm.EDITMODE.SW:
                    {
                        int deltaX = CurrentPosition.X - BP.X;
                        int deltaY = CurrentPosition.Y - EP.Y;
                        this.BeginPoint = new Point(BP.X + deltaX, BP.Y);
                        this.EndPoint = new Point(EP.X, EP.Y + deltaY);
                    }
                    break;
                case MainForm.EDITMODE.S:
                    {
                        int deltaY = CurrentPosition.Y - EP.Y;
                        this.EndPoint = new Point(EP.X, EP.Y + deltaY);
                    }
                    break;
                case MainForm.EDITMODE.SE:
                    {
                        int deltaX = CurrentPosition.X - EP.X;
                        int deltaY = CurrentPosition.Y - EP.Y;
                        this.EndPoint = new Point(EP.X + deltaX, EP.Y + deltaY);
                    }
                    break;
                default:
                    break;
            }
        }

        private void NormalizeShape()
        {
            if (this is MyLine) { return; }

            if (beginPoint.X > endPoint.X)
            {
                int temp = endPoint.X;
                endPoint.X = beginPoint.X;
                beginPoint.X = temp;
            }
            if (beginPoint.Y > endPoint.Y)
            {
                int temp = endPoint.Y;
                endPoint.Y = beginPoint.Y;
                beginPoint.Y = temp;
            }
        }
    }

    class MyLine : Shape
    {
        public MyLine(Point BP, Point EP) : base(BP, EP) { }

        public override void Draw(Graphics g, Pen p, Brush b)
        {
            if (g == null)
            {
                throw new ShapeException("Graphics is null");
            }
            else if (p == null)
            {
                throw new ShapeException("Pen is null");
            }

            g.DrawLine(p, BeginPoint, EndPoint);
        }

        public override void Draw(Graphics g)
        {
            if (g == null)
            {
                throw new ShapeException("Graphics is null");
            }

            g.DrawLine(this.Pen, BeginPoint, EndPoint);
        }

        public override string ToString()
        {
            return String.Format("Прямая ({0}, {1}) - ({2}, {3})", BeginPoint.X, BeginPoint.Y, EndPoint.X, EndPoint.Y);
        }

        public override object Clone()
        {
            MyLine clone = new MyLine(this.BeginPoint, this.EndPoint);
            clone.Pen = (Pen)this.Pen.Clone();
            clone.Brush = (Brush)this.Brush.Clone();
            clone.IsFill = this.IsFill;
            return clone;
        }
    }

    class MyRectangle : Shape
    {
        public MyRectangle(Point BP, Point EP) : base(BP, EP) { }

        public MyRectangle(Point BP, Point EP, bool fill) : base(BP, EP, fill) { }

        public override void Draw(Graphics g, Pen p, Brush b)
        {
            //Проверка параметров

            if (g == null)
            {
                throw new ShapeException("Graphics is null");
            }
            else if (g == null && p == null)
            {
                throw new ShapeException("Pen and Brush is null");
            }

            //Нарисовать прямоугольник необходимого типа

            if (this.IsFill)
            {
                if (b == null) { throw new ShapeException("Brash is null"); }
                g.FillRectangle(b, new Rectangle(BeginPoint.X, BeginPoint.Y, EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y));
                if (p != null)
                {
                    g.DrawRectangle(p, new Rectangle(BeginPoint.X, BeginPoint.Y, EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y));
                }
            }
            else
            {
                if (p == null) { throw new ShapeException("Pen is null"); }
                g.DrawRectangle(p, new Rectangle(BeginPoint.X, BeginPoint.Y, EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y));
            }
        }

        public override void Draw(Graphics g)
        {
            if (g == null)
            {
                throw new ShapeException("Graphics is null");
            }

            if (this.IsFill)
            {
                g.FillRectangle(this.Brush, new Rectangle(BeginPoint.X, BeginPoint.Y, EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y));
            }
            g.DrawRectangle(this.Pen, new Rectangle(BeginPoint.X, BeginPoint.Y, EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y));
        }

        public override string ToString()
        {
            return String.Format("Прямоугольник ({0}, {1}) - ({2}, {3})", BeginPoint.X, BeginPoint.Y, EndPoint.X, EndPoint.Y);
        }

        public override object Clone()
        {
            MyRectangle clone = new MyRectangle(this.BeginPoint, this.EndPoint);
            clone.Pen = (Pen)this.Pen.Clone();
            clone.Brush = (Brush)this.Brush.Clone();
            clone.IsFill = this.IsFill;
            return clone;
        }
    }

    class MyEllipse : Shape
    {
        public MyEllipse(Point BP, Point EP) : base(BP, EP) { }

        public MyEllipse(Point BP, Point EP, bool fill) : base(BP, EP, fill) { }

        public override void Draw(Graphics g, Pen p, Brush b)
        {
            if (g == null)
            {
                throw new ShapeException("Graphics is null");
            }
            else if (g == null && p == null)
            {
                throw new ShapeException("Pen && Brush is null");
            }

            //Нарисовать элипс необходимого типа

            if (this.IsFill)
            {
                if (b == null) { throw new ShapeException("Brash is null"); }
                g.FillEllipse(b, new Rectangle(BeginPoint.X, BeginPoint.Y, EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y));
                if (p != null)
                {
                    g.DrawEllipse(p, new Rectangle(BeginPoint.X, BeginPoint.Y, EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y));
                }
            }
            else
            {
                if (p == null) { throw new ShapeException("Pen is null"); }
                g.DrawEllipse(p, new Rectangle(BeginPoint.X, BeginPoint.Y, EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y));
            }
        }

        public override void Draw(Graphics g)
        {
            if (g == null)
            {
                throw new ShapeException("Graphics is null");
            }

            if (this.IsFill)
            {
                g.FillEllipse(this.Brush, new Rectangle(BeginPoint.X, BeginPoint.Y, EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y));
            }
            g.DrawEllipse(this.Pen, new Rectangle(BeginPoint.X, BeginPoint.Y, EndPoint.X - BeginPoint.X, EndPoint.Y - BeginPoint.Y));
        }

        public override string ToString()
        {
            return String.Format("Эллипс ({0}, {1}) - ({2}, {3})", BeginPoint.X, BeginPoint.Y, EndPoint.X, EndPoint.Y);
        }

        public override object Clone()
        {
            MyEllipse clone = new MyEllipse(this.BeginPoint, this.EndPoint);
            clone.Pen = (Pen)this.Pen.Clone();
            clone.Brush = (Brush)this.Brush.Clone();
            clone.IsFill = this.IsFill;
            return clone;
        }
    }

    class MyFill : IShape
    {
        #region Импорт WinApi функций

        [DllImport("gdi32.dll", EntryPoint = "GetPixel", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetPixel(IntPtr hDc, int x, int y);

        [DllImport("gdi32.dll", EntryPoint = "ExtFloodFill", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern uint ExtFloodFill(IntPtr hDc, int x, int y, uint clr, int mode);

        [DllImport("gdi32.dll", EntryPoint = "CreateSolidBrush", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CreateSolidBrush(uint clr);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SelectObject(IntPtr hDc, IntPtr obj);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DeleteObject(IntPtr obj);

        #endregion
        
        private Point fillPoint;
        public Point FillPoint
        {
            get 
            {
                return fillPoint;
            }
            set
            {
                if (value != null)
                {
                    fillPoint = value;
                }
                else
                {
                    throw new ShapeException("Point is null");
                }
            }
        }

        private Color fillColor;
        public Color FillColor
        {
            get
            {
                return fillColor;
            }
            set
            {
                if (value != null)
                {
                    fillColor = value;
                }
                else
                {
                    throw new ShapeException("Color is null");
                }
            }
        }

        public MyFill(Point p, Color clr)
        {
            FillPoint = p;
            FillColor = clr;
        }

        public void Draw(Graphics g, Pen p, Brush b)
        {
            this.Draw(g);
        }

        public void Draw(Graphics g)
        {
            if (g == null)
            {
                throw new ShapeException("Graphics is null");
            }

            //Получаем контекст устройства

            IntPtr hDc = g.GetHdc();
            
            //Цвет для заливки

            uint currentColor = GetPixel(hDc, this.FillPoint.X, this.FillPoint.Y);
            
            //Цвет, которым заливаем
            
            uint fillColor = (uint)((this.FillColor.B << 16) | (this.FillColor.G << 8) | this.FillColor.R);

            //Кисть для заливки

            IntPtr fillBrush = CreateSolidBrush(fillColor);

            //Назначение кисти для заливки
            
            IntPtr oldBrush = SelectObject(hDc, fillBrush);

            //Заливка

            ExtFloodFill(hDc, this.FillPoint.X, this.FillPoint.Y, currentColor, 1);

            //Возвращение кисти

            SelectObject(hDc, oldBrush);

            //Удалние кисти

            DeleteObject(fillBrush);

            //Освобождение контекста устройства

            g.ReleaseHdc();
        }

        public void SetColor(Color clr)
        {
            if (clr.IsEmpty)
            {
                return;
            }
            this.FillColor = clr;
        }

        public object Clone()
        {
            return new MyFill(this.FillPoint, this.FillColor);
        }

        public override string ToString()
        {
            return String.Format("Заливка ({0}, {1})", FillPoint.X, FillPoint.Y);
        }
    }

    class MyPen : IShape
    {
        private List<Point> points = new List<Point>();
        public List<Point> Points
        {
            get
            {
                return this.points;
            }
            set
            {
                this.points = value;
            }
        }

        private Pen pen = new Pen(Color.Black);
        public Pen Pen
        {
            get
            {
                return this.pen;
            }
            set
            {
                if (value != null)
                {
                    this.pen = value;
                }
                else
                {
                    throw new ShapeException("Pen is null");
                }
            }
        }

        public MyPen(List<Point> points)
        {
            if (points == null)
            {
                return;
            }
            foreach (Point point in points)
            {
                this.points.Add(new Point(point.X, point.Y));
            }
        }

        public void SetPen(Pen p)
        {
            if (p != null)
            {
                this.pen = p;
            }
        }

        public void SetColor(Color clr)
        {
            if (clr.IsEmpty)
            {
                return;
            }

            int widthPen = (int)this.Pen.Width;
            this.Pen = new Pen(clr, widthPen);
        }

        public void SetWidth(int width)
        {
            if (width < 1)
            {
                return;
            }

            Color colorPen = this.Pen.Color;
            this.Pen = new Pen(colorPen, width);
        }

        public void Editing(MainForm.EDITMODE mode, Point CenterPoint, Point CurrentPosition)
        {
            switch (mode)
            {
                case MainForm.EDITMODE.MOVE:
                    int deltaX = CurrentPosition.X - CenterPoint.X;
                    int deltaY = CurrentPosition.Y - CenterPoint.Y;

                    //Пересчёт точек

                    for (int i = 0; i < this.points.Count; i++)
                    {
                        this.points[i] = new Point(this.points[i].X + deltaX, this.points[i].Y + deltaY);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Draw(Graphics g, Pen p, Brush b)
        {
            this.Draw(g);
        }

        public void Draw(Graphics g)
        {
            if (g == null)
            {
                throw new ShapeException("Graphics is null");
            }

            if (this.points == null || this.pen == null)
            {
                throw new ShapeException("Points or Pen is null");
            }

            if (this.points.Count <= 1)
            {
                return;
            }

            //Соединяем прямыми точки

            for (int i = 0; i < this.points.Count - 1; i++)
            {
                Point p1 = this.points[i];
                Point p2 = this.points[i + 1];
                g.DrawLine(this.Pen, p1, p2);
            }
        }

        public object Clone()
        {
            List<Point> points = new List<Point>();
            foreach (Point p in this.points)
            {
                points.Add(p);
            }
            MyPen clone = new MyPen(points);
            clone.Pen = this.Pen;
            return clone;
        }

        public override string ToString()
        {
            return String.Format("Карандаш");
        }
    }
}