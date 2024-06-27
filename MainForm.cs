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
        private List<IElement> draft;

        public MainForm()
        {
            InitializeComponent();
            gateInfos = new GateInfo[9] { Elements.Gates.Buffer.GetInfo(), NOT.GetInfo(), AND.GetInfo(),
                OR.GetInfo(), NAND.GetInfo(), NOR.GetInfo(), XOR.GetInfo(), XNOR.GetInfo(), IMPLY.GetInfo()};
            draft = new List<IElement>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void Render(Graphics g = null)
        {
            if (g == null) g = panelCanvas.CreateGraphics();

            g.Clear(Color.LightGray);

            int width = panelCanvas.Width, height = panelCanvas.Height;

            for (int i = -2; i < height / 30 + 2; i++)
            {
                for (int j = -2; j < width / 30 + 2; j++)
                {
                    g.FillRectangle(Brushes.Black, j * 30 - 12, i * 30 - 18, 2, 1);
                }
            }

            for (int i = 0; i < draft.Count;i++)
            {
                g.DrawImage(draft[i].Diagram, draft[i].Location.X, draft[i].Location.Y, 70, 40); //coef 0.575
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

        private void AddGate(int tag)
        {
            IGate gate = null;
            switch (tag)
            {
                case 1:
                    gate = new Elements.Gates.Buffer();
                    break;
                case 2:
                    gate = new NOT();
                    break;
                case 3:
                    gate = new AND();
                    break;
                case 4:
                    gate = new OR();
                    break;
                case 5:
                    gate = new NAND();
                    break;
                case 6:
                    gate = new NOR();
                    break;
                case 7: 
                    gate = new XOR();
                    break;
                case 8:
                    gate = new XNOR();
                    break;
                case 9:
                    gate = new IMPLY();
                    break;
            }

            int width = panelCanvas.Width, height = panelCanvas.Height;
            gate.Location = new Point(width/2, height/2);

            draft.Add(gate);
            Render();
        }

        private void GatesToolsClicked(object sender, EventArgs e)
        {
            int tag = int.Parse((sender as Control).Tag.ToString());
            AddGate(tag);
        }

        private void MenuButtonsMouseEnter(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = Color.LightGray;

            object tag = (sender as Control).Tag;
            if (tag != null && int.TryParse(tag.ToString(), out int gate))
            {
                labelGateName.Text = gateInfos[gate - 1].Name;
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
