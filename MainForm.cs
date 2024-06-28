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
            Render();
        }
        

        private void Render(Graphics g = null)
        {
            if (g == null) g = panelCanvas.CreateGraphics();

            g.Clear(Color.LightGray);
            panelCanvas.Controls.Clear();

            int width = panelCanvas.Width, height = panelCanvas.Height;

            for (int i = -2; i < height / 30 + 2; i++)
            {
                for (int j = -2; j < width / 30 + 2; j++)
                {
                    g.FillRectangle(Brushes.Black, j * 30 - 12, i * 30 - 18, 2, 1);
                }
            }

            int gateWidth = 70, gateHeight = 40;//coef 0.575
            for (int i = 0; i < draft.Count; i++)
            {
                g.DrawImage(draft[i].Diagram, draft[i].Location.X - gateWidth / 2, draft[i].Location.Y - gateHeight / 2, gateWidth, gateHeight);

                Button removeButton = new Button
                {
                    Tag = draft[i],
                    Size = new Size(10, 10),
                    BackgroundImage = Properties.Resources.close,
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Location = new Point(draft[i].Location.X - gateWidth / 4, draft[i].Location.Y - 4 * gateHeight / 5),
                    FlatStyle = FlatStyle.Flat,
                };
                toolTipMenu.SetToolTip(removeButton, "Видалити вентиль");
                removeButton.Click += (sender, e) =>
                {
                    draft.Remove((sender as Control).Tag as IElement);
                    elementMoveable = false;
                    Cursor = Cursors.Default;
                    Render();
                };
                panelCanvas.Controls.Add(removeButton);

                Button moveButton = new Button
                {
                    Tag = draft[i],
                    Size = new Size(10, 10),
                    BackgroundImage = Properties.Resources.move,
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Location = new Point(draft[i].Location.X - 2 * gateWidth / 5, draft[i].Location.Y - 4 * gateHeight / 5),
                    FlatStyle = FlatStyle.Flat,
                };
                toolTipMenu.SetToolTip(moveButton, "Перемістити вентиль");
                moveButton.Click += (sender, e) =>
                {
                    if (!elementMoveable)
                    {
                        elementMoveable = true;
                        moveableElement = (sender as Control).Tag as IElement;
                        Cursor = Cursors.NoMove2D;
                    }
                    else
                    {
                        if ((sender as Control).Tag as IElement == moveableElement)
                        {
                            elementMoveable = false;
                            Cursor = Cursors.Default;
                        }
                        else
                        {
                            moveableElement = (sender as Control).Tag as IElement;
                        }
                    }
                };
                panelCanvas.Controls.Add(moveButton);
            }
        }

        private void PanelCanvasClick(object sender, EventArgs e)
        {
            if (gateSelected)
            {
                gateSelected = false;
                Cursor = Cursors.Default;
                for (int i = 0; i < panelGates.Controls.Count; i++)
                    if (panelGates.Controls[i].Controls[0].Tag.ToString() == gateTag.ToString())
                    {
                        panelGates.Controls[i].BackColor = SystemColors.Control; break;
                    }
                AddGate(gateTag);
            }
            if (elementMoveable)
            {
                elementMoveable = false;
                Cursor = Cursors.Default;
                moveableElement.Location = panelCanvas.PointToClient(Cursor.Position);
                Render();
            }
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
            gate.Location = panelCanvas.PointToClient(Cursor.Position);

            draft.Add(gate);
            Render();
        }

        private bool gateSelected = false;
        private int gateTag = -1;

        private bool elementMoveable = false;
        private IElement moveableElement = null;
        private void GatesToolsClicked(object sender, EventArgs e)
        {
            int current = int.Parse((sender as Control).Tag.ToString());

            if (!gateSelected)
            {
                gateSelected = true;
                gateTag = current;
                Cursor = Cursors.Cross;
                (sender as Control).Parent.BackColor = Color.LightGray;
            }
            else
            {
                if (current == gateTag)
                {
                    gateSelected = false;
                    Cursor = Cursors.Default;
                    (sender as Control).Parent.BackColor = SystemColors.Control;
                }
                else
                {
                    for (int i = 0; i < panelGates.Controls.Count; i++)
                        if (panelGates.Controls[i].Controls[0].Tag.ToString() == gateTag.ToString())
                        {
                            panelGates.Controls[i].BackColor = SystemColors.Control; break;
                        }
                    gateTag = current;
                    (sender as Control).Parent.BackColor = Color.LightGray;
                }
            }
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
            if (!(gateSelected && (sender as Control).Tag.ToString() == gateTag.ToString()))
                (sender as Control).Parent.BackColor = SystemColors.Control;

            labelGateName.Text = "<Назва вентиля>";
            if (pictureBoxFormula.Image != null) pictureBoxFormula.Image = null;
            if (pictureBoxDiagram != null) pictureBoxDiagram.Image = null;
            if (pictureBoxGateTable != null) pictureBoxGateTable.Image = null;
        }

        protected override void WndProc(ref Message m) //prevents redrawing caused by alt
        {
            if (m.Msg == 0x128) return;
            base.WndProc(ref m);
        }
    }
}
