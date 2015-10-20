using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace Paint
{
    public partial class MainForm : Form
    {
        #region WinApi функции для Zoom

        [DllImport("gdi32.dll", EntryPoint = "StretchBlt")]
        public static extern uint StretchBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int wSrc, int hSrc, uint dwRop);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SelectObject(IntPtr hDc, IntPtr obj);
       
        #endregion

        #region Параметры, Перечисления

        public enum EDITMODE { NONE, NW, N, NE, W, MOVE, E, SW, S, SE };
        private enum SHAPES { LINE, RECTANGLE, FILLRECTANGLE, ELLIPS, FILLELLIPS, FILLTOOL, COLORCHOICE, ZOOM, PEN };
        private int defaultCanvasWidth = 800;
        private int defaultCanvasHeight = 600;

        #endregion

        #region Буферы
       
        private BufferedGraphicsContext context;
        private BufferedGraphics bgGraph;
        private BufferedGraphics bgImage;
        private Canvas canvas = new Canvas();

        #endregion

        #region Состояние/Данные
        
        //Cостояние

        private bool isDraw = false;
        private bool isEdit = false;
        private bool isOpenedFile = false;
        private bool isZoom = false;
        private bool isHide = false;
        private SHAPES typeTool = SHAPES.PEN;
        private Point bPoint = Point.Empty;
        private Image currentImage = null;

        //Коллекции объектов

        private List<IShape> shapesList = new List<IShape>();
        private List<IShape> editBoxShapes = new List<IShape>();
        private List<Point> pointsOfMovement = new List<Point>();
        
        //Переменные для редактирование объектов

        private EDITMODE editMode;
        private IShape tempShape = null;
        private IShape tempEditedShape = null;

        #endregion

        #region Элементы управления

        ColorChoicer colorChoicer = new ColorChoicer();
        LineStyleChoicer lineStyleChoicer = new LineStyleChoicer();
        HatchChoicer hatchChoiser = new HatchChoicer();
        LineWidthChoicer widthChiocer = new LineWidthChoicer(1, 10);
        CheckBox cb_antiAlias = new CheckBox();

        #endregion

        #region DebugData
        
        #endregion

        public MainForm()
        {
            InitializeComponent();

            #region Холст, буфер

            this.canvas.BackColor = Color.White;
            this.canvas.Size = new Size(this.defaultCanvasWidth, this.defaultCanvasHeight);
            this.panel1.Controls.Add(canvas);

            this.context = BufferedGraphicsManager.Current;
            this.context.MaximumBuffer = new Size(this.canvas.Width, this.canvas.Height);
            this.bgGraph = this.context.Allocate(this.canvas.CreateGraphics(), new Rectangle(0, 0, this.canvas.Width, this.canvas.Height));

            #endregion

            #region Привязки к событиям элементов управления приложения

            //Форма

            this.Paint += Form1_Paint;
            
            //Холст

            this.canvas.Paint += canvas_Paint;
            this.canvas.MouseDown += canvas_MouseDown;
            this.canvas.MouseMove += canvas_MouseMove;
            this.canvas.MouseUp += canvas_MouseUp;
            this.canvas.MouseLeave += canvas_MouseLeave;
            //this.canvas.MouseClick += canvas_MouseClick;
            
            //Список фигур

            this.cb_shapeList.SelectedIndexChanged += cb_shapeList_SelectedIndexChanged;
            this.cb_shapeList.DataSourceChanged += cb_shapeList_DataSourceChanged;

            //Элементы управления

            this.colorChoicer.MouseClick += colorChoicer_MouseClick;
            this.lineStyleChoicer.MouseClick += lineStyleChoicer_MouseClick;
            this.widthChiocer.Paint += widthChiocer_Paint;
            this.hatchChoiser.MouseClick += hatchChoiser_MouseClick;

            #endregion

            #region ToolBar

            this.tsb_pen.CheckState = CheckState.Checked;

            this.cb_antiAlias.Text = "Сглаживание";
            this.cb_antiAlias.FlatStyle = FlatStyle.System;
            this.cb_antiAlias.CheckStateChanged += (s, ex) => this.BufferToCanvas();

            ToolStripControlHost host = new ToolStripControlHost(cb_antiAlias);
            this.toolStrip1.Items.Insert(toolStrip1.Items.Count, host);

            #endregion

            #region StatusBar

            this.tssl_size.Text = String.Format("Size: {0} x {1}", this.canvas.Width, this.canvas.Height);
            this.tssl_tool.Text = "Tool: Линия";

            #endregion

            #region Левая панель (кастомные элементы управления)

            this.widthChiocer.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel2.Controls.Add(this.widthChiocer);

            this.lineStyleChoicer.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel2.Controls.Add(this.lineStyleChoicer);

            this.hatchChoiser.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel2.Controls.Add(this.hatchChoiser);

            this.colorChoicer.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel2.Controls.Add(colorChoicer);

            //Подключение элементов к событиям элементов

            this.lineStyleChoicer.LineStyleChanged += this.widthChiocer.SetLineStyle;
            
            #endregion
        }

        #region События (рисование/элементы управления и т.д)
        
        void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.BufferToCanvas();
        }

        void canvas_Paint(object sender, PaintEventArgs e)
        {
            this.BufferToCanvas();
        }

        void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            this.bPoint = e.Location;

            //Заливка / пипетка

            if (!this.isEdit)
            {
                switch (this.typeTool)
                {
                    case SHAPES.FILLTOOL:
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            this.Fill(e.Location);
                        }
                        return;
                    case SHAPES.COLORCHOICE:
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            this.ChooseColor(e.Location, true);
                        }
                        else
                        {
                            this.ChooseColor(e.Location, false);
                        }
                        return;
                }
            }

            //Режимы паинта / рисование / редактирование
            
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //Режим рисования

                if (!this.isEdit)
                {
                    this.isDraw = true;
                    
                    //Начало линии карандаша

                    if (this.typeTool == SHAPES.PEN)
                    {
                        this.pointsOfMovement.Add(bPoint);
                    }
                }

                //Режим редактирования

                else
                {
                    this.BeginEditObject(e.Location);
                }
            }
        }

        void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Скрыть показать курсор для линзы
            
            if (this.isZoom)
            {
                if (!this.isHide)
                {
                    Cursor.Hide();
                    this.isHide = true;
                }
            }
            else
            {
                if (this.isHide)
                {
                    Cursor.Show();
                    this.isHide = false;
                }
            }

            //Курсор и строка состояния

            this.SetCanvasCursor(e.Location);
            this.tssl_position.Text = String.Format("Position: {0}, {1}", e.X, e.Y);

            //Рисование

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.isDraw)
                {
                    this.pointsOfMovement.Add(e.Location);
                    this.ShowDrawableShape(e.Location);
                }

                //Редактирование

                else if (isEdit)
                {
                    this.EditedObject(e.Location);
                }
            }

            //Линза

            else if (e.Button == System.Windows.Forms.MouseButtons.Right && this.typeTool != SHAPES.COLORCHOICE)
            {
                this.isZoom = true;
                this.Zoom(e.Location);
            }
        }

        void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            this.isZoom = false;

            if (isDraw)
            {
                this.isDraw = false;
                this.AddShapeToList(e.Location);
            }
            else if (isEdit)
            {
                this.editMode = EDITMODE.NONE;
                
                //Сохранить текущее состояние

                IShape currentShape = this.GetCurrentEditedObject();
                this.tempEditedShape = (IShape)currentShape.Clone();
            }

            //Обновление холста

            this.BufferToCanvas();
        }

        void canvas_MouseLeave(object sender, EventArgs e)
        {
            if (this.typeTool == SHAPES.COLORCHOICE)
            {
                this.BufferToCanvas();
            }
        }

        void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isEdit)
            {
                return;
            }

            switch (this.typeTool)
            {
                case SHAPES.FILLTOOL:
                    {
                        MyFill currentFill = null;
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            currentFill = new MyFill(e.Location, this.colorChoicer.BColor);
                        }
                        this.shapesList.Add(currentFill);

                        this.UpdateShapeListComboBox();

                        this.BufferToCanvas();
                    }
                    break;
                case SHAPES.COLORCHOICE:
                    {
                        Graphics g = this.canvas.CreateGraphics();
                        IntPtr hDc = g.GetHdc();
                        uint clr = MyFill.GetPixel(hDc, e.X, e.Y);
                        uint R = clr & 0xFF;
                        uint G = (clr >> 8) & 0xFF;
                        uint B = (clr >> 16) & 0xFF;
                        Color currentColor = Color.FromArgb(255, (int)R, (int)G, (int)B);
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            this.colorChoicer.FColor = currentColor;
                        }
                        else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                        {
                            this.colorChoicer.BColor = currentColor;
                        }
                        this.colorChoicer.Invalidate();
                        g.ReleaseHdc();
                    }
                    break;
                default:
                    break;
            }
        }

        void cb_shapeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox currentCombo = (ComboBox)sender;
            if (currentCombo.SelectedIndex == -1)
            {
                return;
            }
            this.btn_edit.Enabled = true;
            this.btn_delete.Enabled = true;
        }

        void cb_shapeList_DataSourceChanged(object sender, EventArgs e)
        {
            ComboBox currentCombo = (ComboBox)sender;
            if (currentCombo.DataSource == null)
            {
                this.btn_delete.Enabled = false;
                this.btn_edit.Enabled = false;
                this.btn_apply.Enabled = false;
                this.btn_cancel.Enabled = false;
            }
        }

        void colorChoicer_MouseClick(object sender, MouseEventArgs e)
        {
           //Обновить холст, если в режиме редактирования

            if (this.isEdit)
            {
                this.BufferToCanvas();
            }
        }

        void lineStyleChoicer_MouseClick(object sender, MouseEventArgs e)
        {
            //Обновить холст, если в режиме редактирования

            if (this.isEdit)
            {
                this.BufferToCanvas();
            }
        }

        void widthChiocer_Paint(object sender, PaintEventArgs e)
        {
            //Обновить холст при перересовке элемента в режиме редактирования

            if (this.isEdit)
            {
                this.BufferToCanvas();
            }
        }

        void hatchChoiser_MouseClick(object sender, MouseEventArgs e)
        {
            //Обновить холст при перересовке элемента в режиме редактирования

            if (this.isEdit)
            {
                this.BufferToCanvas();
            }
        }

        #endregion

        #region ToolBar

        private void ToolButton_Click(object sender, EventArgs e)
        {
            //Режим редактирования фигуры - не позволяет переключаться

            if (this.isEdit) { return; }

            //Снять нажатие

            foreach (var item in this.toolStrip1.Items)
            {
                ToolStripButton button = item as ToolStripButton;
                if (button != null)
                {
                    button.CheckState = CheckState.Unchecked;
                }
            }

            //Установить нажатие

            ((ToolStripButton)sender).CheckState = CheckState.Checked;

            //Установить тип фигуры

            switch (((ToolStripButton)sender).Name)
            {
                case "tsb_pen":
                    this.typeTool = SHAPES.PEN;
                    this.tssl_tool.Text = "Tool: Карандаш";
                    break;
                case "tsb_line":
                    this.typeTool = SHAPES.LINE;
                    this.tssl_tool.Text = "Tool: Линия";
                    break;
                case "tsb_rect":
                    this.typeTool = SHAPES.RECTANGLE;
                    this.tssl_tool.Text = "Tool: Прямоугольник";
                    break;
                case "tlb_fillrect":
                    this.typeTool = SHAPES.FILLRECTANGLE;
                    this.tssl_tool.Text = "Tool: Залитый прямоугольник";
                    break;
                case "tlb_elips":
                    this.typeTool = SHAPES.ELLIPS;
                    this.tssl_tool.Text = "Tool: Эллипс";
                    break;
                case "tlb_fillelips":
                    this.typeTool = SHAPES.FILLELLIPS;
                    this.tssl_tool.Text = "Tool: Залитый эллипс";
                    break;
                case "tlb_fill":
                    this.typeTool = SHAPES.FILLTOOL;
                    this.tssl_tool.Text = "Tool: Заливка";
                    break;
                case "tlb_colorChoicer":
                    typeTool = SHAPES.COLORCHOICE;
                    this.tssl_tool.Text = "Tool: Выбор цвета";
                    break;
                default:
                    break;
            }
        }

        private void tsb_new_Click(object sender, EventArgs e)
        {
            this.CreateNewImage();
        }

        private void tsb_open_Click(object sender, EventArgs e)
        {
            this.OpenImage();
        }

        private void tsl_save_Click(object sender, EventArgs e)
        {
            this.SaveImage();
        }

        private void tsb_undo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Не реализовано и не будет. Не нажимайте больше :)", "Paint", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsb_redo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Не реализовано и не будет. Не нажимайте больше :)", "Paint", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Меню

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreateNewImage();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenImage();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveImage();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Кнопки работы с фигурой :) (на холсте которая)

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (this.cb_shapeList.SelectedIndex == -1 || this.shapesList.Count == 0)
            {
                return;
            }

            if (this.isEdit)
            {
                this.isEdit = false;
                this.cb_shapeList.Enabled = true;
            }

            int currentShapeIndex = this.cb_shapeList.SelectedIndex;
            this.shapesList.RemoveAt(currentShapeIndex);

            this.UpdateShapeListComboBox();

            this.editBoxShapes.Clear();
            this.BufferToCanvas();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            //Нет фигуры в списке - выходим

            if (this.cb_shapeList.SelectedIndex == -1 || this.shapesList.Count == 0)
            {
                return;
            }

            //Настройка интерфейса приложения

            this.isEdit = true;
            this.btn_edit.Enabled = false;
            this.btn_apply.Enabled = true;
            this.btn_cancel.Enabled = true;
            this.cb_shapeList.Enabled = false;

            //Получаем ссылку на редактирумую фигуру

            IShape currentShape = this.GetCurrentEditedObject();

            //Сохранить копию для отмены и радактирования

            this.tempShape = (IShape)currentShape.Clone();
            this.tempEditedShape = (IShape)currentShape.Clone();

            //Фигура

            if (currentShape is Shape)
            {
                Shape myShape = currentShape as Shape;
                
                //Создать элементы "сетки" для редактирования

                this.CreateEditBoxShapes(currentShape);

                //Настроить элементы управления параметрами фигуры

                //Цвет

                this.colorChoicer.SetParametersFromShape(myShape);
                this.colorChoicer.FColorChanged += myShape.SetForeColor;
                if (myShape.IsFill)
                {
                    this.colorChoicer.BColorChanged += myShape.SetBackColor;
                }

                //Стиль линии

                this.lineStyleChoicer.SetLineStyle(myShape.Pen.DashStyle);
                this.lineStyleChoicer.LineStyleChanged += myShape.SetPenDashStyle;

                //Толщина линии

                this.widthChiocer.SetLineWidth((int)myShape.Pen.Width);
                this.widthChiocer.LineWidthChanged += myShape.SetPenWidth;

                //Тип штриховки

                this.hatchChoiser.SetHatchStyle(myShape.Brush);
                this.hatchChoiser.HatchStyleChanged += myShape.SetBrushStyle;
            }

            //Заливка
            
            else if (currentShape is MyFill)
            {
                MyFill myFill = currentShape as MyFill;
                this.CreateEditBoxShapes(currentShape);
                this.colorChoicer.BColorChanged += myFill.SetColor;
            }

            //Карандаш

            else if (currentShape is MyPen)
            {
                MyPen myPen = currentShape as MyPen;
                this.CreateEditBoxShapes(currentShape);
                this.colorChoicer.FColorChanged += myPen.SetColor;
                this.widthChiocer.LineWidthChanged += myPen.SetWidth;
            }

            //Обновить холст

            this.BufferToCanvas();
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            this.EndEditing(false);
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.EndEditing(true);
        }

        #endregion

        #region Работа с файлами

        private void CreateNewImage()
        {
            this.bPoint = Point.Empty;

            //Диалог создания изображения

            NewImageDialog nfDialog = new NewImageDialog();
            if (nfDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Сброс состояния приложиния

                this.ResetState(true);

                //Буферы/холст

                this.canvas.Size = new Size(nfDialog.ImageWidth, nfDialog.ImageHeight);
                
                this.context.MaximumBuffer = new Size(this.canvas.Width, this.canvas.Height);
                this.bgGraph = this.context.Allocate(this.canvas.CreateGraphics(), new Rectangle(0, 0, this.canvas.Width, this.canvas.Height));
                this.bgGraph.Graphics.Clear(Color.White);
                this.bgGraph.Render(this.canvas.CreateGraphics());
                if (this.bgImage != null)
                {
                    this.bgImage.Graphics.Clear(Color.White);
                }

                //Интерфейс

                this.tssl_size.Text = String.Format("Size: {0} x {1}", this.canvas.Width, this.canvas.Height);
                this.cb_shapeList.Enabled = true;
            }
        }

        private void OpenImage()
        {
            this.bPoint = Point.Empty;

            //Создать диалог открытия файла

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Все файлы|*.*|Изображение *.bmp|*.bmp|Изображение *.jpg|*.jpg|Изображение *.jpeg|*.jpeg|Изображение *.png|*.png|Изображение *.gif|*.gif";
            openDialog.InitialDirectory = Application.StartupPath;
            openDialog.FileName = "";

            //Запрос имени файла / загрузка

            if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    this.currentImage = Image.FromFile(openDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Не удалось загрузить изображение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Состояние

                this.ResetState(false);

                //Загрузить изображение / подготовить холст

                this.canvas.Size = new Size(this.currentImage.Width, this.currentImage.Height);
                this.context.MaximumBuffer = new Size(this.canvas.Width, this.canvas.Height);
                this.bgGraph = this.context.Allocate(this.canvas.CreateGraphics(), new Rectangle(0, 0, this.canvas.Width, this.canvas.Height));
                this.bgImage = this.context.Allocate(this.canvas.CreateGraphics(), new Rectangle(0, 0, this.canvas.Width, this.canvas.Height));

                //Интерфейс

                this.tssl_size.Text = String.Format("Size: {0} x {1}", this.canvas.Width, this.canvas.Height);
                this.cb_shapeList.Enabled = true;

                this.BufferToCanvas();
            }
        }

        private void SaveImage()
        {
            this.bPoint = Point.Empty;

            //Создать диалог открытия файла

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = ".bmp";
            saveDialog.InitialDirectory = Application.StartupPath;
            saveDialog.Filter = "Изображение *.bmp|*.bmp|Изображение *.jpg|*.jpg|Изображение *.png|*.png|Изображение *.gif|*.gif";
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Создать битмап / рендерить буфер в битмап

                Bitmap bitmap = new Bitmap(this.canvas.Width, this.canvas.Height);
                Graphics bitmapGraph = Graphics.FromImage(bitmap);
                this.bgGraph.Render(bitmapGraph);

                //Сохраняем изображение в файл

                ImageFormat imageFormat = ImageFormat.Bmp;
                switch (Path.GetExtension(saveDialog.FileName))
                {
                    case ".bmp":
                        bitmap.Save(saveDialog.FileName, ImageFormat.Bmp);
                        break;
                    case ".jpg":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case ".png":
                        imageFormat = ImageFormat.Png;
                        break;
                    case ".gif":
                        imageFormat = ImageFormat.Gif;
                        break;
                }
                try
                {
                    bitmap.Save(saveDialog.FileName, imageFormat);
                }
                catch
                {
                    MessageBox.Show("Не удалось сохранить файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        #endregion

        #region Zoom-методы

        private void Zoom(Point pos)
        {
            int srcSize = 40;
            int destSize = 120;

            //Графикс холста

            Graphics g = this.canvas.CreateGraphics();

            //Рендер буфера в графикс холста

            this.bgGraph.Render(g);

            //ГрафПаз для линзы

            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(new Rectangle(pos.X - destSize / 2, pos.Y - destSize / 2, destSize, destSize));

            //Регион

            Region R = new Region(gp);
            IntPtr rgn = R.GetHrgn(g);

            //DC назначения

            IntPtr hdcDest = g.GetHdc();

            //DC источника

            IntPtr hdcSrc = bgGraph.Graphics.GetHdc();

            //Назначить регион назначению

            IntPtr rgnOld = SelectObject(hdcDest, rgn);

            //Копирование с увеличением

            StretchBlt(hdcDest, pos.X - destSize / 2, pos.Y - destSize / 2, destSize, destSize, hdcSrc, pos.X - srcSize / 2, pos.Y - srcSize / 2, srcSize, srcSize, 0xCC0020);

            //Вернуть регион

            SelectObject(hdcDest, rgnOld);

            //Освобождение хэндлов

            g.ReleaseHdc();
            bgGraph.Graphics.ReleaseHdc();

            //Рамка линзы

            g.DrawEllipse(new Pen(Color.Black), pos.X - destSize / 2 - 1, pos.Y - destSize / 2 - 1, destSize - 2, destSize - 2);
        }

        private void ZoomForColorChoice(Point pos)
        {
            //Размеры областей для копированя
            //Масштаб 8 : 1 (40 / 5)

            int srcSize = 5;
            int destSize = 40;

            //Смещение "линзы" от курсора

            int offset = 5;

            //Координаты линзы в зависимости от положения курсора на холсте

            int zoomX;
            int zoomY;

            //3

            if (pos.X + destSize + offset > this.canvas.Width && pos.Y - destSize - offset < 0)
            {
                zoomX = pos.X - destSize - offset;
                zoomY = pos.Y + offset;
            }

            //2

            else if (pos.X + destSize + offset > this.canvas.Width)
            {
                zoomX = pos.X - destSize - offset;
                zoomY = pos.Y - destSize - offset;
            }

            //4

            else if (pos.Y - destSize - offset < 0)
            {
                zoomX = pos.X + offset;
                zoomY = pos.Y + offset;
            }

            //1

            else
            {
                zoomX = pos.X + offset;
                zoomY = pos.Y - destSize - offset;
            }

            //Графикс холста

            Graphics g = this.canvas.CreateGraphics();

            //Рендер буфера в графикс холста

            this.bgGraph.Render(g);

            //GraphicsPath для линзы (прямоугольный)

            GraphicsPath gp = new GraphicsPath();
            Rectangle r = new Rectangle(
                zoomX,
                zoomY,
                destSize,
                destSize);
            gp.AddRectangle(r);

            //Регион из GraphicsPath

            Region R = new Region(gp);
            IntPtr rgn = R.GetHrgn(g);

            //DC назначения

            IntPtr hdcDest = g.GetHdc();

            //DC источника

            IntPtr hdcSrc = bgGraph.Graphics.GetHdc();

            //Назначить регион назначению

            IntPtr rgnOld = SelectObject(hdcDest, rgn);

            //Копирование с увеличением

            StretchBlt(
                hdcDest,
                zoomX,
                zoomY,
                destSize,
                destSize,
                hdcSrc,
                pos.X - srcSize / 2,
                pos.Y - srcSize / 2,
                srcSize,
                srcSize,
                0xCC0020
            );

            //Вернуть регион

            SelectObject(hdcDest, rgnOld);

            //Освобождение хэндлов

            g.ReleaseHdc();
            bgGraph.Graphics.ReleaseHdc();

            //Рамка линзы + "прицел"

            int rectX = zoomX;
            int rectY = zoomY;
            int rectSize = destSize;

            //Рамка

            int centerSize = 8;

            g.DrawRectangle(
                new Pen(Color.Black),
                rectX,
                rectY,
                rectSize,
                rectSize);

            //Прицел

            g.DrawRectangle(
                new Pen(Color.Black),
                rectX + (rectSize - centerSize) / 2,
                rectY + (rectSize - centerSize) / 2,
                centerSize,
                centerSize);
        }

        #endregion

        #region Инструменты рисования/редактирования

        private void AddShapeToList(Point endPoint)
        {
            //Обрабатываются в клике

            if (this.typeTool == SHAPES.FILLTOOL || this.typeTool == SHAPES.COLORCHOICE)
            {
                return;
            }

            Shape currentShape = null;
            switch (this.typeTool)
            {
                case SHAPES.PEN:
                    this.pointsOfMovement.Add(endPoint);
                    MyPen myPen = new MyPen(this.pointsOfMovement);
                    Pen pen = new Pen(this.colorChoicer.FColor, this.widthChiocer.LineWidth);
                    myPen.Pen = pen;
                    this.shapesList.Add(myPen);
                    this.pointsOfMovement.Clear();
                    this.UpdateShapeListComboBox();
                    return;
                case SHAPES.LINE:
                    currentShape = new MyLine(this.bPoint, endPoint);
                    break;
                case SHAPES.RECTANGLE:
                    currentShape = new MyRectangle(this.bPoint, endPoint);
                    break;
                case SHAPES.FILLRECTANGLE:
                    currentShape = new MyRectangle(this.bPoint, endPoint, true);
                    break;
                case SHAPES.ELLIPS:
                    currentShape = new MyEllipse(this.bPoint, endPoint);
                    break;
                case SHAPES.FILLELLIPS:
                    currentShape = new MyEllipse(this.bPoint, endPoint, true);
                    break;
                default:
                    throw new ShapeException("Неизвестный тип фигуры");
            }

            Pen currentPen = new Pen(this.colorChoicer.FColor, this.widthChiocer.LineWidth);
            currentPen.DashStyle = this.lineStyleChoicer.SelectedStyle;
            currentShape.Pen = currentPen;

            if (this.hatchChoiser.SelectedHatch == null)
            {
                currentShape.Brush = new SolidBrush(this.colorChoicer.BColor);
            }
            else
            {
                currentShape.Brush = new HatchBrush((HatchStyle)this.hatchChoiser.SelectedHatch, this.colorChoicer.FColor, this.colorChoicer.BColor);
            }

            this.shapesList.Add(currentShape);

            this.UpdateShapeListComboBox();

            this.pointsOfMovement.Clear();
        }

        private void ShowDrawableShape(Point endPoint)
        {
            //Отрисовка списка фигур в буфер

            this.DrawShapesToBuffer();

            //Отрисовка текущей фигуры в буфер

            Pen currentPen = new Pen(this.colorChoicer.FColor, this.widthChiocer.LineWidth);
            currentPen.DashStyle = this.lineStyleChoicer.SelectedStyle;

            Brush currentBrush = null;
            if (this.hatchChoiser.SelectedHatch == null)
            {
                currentBrush = new SolidBrush(this.colorChoicer.BColor);
            }
            else
            {
                currentBrush = new HatchBrush((HatchStyle)this.hatchChoiser.SelectedHatch, this.colorChoicer.FColor, this.colorChoicer.BColor);
            }

            Graphics bg = this.bgGraph.Graphics;
            switch (this.typeTool)
            {
                case SHAPES.PEN:
                    Pen pen = new Pen(this.colorChoicer.FColor);
                    pen.Width = this.widthChiocer.LineWidth;
                    for (int i = 0; i < this.pointsOfMovement.Count - 1; i++)
                    {
                        Point p1 = this.pointsOfMovement[i];
                        Point p2 = this.pointsOfMovement[i + 1];
                        bg.DrawLine(pen, p1, p2);
                    }
                    break;
                case SHAPES.LINE:
                    bg.DrawLine(currentPen, this.bPoint, endPoint);
                    break;
                case SHAPES.RECTANGLE:
                    bg.DrawRectangle(currentPen, new Rectangle((this.bPoint.X < endPoint.X) ? this.bPoint.X : endPoint.X, (this.bPoint.Y < endPoint.Y) ? this.bPoint.Y : endPoint.Y, Math.Abs(endPoint.X - this.bPoint.X), Math.Abs(endPoint.Y - this.bPoint.Y)));
                    break;
                case SHAPES.FILLRECTANGLE:
                    bg.FillRectangle(currentBrush, new Rectangle((this.bPoint.X < endPoint.X) ? this.bPoint.X : endPoint.X, (this.bPoint.Y < endPoint.Y) ? this.bPoint.Y : endPoint.Y, Math.Abs(endPoint.X - this.bPoint.X), Math.Abs(endPoint.Y - this.bPoint.Y)));
                    bg.DrawRectangle(currentPen, new Rectangle((this.bPoint.X < endPoint.X) ? this.bPoint.X : endPoint.X, (this.bPoint.Y < endPoint.Y) ? this.bPoint.Y : endPoint.Y, Math.Abs(endPoint.X - this.bPoint.X), Math.Abs(endPoint.Y - this.bPoint.Y)));
                    break;
                case SHAPES.ELLIPS:
                    bg.DrawEllipse(currentPen, new Rectangle((this.bPoint.X < endPoint.X) ? this.bPoint.X : endPoint.X, (this.bPoint.Y < endPoint.Y) ? this.bPoint.Y : endPoint.Y, Math.Abs(endPoint.X - this.bPoint.X), Math.Abs(endPoint.Y - this.bPoint.Y)));
                    break;
                case SHAPES.FILLELLIPS:
                    bg.FillEllipse(currentBrush, new Rectangle((this.bPoint.X < endPoint.X) ? this.bPoint.X : endPoint.X, (this.bPoint.Y < endPoint.Y) ? this.bPoint.Y : endPoint.Y, Math.Abs(endPoint.X - this.bPoint.X), Math.Abs(endPoint.Y - this.bPoint.Y)));
                    bg.DrawEllipse(currentPen, new Rectangle((this.bPoint.X < endPoint.X) ? this.bPoint.X : endPoint.X, (this.bPoint.Y < endPoint.Y) ? this.bPoint.Y : endPoint.Y, Math.Abs(endPoint.X - this.bPoint.X), Math.Abs(endPoint.Y - this.bPoint.Y)));
                    break;
                default:
                    break;
            }

            //Рендеринг буфера в холст

            bgGraph.Render(this.canvas.CreateGraphics());
        }

        private void Fill(Point pos)
        {
            MyFill currentFill = null;
            currentFill = new MyFill(pos, this.colorChoicer.BColor);
            this.shapesList.Add(currentFill);

            this.UpdateShapeListComboBox();

            this.BufferToCanvas();
        }

        private void ChooseColor(Point pos, bool fore)
        {
            Graphics g = this.canvas.CreateGraphics();
            IntPtr hDc = g.GetHdc();
            uint clr = MyFill.GetPixel(hDc, pos.X, pos.Y);
            uint R = clr & 0xFF;
            uint G = (clr >> 8) & 0xFF;
            uint B = (clr >> 16) & 0xFF;
            Color currentColor = Color.FromArgb(255, (int)R, (int)G, (int)B);
            if (fore)
            {
                this.colorChoicer.FColor = currentColor;
            }
            else
            {
                this.colorChoicer.BColor = currentColor;
            }
            this.colorChoicer.Invalidate();
            g.ReleaseHdc();
        }

        private void BeginEditObject(Point pos)
        {
            IShape currentShape = this.GetCurrentEditedObject();

            //Режим редактирования а зависимости от положения над рамкой редкатирования

            //Фигуры

            if (currentShape is Shape)
            {
                for (int i = this.editBoxShapes.Count - 1; i >= 0; i--)
                {
                    Shape currentAnchorShape = this.editBoxShapes[i] as Shape;
                    Rectangle rect = currentAnchorShape.GetRectangle();
                    if (rect.Contains(pos))
                    {
                        if (i != 0)
                        {
                            this.editMode = (EDITMODE)(i);
                            break;
                        }
                    }
                }
            }

            //Заливки

            else if (currentShape is MyFill)
            {
                foreach (Shape shape in this.editBoxShapes)
                {
                    if (shape is MyRectangle)
                    {
                        if (((MyRectangle)shape).GetRectangle().Contains(pos))      //omg!
                        {
                            this.editMode = EDITMODE.MOVE;
                            break;
                        }
                    }
                }
            }

            //Карандаш

            else if(currentShape is MyPen)
            {
                foreach (Shape shape in this.editBoxShapes)
                {
                    if (shape is MyRectangle)
                    {
                        if (((MyRectangle)shape).GetRectangle().Contains(pos))      //omg!
                        {
                            this.editMode = EDITMODE.MOVE;
                            break;
                        }
                    }
                }
            }
        }

        private void EditedObject(Point pos)
        {
            //Текущий объект

            IShape currentShape = this.GetCurrentEditedObject();

            //Фигура

            if (currentShape is Shape)
            {
                Shape editedShape = currentShape as Shape;
                if (this.editMode != EDITMODE.NONE)
                {
                    Point oldBeginPoint = ((Shape)this.tempEditedShape).BeginPoint;
                    Point oldEndPoint = ((Shape)this.tempEditedShape).EndPoint;
                    editedShape.Editing(this.editMode, oldBeginPoint, oldEndPoint, pos);
                }
            }

            //Заливка

            else if (currentShape is MyFill)
            {
                if (this.editMode == EDITMODE.MOVE)
                {
                    ((MyFill)currentShape).FillPoint = pos;
                }
            }

            //Карандаш

            else if (currentShape is MyPen)
            {
                MyPen editedPen = currentShape as MyPen;
                if (this.editMode == EDITMODE.MOVE)
                {
                    Point center = Point.Empty;
                    MyRectangle rect = (MyRectangle)this.editBoxShapes[0];
                    center.X = (rect.BeginPoint.X + rect.EndPoint.X) / 2;
                    center.Y = (rect.BeginPoint.Y + rect.EndPoint.Y) / 2;
                    editedPen.Editing(this.editMode, center, pos);
                }
            }

            this.pointsOfMovement.Clear();
            this.CreateEditBoxShapes(currentShape);
            this.BufferToCanvas();
        }

        private void EndEditing(bool restore)
        {
            //Настройка интерфейса приложения

            this.isEdit = false;
            this.btn_edit.Enabled = true;
            this.btn_apply.Enabled = false;
            this.btn_cancel.Enabled = false;
            this.cb_shapeList.Enabled = true;

            //Получаем текущую фигуру

            IShape currentShape = this.GetCurrentEditedObject();

            //Фигура

            if (currentShape is Shape)
            {
                Shape myShape = currentShape as Shape;

                //Отключится от событий

                this.colorChoicer.FColorChanged -= myShape.SetForeColor;
                this.colorChoicer.BColorChanged -= myShape.SetBackColor;
                this.lineStyleChoicer.LineStyleChanged -= myShape.SetPenDashStyle;
                this.widthChiocer.LineWidthChanged -= myShape.SetPenWidth;
                this.hatchChoiser.HatchStyleChanged -= myShape.SetBrushStyle;
            }

            //Заливка (феил в архитектуре?)

            else if (currentShape is MyFill)
            {
                MyFill myFill = currentShape as MyFill;
                this.colorChoicer.FColorChanged -= myFill.SetColor;
            }

            else if (currentShape is MyPen)
            {
                MyPen myPen = currentShape as MyPen;
                this.colorChoicer.FColorChanged -= myPen.SetColor;
                this.widthChiocer.LineWidthChanged -= myPen.SetWidth;
            }

            //Восстановить фигуру , если отмена

            if (restore)
            {
                int selectedIndex = this.cb_shapeList.SelectedIndex;
                this.shapesList[selectedIndex] = this.tempShape;
            }

            //Обновление/очистка

            this.editBoxShapes.Clear();
            this.BufferToCanvas();
        }

        private void CreateEditBoxShapes(IShape s)
        {
            this.editBoxShapes.Clear();

            int size = 4;                           //размер якоря
            int radius = 15;                        //радиус для круга заливки

            //Инструменты рисования якорей

            Pen smPen = new Pen(Color.Black);
            Brush smBrush = Brushes.LightGreen;

            //Инструменты рисования рамки

            Pen pen = new Pen(Color.Black, 2);
            pen.DashStyle = DashStyle.Dash;
           
            if (s is Shape)
            {
                Shape shape = s as Shape;

                //Точки по углам

                Point LeftTop = shape.BeginPoint;
                Point RightTop = new Point(shape.EndPoint.X, shape.BeginPoint.Y);
                Point LeftBottom = new Point(shape.BeginPoint.X, shape.EndPoint.Y);
                Point RightBottom = shape.EndPoint;

                //Точки в серединах рёбер

                Point MiddleTop = new Point((LeftTop.X + RightTop.X) / 2, LeftTop.Y);
                Point MiddleBottom = new Point((LeftBottom.X + RightBottom.X) / 2, LeftBottom.Y);
                Point MiddleLeft = new Point(LeftTop.X, (LeftTop.Y + LeftBottom.Y) / 2);
                Point MiddleRight = new Point(RightTop.X, (RightTop.Y + RightBottom.Y) / 2);

                //Центральная точка

                Point Center = new Point(MiddleTop.X, MiddleRight.Y);

                //Основная рамка(Пунктир)

                MyRectangle editRect = new MyRectangle(LeftTop, RightBottom);
                editRect.Pen = pen;
                this.editBoxShapes.Add(editRect);

                //Якоря по углам и середине (чёрная рамка, заливка)

                //Массив точек

                Point[] editPoints = { LeftTop, MiddleTop, RightTop, MiddleLeft, Center, MiddleRight, LeftBottom, MiddleBottom, RightBottom };

                MyRectangle smallEditRect = null;

                foreach (Point point in editPoints)
                {
                    smallEditRect = new MyRectangle(new Point(point.X - size, point.Y - size), new Point(point.X + size, point.Y + size), true);
                    smallEditRect.Pen = smPen;
                    smallEditRect.Brush = smBrush;
                    this.editBoxShapes.Add(smallEditRect);
                }
            }

            // Заливка

            else if (s is MyFill)
            {
                MyFill fill = s as MyFill;

                //Основная окружность(Пунктир)

                MyEllipse editCircle = new MyEllipse(new Point(fill.FillPoint.X - radius, fill.FillPoint.Y - radius), new Point(fill.FillPoint.X + radius, fill.FillPoint.Y + radius));
                editCircle.Pen = pen;
                this.editBoxShapes.Add(editCircle);

                //Центральный прямоугольник

                MyRectangle smallEditRect = new MyRectangle(new Point(fill.FillPoint.X - size, fill.FillPoint.Y - size), new Point(fill.FillPoint.X + size, fill.FillPoint.Y + size), true);
                smallEditRect.Pen = smPen;
                smallEditRect.Brush = smBrush;
                this.editBoxShapes.Add(smallEditRect);
            }

            //Карандаш

            else if (s is MyPen)
            {
                MyPen myPen = s as MyPen;
                
                //Крайние координаты

                int xMin = this.canvas.Width;
                int xMax = 0;
                int yMin = this.canvas.Height;
                int yMax = 0;

                foreach (Point point in myPen.Points)
                {
                    if (xMin > point.X)
                    {
                        xMin = point.X;
                    }
                    if (xMax < point.X)
                    {
                        xMax = point.X;
                    }
                    if (yMin > point.Y)
                    {
                        yMin = point.Y;
                    }
                    if (yMax < point.Y)
                    {
                        yMax = point.Y;
                    }
                }

                MyRectangle editRect = new MyRectangle(new Point(xMin, yMin), new Point(xMax, yMax));
                editRect.Pen = pen;
                this.editBoxShapes.Add(editRect);

                //Центральный прямоугольник

                int xCenter = (xMin + xMax) / 2;
                int yCenter = (yMin + yMax) / 2;

                MyRectangle smallEditRect = new MyRectangle(new Point(xCenter - size, yCenter - size), new Point(xCenter + size, yCenter + size), true);
                smallEditRect.Pen = smPen;
                smallEditRect.Brush = smBrush;
                this.editBoxShapes.Add(smallEditRect);
            }
        }

        #endregion

        #region Хелперы

        private void DrawShapesToBuffer()
        {
            //Поместить изображение в буфер для изображений

            if (this.bgImage != null)
            {
                Graphics bgImage = this.bgImage.Graphics;
                if (this.currentImage != null && this.isOpenedFile == false)
                {
                    bgImage.DrawImage(this.currentImage, 0, 0);
                    this.isOpenedFile = true;
                }
            }

            //Вывод фигур и изображения в буфер

            Graphics bgShape = this.bgGraph.Graphics;

            if (this.cb_antiAlias.Checked == true)
            {
                bgShape.SmoothingMode = SmoothingMode.AntiAlias;
            }
            else
            {
                bgShape.SmoothingMode = SmoothingMode.None;
            }

            if (this.currentImage != null)
            {
                this.bgImage.Render(bgShape);
            }
            else
            {
                bgShape.Clear(Color.White);
            }
            
            //Вывод фигур

            foreach (IShape shape in this.shapesList)
            {
                if (shape is Shape || shape is MyPen)
                {
                    shape.Draw(bgShape);
                }
            }

            //Вывод заливок

            foreach (IShape shape in this.shapesList)
            {
                if (shape is MyFill)
                {
                    shape.Draw(bgShape);
                }
            }

            //Вывод прямоугольника для редактирования

            if (this.isEdit && this.editBoxShapes.Count != 0)
            {
                foreach (IShape shape in this.editBoxShapes)
                {
                    shape.Draw(bgShape);
                }
            }
        }

        private void BufferToCanvas()
        {
            this.DrawShapesToBuffer();
            this.bgGraph.Render(this.canvas.CreateGraphics());
        }

        private void SetCanvasCursor(Point pos)
        {
            //Режим редактирования

            if (this.isEdit)
            {
                this.canvas.Cursor = Cursors.Default;

                //Получаем текущую фигуру

                IShape currentShape = this.GetCurrentEditedObject();

                //Для фигуры

                if (currentShape is Shape)
                {
                    for (int i = this.editBoxShapes.Count - 1; i >= 0; i--)
                    {
                        Shape currentAnchorShape = this.editBoxShapes[i] as Shape;
                        Rectangle rect = currentAnchorShape.GetRectangle();
                        if (rect.Contains(pos))
                        {
                            if (i == 1 || i == 9)
                            {
                                this.canvas.Cursor = Cursors.SizeNWSE;
                            }
                            else if (i == 3 || i == 7)
                            {
                                this.canvas.Cursor = Cursors.SizeNESW;
                            }
                            else if (i == 4 || i == 6)
                            {
                                this.canvas.Cursor = Cursors.SizeWE;
                            }
                            else if (i == 2 || i == 8)
                            {
                                this.canvas.Cursor = Cursors.SizeNS;
                            }
                            else if (i == 5)
                            {
                                this.canvas.Cursor = Cursors.SizeAll;
                            }
                            break;
                        }
                    }
                }

                //Для заливки

                else if (currentShape is MyFill)
                {
                    foreach (Shape shape in this.editBoxShapes)
                    {
                        if (shape is MyRectangle)
                        {
                            if (((MyRectangle)shape).GetRectangle().Contains(pos))      //omg!
                            {
                                this.canvas.Cursor = Cursors.SizeAll;
                            }
                        }
                    }
                }
                else if (currentShape is MyPen)
                {
                    for (int i = 1; i < this.editBoxShapes.Count; i++)
                    {
                        if (((MyRectangle)this.editBoxShapes[i]).GetRectangle().Contains(pos))      //omg!
                        {
                            this.canvas.Cursor = Cursors.SizeAll;
                        }
                    }
                }
            }

            //Режим рисования

            else
            {
                switch (this.typeTool)
                {
                    case SHAPES.PEN:
                        this.canvas.Cursor = Cursors.Cross;
                        break;
                    case SHAPES.LINE:
                        this.canvas.Cursor = Cursors.Cross;
                        break;
                    case SHAPES.RECTANGLE:
                        this.canvas.Cursor = Cursors.Cross;
                        break;
                    case SHAPES.FILLRECTANGLE:
                        this.canvas.Cursor = Cursors.Cross;
                        break;
                    case SHAPES.ELLIPS:
                        this.canvas.Cursor = Cursors.Cross;
                        break;
                    case SHAPES.FILLELLIPS:
                        this.canvas.Cursor = Cursors.Cross;
                        break;
                    case SHAPES.FILLTOOL:
                        this.canvas.Cursor = Cursors.Hand;
                        break;
                    case SHAPES.COLORCHOICE:
                        this.canvas.Cursor = Cursors.Hand;
                        this.ZoomForColorChoice(pos);
                        break;
                    case SHAPES.ZOOM:
                        this.canvas.Cursor = Cursors.Hand;
                        break;
                    default:
                        break;
                }
            }
        }

        private IShape GetCurrentEditedObject()
        {
            if (this.cb_shapeList.SelectedIndex == -1 || this.shapesList.Count == 0)
            {
                return null;
            }
            return this.shapesList[this.cb_shapeList.SelectedIndex];
        }

        private void ResetState(bool clearImage)
        {
            this.isEdit = false;
            this.isOpenedFile = false;
            this.isZoom = false;
            this.isHide = false;

            this.cb_shapeList.DataSource = null;

            if (clearImage)
            {
                this.currentImage = null;
            }

            this.tempShape = null;
            this.tempEditedShape = null;

            this.shapesList.Clear();
            this.editBoxShapes.Clear();
        }

        private void UpdateShapeListComboBox()
        {
            this.cb_shapeList.DataSource = null;
            this.cb_shapeList.DataSource = this.shapesList;
            this.cb_shapeList.SelectedIndex = this.shapesList.Count - 1;
        }

        #endregion
    }
}