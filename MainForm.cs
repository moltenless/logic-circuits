using LogicCircuits.Elements;
using LogicCircuits.Elements.Gates;
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
        private GateInfo[] gateInfos;

        public MainForm()
        {
            InitializeComponent();
            gateInfos = new GateInfo[9] { Elements.Gates.Buffer.GetInfo(), NOT.GetInfo(), AND.GetInfo(), 
                OR.GetInfo(), NAND.GetInfo(), NOR.GetInfo(), XOR.GetInfo(), XNOR.GetInfo(), IMPLY.GetInfo()};
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void Render(Graphics g = null)
        {
            if (g == null) g = panelCanvas.CreateGraphics();

            int width = panelCanvas.Width, height = panelCanvas.Height;

            for (int i = -2; i < height / 30 + 2; i++)
            {
                for (int j = -2; j < width / 30 + 2; j++)
                {
                    g.FillRectangle(Brushes.Black, j * 30 - 12, i * 30 - 12, 2, 1);
                }
            }
        }

        private void panelCanvas_Paint(object sender, PaintEventArgs e)
        {
            Render(e.Graphics);
        }

        private void panelCanvas_SizeChanged(object sender, EventArgs e)
        {
            panelCanvas.Refresh();
        }

        private void MenuButtonsMouseEnter(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = Color.LightGray;

            object tag = (sender as Control).Tag;
            if (tag != null && int.TryParse(tag.ToString(), out int gate))
            {
                labelGateName.Text = gateInfos[gate-1].Name;
                pictureBoxFormula.Image = gateInfos[gate - 1].Formula;
                pictureBoxDiagram.Image = gateInfos[gate - 1].Diagram;
                pictureBoxGateTable.Image = gateInfos[gate - 1].TruthTable;
            }
        }

        private void MenuButtonsMouseLeave(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = SystemColors.Control;

            labelGateName.Text = "<Назва вентиля>";
            if (pictureBoxFormula.Image != null) pictureBoxFormula.Image = null;
            if (pictureBoxDiagram != null) pictureBoxDiagram.Image = null;
            if (pictureBoxGateTable != null) pictureBoxGateTable.Image = null;
        }
    }
}
