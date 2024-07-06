using LogicCircuits.Elements.Gates;
using LogicCircuits.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicCircuits
{
    public partial class MainForm : Form
    {
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

                if (draft[i] is Input input && !input.IsSlave)
                    g.DrawString(input.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(input.Location.X - 12, input.Location.Y - 3 * signalHeight / 2));
                if (draft[i] is Output output)
                    g.DrawString(output.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(output.Location.X + 14, output.Location.Y - 8));

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
                    if (curr is Output)
                        panelParams.Controls[0].Enabled = true;
                    elementMoveable = false;
                    Cursor = Cursors.Default;

                    if (curr is Input supervisor && supervisor.IsSupervisor)
                    {
                        for (int k = 0; k < supervisor.AdditionalOutputs.Count; k++)
                        {
                            if (supervisor.AdditionalOutputs[k].Output != null)
                            {
                                supervisor.AdditionalOutputs[k].Output.Inputs.Remove(supervisor.AdditionalOutputs[k]);
                                supervisor.AdditionalOutputs[k].Output = null;
                            }
                            draft.Remove(supervisor.AdditionalOutputs[k]);
                        }
                    }
                    UpdateStatus();
                    Render();
                };

                if (draft[i] is Input slave && slave.IsSlave)
                {
                    removeButton.Location = new Point(slave.Location.X + 15, slave.Location.Y - 16);
                    toolTipMenu.SetToolTip(removeButton, "Видалити вентиль розгалуження");
                    removeButton.Click += (s, e) =>
                    {
                        Input slave1 = (s as Control).Tag as Input;
                        Input father = slave1.Supervisor;
                        father.AdditionalOutputs.Remove(slave1);
                        if (father.AdditionalOutputs.Count == 0)
                            father.IsSupervisor = false;
                        for (int k = 0; k < father.AdditionalOutputs.Count; k++)
                            father.AdditionalOutputs[k].Location = new Point(father.Location.X, father.Location.Y + 33 * (k + 1));
                        UpdateStatus();
                        Render();
                    };
                }

                panelCanvas.Controls.Add(removeButton);

                if (!(draft[i] is Input input1 && input1.IsSlave))
                {
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
                }


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
                else if (draft[i] is Input ordinaryInput && !ordinaryInput.IsSlave)
                    connLocation = new Point(draft[i].Location.X - 6 * signalWidth / 5, draft[i].Location.Y - 2 * signalHeight / 7);
                else if (draft[i] is Input child && child.IsSlave)
                    connLocation = new Point(draft[i].Location.X - 7, draft[i].Location.Y - 7);

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
                                UpdateStatus();
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
                            UpdateStatus();
                            Render();
                        }
                        if (current.Location.X <= connectableElement.Location.X)
                        {
                            if (current is IOutputContainingElement outputting)
                                if (connectableElement is IInputContainingElement inputting)
                                    outputting.Connect(inputting).ToString();
                            UpdateStatus();
                            Render();
                        }
                    }
                };
                panelCanvas.Controls.Add(connectButton);

                if (draft[i] is Input param && !param.IsSlave)
                {
                    Label valueButton = new Label
                    {
                        AutoSize = false,
                        Size = new Size(23, 23),
                        Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel),
                        Tag = param,
                        Text = param.Value.ToString(),
                        Location = new Point(param.Location.X - 11, param.Location.Y - 11)
                    };
                    if (param.Value == 0)
                        valueButton.BackColor = Color.LightGoldenrodYellow;
                    if (param.Value == 1)
                        valueButton.BackColor = Color.LightSteelBlue;
                    toolTipMenu.SetToolTip(valueButton, "Перемикати значення вхідного сигнала 0/1");
                    valueButton.Click += (s, e) =>
                    {
                        ((s as Control).Tag as Input).Value = ((s as Control).Tag as Input).Value == 0 ? 1 : 0;
                        if ((s as Control).Tag is Input supervisor && supervisor.IsSupervisor)
                        {
                            for (int k = 0; k < supervisor.AdditionalOutputs.Count; k++)
                                supervisor.AdditionalOutputs[k].Value = supervisor.Value;
                        }
                        UpdateStatus();
                        Render();
                    };
                    panelCanvas.Controls.Add(valueButton);

                    PictureBox branchingButton = new PictureBox
                    {
                        Tag = param,
                        Size = new Size(25, 25),
                        Image = Properties.Resources.branching,
                        SizeMode = PictureBoxSizeMode.Zoom,
                    };
                    branchingButton.Location = new Point(param.Location.X - 42, param.Location.Y + 15);
                    toolTipMenu.SetToolTip(branchingButton, "Додати ще один вхідний сигнал розгалуженням цього параметра");
                    branchingButton.Click += (s, e) =>
                    {
                        Input super = (s as Control).Tag as Input;

                        Input additional = new Input(super.Name);
                        additional.IsSlave = true;
                        additional.Supervisor = super;
                        additional.NumberAsAdditional = super.AdditionalOutputs.Count + 1;

                        additional.Location = new Point(super.Location.X, super.Location.Y + 33 * additional.NumberAsAdditional);
                        additional.Diagram = Properties.Resources.additional_input;
                        additional.Value = super.Value;

                        super.IsSupervisor = true;
                        super.AdditionalOutputs.Add(additional);

                        draft.Add(additional);
                        Render();
                    };
                    panelCanvas.Controls.Add(branchingButton);
                }


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
                                points1[k] = new Point(sortedList[k].Location.X + signalWidth - 1, sortedList[k].Location.Y);

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
    }
}
