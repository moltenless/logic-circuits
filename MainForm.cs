using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LogicCircuits
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }
        
        private void MenuButtonsMouseEnter(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = Color.LightGray;

            Graphics g = panelCanvas.CreateGraphics();
            g.DrawImage(Image.FromFile("D:\\Chrome media\\120px-NOT_ANSI_Labelled.svg.png"), 200, 200);
        }

        private void MenuButtonsMouseLeave(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = SystemColors.Control;
        }
    }
}
