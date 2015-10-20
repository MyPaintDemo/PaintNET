using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Paint
{
    public partial class NewImageDialog : Form
    {
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        private bool isValid = true;

        public NewImageDialog()
        {
            InitializeComponent();

            this.FormClosing += NewImageDialog_FormClosing;
        }

        void NewImageDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (this.isValid)
                {
                    this.ImageWidth = Int32.Parse(this.tb_width.Text);
                    this.ImageHeight = Int32.Parse(this.tb_heigth.Text);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void SizeValidation(object sender, CancelEventArgs e)
        {
            if (!(sender is TextBox)) { return; }
            
            Regex sizeReg = new Regex(@"^\d{1,4}$");
            string errorMessage = "Некорректный размер изображения";
            TextBox currentTextBox = sender as TextBox;

            if (!sizeReg.IsMatch(currentTextBox.Text))
            {
                this.errorProvider1.SetError(currentTextBox, errorMessage);
                this.isValid = false;
            }
            else
            {
                this.isValid = true;
                this.errorProvider1.SetError(currentTextBox, "");
                switch (currentTextBox.Name)
	            {
                    case "tb_width":
                        this.ImageWidth = Int32.Parse(this.tb_width.Text);
                        break;
                    case "tb_heigth":
                        this.ImageHeight = Int32.Parse(this.tb_heigth.Text);
                        break;
		            default:
                        break;
	            }
            }
        }
    }
}