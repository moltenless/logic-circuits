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
            object tag = (sender as Control).Tag;
            if (tag != null && int.TryParse(tag.ToString(), out int gate))
            {
                string[]
            }
        }

        private void MenuButtonsMouseLeave(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = SystemColors.Control;
        }
    }
}
