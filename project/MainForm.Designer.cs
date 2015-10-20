namespace Paint
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_new = new System.Windows.Forms.ToolStripButton();
            this.tsb_open = new System.Windows.Forms.ToolStripButton();
            this.tsb_save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_undo = new System.Windows.Forms.ToolStripButton();
            this.tsb_redo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_pen = new System.Windows.Forms.ToolStripButton();
            this.tsb_line = new System.Windows.Forms.ToolStripButton();
            this.tsb_rect = new System.Windows.Forms.ToolStripButton();
            this.tlb_fillrect = new System.Windows.Forms.ToolStripButton();
            this.tlb_elips = new System.Windows.Forms.ToolStripButton();
            this.tlb_fillelips = new System.Windows.Forms.ToolStripButton();
            this.tsb_text = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tlb_fill = new System.Windows.Forms.ToolStripButton();
            this.tlb_colorChoicer = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cb_shapeList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_edit = new System.Windows.Forms.Button();
            this.btn_apply = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssl_size = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_tool = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_position = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_new,
            this.tsb_open,
            this.tsb_save,
            this.toolStripSeparator1,
            this.tsb_undo,
            this.tsb_redo,
            this.toolStripSeparator4,
            this.tsb_pen,
            this.tsb_line,
            this.tsb_rect,
            this.tlb_fillrect,
            this.tlb_elips,
            this.tlb_fillelips,
            this.tsb_text,
            this.toolStripSeparator3,
            this.tlb_fill,
            this.tlb_colorChoicer,
            this.toolStripSeparator5});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1167, 38);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_new
            // 
            this.tsb_new.AutoSize = false;
            this.tsb_new.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_new.Image = global::Paint.Properties.Resources.newfile;
            this.tsb_new.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_new.Name = "tsb_new";
            this.tsb_new.Size = new System.Drawing.Size(35, 35);
            this.tsb_new.Text = "Новый";
            this.tsb_new.Click += new System.EventHandler(this.tsb_new_Click);
            // 
            // tsb_open
            // 
            this.tsb_open.AutoSize = false;
            this.tsb_open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_open.Image = global::Paint.Properties.Resources.open;
            this.tsb_open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_open.Name = "tsb_open";
            this.tsb_open.Size = new System.Drawing.Size(35, 35);
            this.tsb_open.Text = "Открыть";
            this.tsb_open.Click += new System.EventHandler(this.tsb_open_Click);
            // 
            // tsb_save
            // 
            this.tsb_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_save.Image = global::Paint.Properties.Resources.save;
            this.tsb_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_save.Name = "tsb_save";
            this.tsb_save.Size = new System.Drawing.Size(24, 35);
            this.tsb_save.Text = "Сохранить";
            this.tsb_save.Click += new System.EventHandler(this.tsl_save_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // tsb_undo
            // 
            this.tsb_undo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_undo.Image = global::Paint.Properties.Resources.undo;
            this.tsb_undo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_undo.Name = "tsb_undo";
            this.tsb_undo.Size = new System.Drawing.Size(24, 35);
            this.tsb_undo.Text = "Отменить";
            this.tsb_undo.Click += new System.EventHandler(this.tsb_undo_Click);
            // 
            // tsb_redo
            // 
            this.tsb_redo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_redo.Image = global::Paint.Properties.Resources.redo;
            this.tsb_redo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_redo.Name = "tsb_redo";
            this.tsb_redo.Size = new System.Drawing.Size(24, 35);
            this.tsb_redo.Text = "Вернуть";
            this.tsb_redo.Click += new System.EventHandler(this.tsb_redo_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 38);
            // 
            // tsb_pen
            // 
            this.tsb_pen.AutoSize = false;
            this.tsb_pen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_pen.Image = global::Paint.Properties.Resources.pencil;
            this.tsb_pen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_pen.Name = "tsb_pen";
            this.tsb_pen.Size = new System.Drawing.Size(35, 35);
            this.tsb_pen.Text = "Карандаш";
            this.tsb_pen.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // tsb_line
            // 
            this.tsb_line.AutoSize = false;
            this.tsb_line.CheckOnClick = true;
            this.tsb_line.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_line.Image = global::Paint.Properties.Resources.line;
            this.tsb_line.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_line.Name = "tsb_line";
            this.tsb_line.Size = new System.Drawing.Size(35, 35);
            this.tsb_line.Text = "Прямая";
            this.tsb_line.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // tsb_rect
            // 
            this.tsb_rect.AutoSize = false;
            this.tsb_rect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_rect.Image = global::Paint.Properties.Resources.rectangle;
            this.tsb_rect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_rect.Name = "tsb_rect";
            this.tsb_rect.Size = new System.Drawing.Size(35, 35);
            this.tsb_rect.Text = "Прямоугольник";
            this.tsb_rect.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // tlb_fillrect
            // 
            this.tlb_fillrect.AutoSize = false;
            this.tlb_fillrect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlb_fillrect.Image = global::Paint.Properties.Resources.fillrectangle;
            this.tlb_fillrect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlb_fillrect.Name = "tlb_fillrect";
            this.tlb_fillrect.Size = new System.Drawing.Size(35, 35);
            this.tlb_fillrect.Text = "Заполненный прямоугольник";
            this.tlb_fillrect.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // tlb_elips
            // 
            this.tlb_elips.AutoSize = false;
            this.tlb_elips.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlb_elips.Image = global::Paint.Properties.Resources.ellipse;
            this.tlb_elips.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlb_elips.Name = "tlb_elips";
            this.tlb_elips.Size = new System.Drawing.Size(35, 35);
            this.tlb_elips.Text = "Эллипс";
            this.tlb_elips.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // tlb_fillelips
            // 
            this.tlb_fillelips.AutoSize = false;
            this.tlb_fillelips.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlb_fillelips.Image = global::Paint.Properties.Resources.fillellipse;
            this.tlb_fillelips.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlb_fillelips.Name = "tlb_fillelips";
            this.tlb_fillelips.Size = new System.Drawing.Size(35, 35);
            this.tlb_fillelips.Text = "Заполненный эллипс";
            this.tlb_fillelips.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // tsb_text
            // 
            this.tsb_text.AutoSize = false;
            this.tsb_text.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_text.Enabled = false;
            this.tsb_text.Image = global::Paint.Properties.Resources.text;
            this.tsb_text.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_text.Name = "tsb_text";
            this.tsb_text.Size = new System.Drawing.Size(35, 35);
            this.tsb_text.Text = "Текст";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 38);
            // 
            // tlb_fill
            // 
            this.tlb_fill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlb_fill.Image = global::Paint.Properties.Resources.fill;
            this.tlb_fill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlb_fill.Name = "tlb_fill";
            this.tlb_fill.Size = new System.Drawing.Size(24, 35);
            this.tlb_fill.Text = "Заливка";
            this.tlb_fill.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // tlb_colorChoicer
            // 
            this.tlb_colorChoicer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlb_colorChoicer.Image = global::Paint.Properties.Resources.chooser;
            this.tlb_colorChoicer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlb_colorChoicer.Name = "tlb_colorChoicer";
            this.tlb_colorChoicer.Size = new System.Drawing.Size(24, 35);
            this.tlb_colorChoicer.Text = "Выбор цвета";
            this.tlb_colorChoicer.Click += new System.EventHandler(this.ToolButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 65);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1143, 525);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(225, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(914, 517);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(214, 517);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.Controls.Add(this.cb_shapeList, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(208, 29);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // cb_shapeList
            // 
            this.cb_shapeList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_shapeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_shapeList.FormattingEnabled = true;
            this.cb_shapeList.Location = new System.Drawing.Point(65, 4);
            this.cb_shapeList.Name = "cb_shapeList";
            this.cb_shapeList.Size = new System.Drawing.Size(140, 21);
            this.cb_shapeList.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Фигуры";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Controls.Add(this.btn_edit, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btn_apply, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.btn_cancel, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.btn_delete, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 38);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(208, 29);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // btn_edit
            // 
            this.btn_edit.BackgroundImage = global::Paint.Properties.Resources.edit_btn;
            this.btn_edit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_edit.Enabled = false;
            this.btn_edit.Location = new System.Drawing.Point(55, 3);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(46, 23);
            this.btn_edit.TabIndex = 0;
            this.btn_edit.UseVisualStyleBackColor = true;
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // btn_apply
            // 
            this.btn_apply.BackgroundImage = global::Paint.Properties.Resources.apply_btn;
            this.btn_apply.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_apply.Enabled = false;
            this.btn_apply.Location = new System.Drawing.Point(107, 3);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(46, 23);
            this.btn_apply.TabIndex = 1;
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.BackgroundImage = global::Paint.Properties.Resources.cancel_btn;
            this.btn_cancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_cancel.Enabled = false;
            this.btn_cancel.Location = new System.Drawing.Point(159, 3);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(46, 23);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.BackgroundImage = global::Paint.Properties.Resources.delete_btn;
            this.btn_delete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_delete.Enabled = false;
            this.btn_delete.Location = new System.Drawing.Point(3, 3);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(46, 23);
            this.btn_delete.TabIndex = 3;
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1167, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.newToolStripMenuItem.Text = "New...";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(109, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl_size,
            this.tssl_tool,
            this.tssl_position});
            this.statusStrip1.Location = new System.Drawing.Point(0, 593);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1167, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssl_size
            // 
            this.tssl_size.AutoSize = false;
            this.tssl_size.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tssl_size.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedInner;
            this.tssl_size.Name = "tssl_size";
            this.tssl_size.Size = new System.Drawing.Size(200, 17);
            this.tssl_size.Text = "Size:";
            // 
            // tssl_tool
            // 
            this.tssl_tool.AutoSize = false;
            this.tssl_tool.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tssl_tool.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedInner;
            this.tssl_tool.Name = "tssl_tool";
            this.tssl_tool.Size = new System.Drawing.Size(200, 17);
            this.tssl_tool.Text = "Tool";
            // 
            // tssl_position
            // 
            this.tssl_position.AutoSize = false;
            this.tssl_position.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tssl_position.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedInner;
            this.tssl_position.Name = "tssl_position";
            this.tssl_position.Size = new System.Drawing.Size(200, 17);
            this.tssl_position.Text = "Position";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 38);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 615);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "Paint .Net v0.2";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_new;
        private System.Windows.Forms.ToolStripButton tsb_open;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_line;
        private System.Windows.Forms.ToolStripButton tsb_rect;
        private System.Windows.Forms.ToolStripButton tlb_fillrect;
        private System.Windows.Forms.ToolStripButton tlb_elips;
        private System.Windows.Forms.ToolStripButton tlb_fillelips;
        private System.Windows.Forms.ToolStripButton tsb_save;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tssl_size;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_shapeList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ToolStripButton tlb_fill;
        private System.Windows.Forms.ToolStripButton tlb_colorChoicer;
        private System.Windows.Forms.ToolStripStatusLabel tssl_tool;
        private System.Windows.Forms.ToolStripButton tsb_undo;
        private System.Windows.Forms.ToolStripButton tsb_redo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.ToolStripStatusLabel tssl_position;
        private System.Windows.Forms.ToolStripButton tsb_pen;
        private System.Windows.Forms.ToolStripButton tsb_text;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}

