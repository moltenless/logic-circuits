using LogicCircuits.Elements;
using LogicCircuits.Elements.Gates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
            int signalWidth = 30, signalHeight = 30;
            for (int i = 0; i < draft.Count; i++)
            {
                if (draft[i] is IGate)
                    g.DrawImage(draft[i].Diagram, draft[i].Location.X - gateWidth / 2, draft[i].Location.Y - gateHeight / 2, gateWidth, gateHeight);
                else
                    g.DrawImage(draft[i].Diagram, draft[i].Location.X - signalWidth / 2, draft[i].Location.Y - signalHeight / 2, signalWidth, signalHeight);

                if (draft[i] is Input input)
                {
                    g.DrawString(input.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(input.Location.X - 12, input.Location.Y - 3 * signalHeight / 2));
                }
                if (draft[i] is Output output)
                {
                    g.DrawString(output.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(output.Location.X + 14, output.Location.Y - 8));
                }

                PictureBox removeButton = new PictureBox
                {
                    Tag = draft[i],
                    Size = new Size(10, 10),
                    Image = Properties.Resources.close,
                    SizeMode = PictureBoxSizeMode.Zoom,
                };
                removeButton.Location = draft[i] is IGate ? new Point(draft[i].Location.X - gateWidth / 4, draft[i].Location.Y - 4 * gateHeight / 5)
                    : new Point(draft[i].Location.X, draft[i].Location.Y - 7 * signalHeight / 8);
                toolTipMenu.SetToolTip(removeButton, "Видалити вентиль");
                removeButton.Click += (sender, e) =>
                {
                    IElement curr = (sender as Control).Tag as IElement;
                    if (curr is IOutputContainingElement currOut)
                    {
                        if (currOut.Output != null)
                        {
                            currOut.Output.Inputs.Remove(currOut);
                            currOut.Output = null;
                        }
                    }
                    if (curr is IInputContainingElement currIn)
                    {
                        for (int k = 0; k < currIn.Inputs.Count; k++)
                        {
                            currIn.Inputs[k].Output = null;
                            currIn.Inputs.Remove(currIn.Inputs[k]);
                        }
                    }
                    draft.Remove(curr);
                    panelParams.Controls[0].Enabled = true;
                    elementMoveable = false;
                    Cursor = Cursors.Default;
                    Render();
                };
                panelCanvas.Controls.Add(removeButton);


                PictureBox moveButton = new PictureBox
                {
                    Tag = draft[i],
                    Size = new Size(10, 10),
                    Image = Properties.Resources.move,
                    SizeMode = PictureBoxSizeMode.Zoom,
                };
                moveButton.Location = draft[i] is IGate ? new Point(draft[i].Location.X - 2 * gateWidth / 5, draft[i].Location.Y - 4 * gateHeight / 5)
                    : new Point(draft[i].Location.X - signalWidth / 3, draft[i].Location.Y - 7 * signalHeight / 8);
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


                PictureBox connectButton = new PictureBox
                {
                    Tag = draft[i],
                    Size = new Size(15, 15),
                    Image = Properties.Resources.connect,
                    SizeMode = PictureBoxSizeMode.Zoom,
                };
                Point connLocation = new Point();
                if (draft[i] is AND || draft[i] is NAND)
                    connLocation = new Point(draft[i].Location.X - 2 * gateWidth / 6, draft[i].Location.Y - 1 * gateHeight / 5);
                else if (draft[i] is OR || draft[i] is NOR)
                    connLocation = new Point(draft[i].Location.X - 2 * gateWidth / 7, draft[i].Location.Y - 1 * gateHeight / 5);
                else if (draft[i] is XOR || draft[i] is XNOR)
                    connLocation = new Point(draft[i].Location.X - 2 * gateWidth / 9, draft[i].Location.Y - 1 * gateHeight / 5);
                else if (draft[i] is IMPLY)
                    connLocation = new Point(draft[i].Location.X - 2 * gateWidth / 8, draft[i].Location.Y - 1 * gateHeight / 5);
                else if (draft[i] is Elements.Gates.Buffer || draft[i] is NOT)
                    connLocation = new Point(draft[i].Location.X - 2 * gateWidth / 5, draft[i].Location.Y - 1 * gateHeight / 5);
                else if (draft[i] is Input)
                    connLocation = new Point(draft[i].Location.X - 6 * signalWidth / 5, draft[i].Location.Y - 2 * signalHeight / 7);
                else if (draft[i] is Output)
                    connLocation = new Point(draft[i].Location.X - 2 * signalWidth / 7, draft[i].Location.Y + 2 * signalHeight / 3);


                connectButton.Location = connLocation;
                toolTipMenu.SetToolTip(connectButton, "Приєднати вентиль або вихідний сигнал");
                connectButton.Click += (sender, e) =>
                {
                    if (!elementConnectable)
                    {
                        if ((sender as Control).Tag is IOutputContainingElement curr)
                        {
                            if (curr.Output != null)
                            {
                                curr.Output.Inputs.Remove(curr);
                                curr.Output = null;
                                Render();
                                return;
                            }
                        }
                        elementConnectable = true;
                        connectableElement = (sender as Control).Tag as IElement;
                        Cursor = Cursors.Hand;
                    }
                    else
                    {
                        elementConnectable = false;
                        Cursor = Cursors.Default;

                        IElement current = (sender as Control).Tag as IElement;

                        if (connectableElement.Location.X < current.Location.X)
                        {
                            if (connectableElement is IOutputContainingElement outputting)
                                if (current is IInputContainingElement inputting)
                                    outputting.Connect(inputting).ToString();
                            Render();
                        }
                        if (current.Location.X <= connectableElement.Location.X)
                        {
                            if (current is IOutputContainingElement outputting)
                                if (connectableElement is IInputContainingElement inputting)
                                    outputting.Connect(inputting).ToString();
                            Render();
                        }
                    }
                };
                panelCanvas.Controls.Add(connectButton);


                if (draft[i] is IInputContainingElement element2)
                {
                    int inputs = element2.Inputs.Count;
                    if (inputs != 0)
                    {
                        Point[] points1 = new Point[inputs];
                        Point[] points2 = new Point[inputs];

                        IOutputContainingElement[] copy = new IOutputContainingElement[inputs];
                        element2.Inputs.CopyTo(copy);
                        List<IOutputContainingElement> sortedList = copy.ToList();
                        sortedList.Sort((IOutputContainingElement i1, IOutputContainingElement i2) => i1.Location.Y < i2.Location.Y ? -1 : 1);

                        for (int k = 0; k < inputs; k++)
                        {
                            if (sortedList[k] is IGate)
                                points1[k] = new Point(sortedList[k].Location.X + gateWidth / 2 - 1, sortedList[k].Location.Y);
                            if (sortedList[k] is Input)
                                points1[k] = new Point(sortedList[k].Location.X + 2 * signalWidth - 1, sortedList[k].Location.Y);

                            if (sortedList[k] is NOT) points1[k].Y++;
                            if (sortedList[k] is AND) points1[k].X--;
                        }

                        if (element2 is IGate)
                        {
                            int inputsArea = gateHeight / 7 * 5;
                            int gap = inputsArea / (inputs + 1);

                            for (int k = 1; k < inputs + 1; k++)
                                points2[k - 1] = new Point(element2.Location.X - gateWidth / 2, element2.Location.Y - inputsArea / 2 + gap * k);
                        }
                        if (element2 is Output)
                        {
                            points2[0] = new Point(element2.Location.X - signalWidth / 2, element2.Location.Y);
                        }

                        if (element2 is OR || element2 is NOR || element2 is XOR || element2 is XNOR || element2 is IMPLY)
                            for (int k = 0; k < inputs; k++)
                                points2[k].X += 8;


                        int maxX = points1[0].X;
                        for (int k = 1; k < inputs; k++)
                            if (points1[k].X > maxX)
                                maxX = points1[k].X;
                        if (element2 is Output)
                            maxX = (points1[0].X + points2[0].X) / 2;

                        int minY = points1[0].Y < points2[0].Y ? points1[0].Y : points2[0].Y;
                        int maxY = points1[points1.Length - 1].Y > points2[points2.Length - 1].Y ? points1[points1.Length - 1].Y : points2[points2.Length - 1].Y;

                        Pen pen = new Pen(Color.Black, 2f);
                        g.DrawLine(pen, new Point(maxX, minY), new Point(maxX, maxY));

                        for (int k = 0; k < inputs; k++)
                        {
                            Point start = points1[k];
                            Point end = points2[k];
                            Point p1 = new Point(maxX, start.Y);
                            Point p2 = new Point(maxX, end.Y);
                            if (sortedList[k] is Input)
                                g.DrawLine(pen, new Point(sortedList[k].Location.X + signalWidth / 2 - 1, sortedList[k].Location.Y), start);
                            g.DrawLine(pen, start, new Point(maxX, start.Y));
                            if (inputs < 8)
                                g.DrawLine(pen, new Point(maxX, end.Y), end);
                            else
                                g.DrawLine(new Pen(Color.Black, 1f), new Point(maxX, end.Y), end);
                        }
                    }
                }
            }
        }

        private void PanelCanvasClick(object sender, EventArgs e)
        {
            if (elementSelected)
            {
                elementSelected = false;
                Cursor = Cursors.Default;
                for (int i = 0; i < panelGates.Controls.Count; i++)
                    if (panelGates.Controls[i].Controls[0].Tag.ToString() == selectedElement.ToString())
                    {
                        panelGates.Controls[i].BackColor = SystemColors.Control; break;
                    }
                for (int i = 0; i < panelParams.Controls.Count; i++)
                    if (panelParams.Controls[i].Controls[4].Tag.ToString() == selectedElement.ToString())
                    {
                        panelParams.Controls[i].BackColor = SystemColors.Control; break;
                    }
                AddElement(selectedElement);
            }
            if (elementMoveable)
            {
                elementMoveable = false;
                Cursor = Cursors.Default;
                Point newLoc = panelCanvas.PointToClient(Cursor.Position);
                moveableElement.Location = newLoc.X < 37 && newLoc.Y < 33 ? new Point(37, 33) : newLoc.X < 37 ? new Point(37, newLoc.Y) : newLoc.Y < 33 ? new Point(newLoc.X, 33) : newLoc;
                Render();
            }
        }

        private void AddElement(int tag)
        {
            IElement element = null;
            switch (tag)
            {
                case 1:
                    element = new Elements.Gates.Buffer();
                    break;
                case 2:
                    element = new NOT();
                    break;
                case 3:
                    element = new AND();
                    break;
                case 4:
                    element = new OR();
                    break;
                case 5:
                    element = new NAND();
                    break;
                case 6:
                    element = new NOR();
                    break;
                case 7:
                    element = new XOR();
                    break;
                case 8:
                    element = new XNOR();
                    break;
                case 9:
                    element = new IMPLY();
                    break;
                case 10:
                    string prefix = null;
                    if (radioButtonInputAuto.Checked)
                        prefix = "X";
                    if (radioButtonInputCustom.Checked)
                        prefix = textBoxInput.Text;

                    int indexer = 1;
                    string name = prefix;
                    bool unique = NameUnique(name);
                    while (!unique)
                    {
                        name = prefix + indexer;
                        indexer++;
                        unique = NameUnique(name);
                    }
                    element = new Input(name);
                    break;
                case 11:
                    string prefix2 = null;
                    if (radioButtonOutputAuto.Checked)
                        prefix2 = "Y";
                    if (radioButtonOutputCustom.Checked)
                        prefix2 = textBoxOutput.Text;

                    int indexer2 = 1;
                    string name2 = prefix2;
                    bool unique2 = NameUnique(name2);
                    while (!unique2)
                    {
                        name2 = prefix2 + indexer2;
                        indexer2++;
                        unique2 = NameUnique(name2);
                    }
                    element = new Output(name2);
                    break;
            }

            Point newLoc = panelCanvas.PointToClient(Cursor.Position);
            element.Location = newLoc.X < 37 && newLoc.Y < 33 ? new Point(37, 33) : newLoc.X < 37 ? new Point(37, newLoc.Y) : newLoc.Y < 33 ? new Point(newLoc.X, 33) : newLoc;

            draft.Add(element);
            if (element is Output)
                panelParams.Controls[0].Enabled = false;
            Render();
        }

        private bool NameUnique(string name)
        {
            for (int i = 0; i < draft.Count; i++)
                if (draft[i] is Input input)
                {
                    if (input.Name == name)
                        return false;
                }
                else if (draft[i] is Output output)
                {
                    if (output.Name == name)
                        return false;
                }
            return true;
        }

        private bool elementSelected = false;
        private int selectedElement = -1;

        private bool elementMoveable = false;
        private IElement moveableElement = null;

        private bool elementConnectable = false;
        private IElement connectableElement = null;
        private void ElementsToolsClicked(object sender, EventArgs e)
        {
            int current = int.Parse((sender as Control).Tag.ToString());

            if (!elementSelected)
            {
                elementSelected = true;
                selectedElement = current;
                Cursor = Cursors.Cross;
                (sender as Control).Parent.BackColor = Color.LightGray;
            }
            else
            {
                if (current == selectedElement)
                {
                    elementSelected = false;
                    Cursor = Cursors.Default;
                    (sender as Control).Parent.BackColor = SystemColors.Control;
                }
                else
                {
                    for (int i = 0; i < panelGates.Controls.Count; i++)
                        if (panelGates.Controls[i].Controls[0].Tag.ToString() == selectedElement.ToString())
                        {
                            panelGates.Controls[i].BackColor = SystemColors.Control; break;
                        }
                    for (int i = 0; i < panelParams.Controls.Count; i++)
                        if (panelParams.Controls[i].Controls[4].Tag.ToString() == selectedElement.ToString())
                        {
                            panelParams.Controls[i].BackColor = SystemColors.Control; break;
                        }
                    selectedElement = current;
                    (sender as Control).Parent.BackColor = Color.LightGray;
                }
            }
        }

        private void MenuButtonsMouseEnter(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = Color.LightGray;

            object tag = (sender as Control).Tag;
            if (tag != null && int.TryParse(tag.ToString(), out int gate) && gate <= 9)
            {
                labelGateName.Text = gateInfos[gate - 1].Name;
                pictureBoxFormula.Image = gateInfos[gate - 1].Formula;
                pictureBoxDiagram.Image = gateInfos[gate - 1].Diagram;
                pictureBoxGateTable.Image = gateInfos[gate - 1].TruthTable;
            }
        }

        private void MenuButtonsMouseLeave(object sender, EventArgs e)
        {
            if (!(elementSelected && (sender as Control).Tag.ToString() == selectedElement.ToString()))
                (sender as Control).Parent.BackColor = SystemColors.Control;

            labelGateName.Text = "<Назва вентиля>";
            if (pictureBoxFormula.Image != null) pictureBoxFormula.Image = null;
            if (pictureBoxDiagram != null) pictureBoxDiagram.Image = null;
            if (pictureBoxGateTable != null) pictureBoxGateTable.Image = null;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x128) return;
            base.WndProc(ref m);
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            Render();
        }
    }
}
