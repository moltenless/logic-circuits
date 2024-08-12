using LogicCircuits.Elements.Gates;
using LogicCircuits.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;

namespace LogicCircuits
{
    public partial class MainForm : Form
    {
        private void RenderCompletely()
        {
            Graphics g = panelCanvas.CreateGraphics();

            g.Clear(Color.LightGray);
            panelCanvas.Controls.Clear();

            DrawBackground(g);

            int gateWidth = 70, gateHeight = 40;//coef 0.575
            int signalWidth = 30, signalHeight = 30;
            for (int i = 0; i < draft.Count; i++)
            {
                if (draft[i] is IGate)
                    g.DrawImage(DiagramImages.GetDiagram(draft[i]), draft[i].Location.X - gateWidth / 2, draft[i].Location.Y - gateHeight / 2, gateWidth, gateHeight);
                else
                    g.DrawImage(DiagramImages.GetDiagram(draft[i]), draft[i].Location.X - signalWidth / 2, draft[i].Location.Y - signalHeight / 2, signalWidth, signalHeight);

                if (draft[i] is Input input && !input.IsSlave)
                    g.DrawString(input.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(input.Location.X - 12, input.Location.Y - 3 * signalHeight / 2));
                if (draft[i] is Output output)
                    g.DrawString(output.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(output.Location.X + 14, output.Location.Y - 8));

                AddRemoveButton(draft[i], gateWidth, gateHeight, signalWidth, signalHeight);

                if (!(draft[i] is Input input1 && input1.IsSlave))
                    AddMoveButton(draft[i], gateWidth, gateHeight, signalWidth, signalHeight);

                AddConnectionButton(draft[i], gateWidth, gateHeight, signalWidth, signalHeight);

                if (draft[i] is Input param && !param.IsSlave)
                {
                    AddValueButton(param);
                    AddBranchingButton(param);
                }

                if (draft[i] is IInputContainingElement element2)
                {
                    int inputs = element2.Inputs.Count;
                    if (inputs != 0)
                        DrawConnections(g, inputs, element2, gateWidth, gateHeight, signalWidth, signalHeight);
                }
            }

            if (ready)
                DrawOutputValue(g, signalWidth, signalHeight);
        }

        private void RenderAfterRemoval(List<IElement> elementsToRemove)
        {
            Graphics g = panelCanvas.CreateGraphics();

            g.Clear(Color.LightGray);

            for (int i = 0; i < elementsToRemove.Count; i++)
                for (int j = 0; j < elementsToRemove[i].Controls.Count; j++)
                    panelCanvas.Controls.Remove(elementsToRemove[i].Controls[j]);

            DrawBackground(g);

            int gateWidth = 70, gateHeight = 40;//coef 0.575
            int signalWidth = 30, signalHeight = 30;
            for (int i = 0; i < draft.Count; i++)
            {
                if (draft[i] is IGate)
                    g.DrawImage(DiagramImages.GetDiagram(draft[i]), draft[i].Location.X - gateWidth / 2, draft[i].Location.Y - gateHeight / 2, gateWidth, gateHeight);
                else
                    g.DrawImage(DiagramImages.GetDiagram(draft[i]), draft[i].Location.X - signalWidth / 2, draft[i].Location.Y - signalHeight / 2, signalWidth, signalHeight);

                if (draft[i] is Input input && !input.IsSlave)
                    g.DrawString(input.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(input.Location.X - 12, input.Location.Y - 3 * signalHeight / 2));
                if (draft[i] is Output output)
                    g.DrawString(output.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(output.Location.X + 14, output.Location.Y - 8));

                if (draft[i] is IInputContainingElement element2)
                {
                    int inputs = element2.Inputs.Count;
                    if (inputs != 0)
                        DrawConnections(g, inputs, element2, gateWidth, gateHeight, signalWidth, signalHeight);
                }
            }

            if (ready)
                DrawOutputValue(g, signalWidth, signalHeight);
        }

        private void RenderAfterRemovalSlave(Input father)
        {
            for (int i = 0; i < father.AdditionalOutputs.Count; i++)
                for (int j = 0; j < father.AdditionalOutputs[i].Controls.Count; j++)
                    if (father.AdditionalOutputs[i].Controls[j].Name == "remove")
                        father.AdditionalOutputs[i].Controls[j].Location = new Point(father.AdditionalOutputs[i].Location.X + 15, father.AdditionalOutputs[i].Location.Y - 16);
                    else if (father.AdditionalOutputs[i].Controls[j].Name == "connection")
                        father.AdditionalOutputs[i].Controls[j].Location = new Point(father.AdditionalOutputs[i].Location.X - 7, father.AdditionalOutputs[i].Location.Y - 7);
        }

        private void RenderOnlyGraphics()
        {
            Graphics g = panelCanvas.CreateGraphics();

            g.Clear(Color.LightGray);

            DrawBackground(g);

            int gateWidth = 70, gateHeight = 40;//coef 0.575
            int signalWidth = 30, signalHeight = 30;
            for (int i = 0; i < draft.Count; i++)
            {
                if (draft[i] is IGate)
                    g.DrawImage(DiagramImages.GetDiagram(draft[i]), draft[i].Location.X - gateWidth / 2, draft[i].Location.Y - gateHeight / 2, gateWidth, gateHeight);
                else
                    g.DrawImage(DiagramImages.GetDiagram(draft[i]), draft[i].Location.X - signalWidth / 2, draft[i].Location.Y - signalHeight / 2, signalWidth, signalHeight);

                if (draft[i] is Input input && !input.IsSlave)
                    g.DrawString(input.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(input.Location.X - 12, input.Location.Y - 3 * signalHeight / 2));
                if (draft[i] is Output output)
                    g.DrawString(output.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(output.Location.X + 14, output.Location.Y - 8));

                if (draft[i] is IInputContainingElement element2)
                {
                    int inputs = element2.Inputs.Count;
                    if (inputs != 0)
                        DrawConnections(g, inputs, element2, gateWidth, gateHeight, signalWidth, signalHeight);
                }
            }

            if (ready)
                DrawOutputValue(g, signalWidth, signalHeight);
        }

        private void RenderAfterSwitchingValue(Control valueButton, Input input)
        {
            RenderOnlyGraphics();
            valueButton.Text = input.Value.ToString();
            if (input.Value == 0)
                valueButton.BackColor = Color.LightGoldenrodYellow;
            if (input.Value == 1)
                valueButton.BackColor = Color.LightSteelBlue;
        }

        private void RenderAfterAddingSlave(Input additional)
        {
            Graphics g = panelCanvas.CreateGraphics();

            int gateWidth = 70, gateHeight = 40;//coef 0.575
            int signalWidth = 30, signalHeight = 30;

            g.DrawImage(DiagramImages.GetDiagram(additional), additional.Location.X - signalWidth / 2, additional.Location.Y - signalHeight / 2, signalWidth, signalHeight);
            AddRemoveButton(additional, gateWidth, gateHeight, signalWidth, signalHeight);
            AddConnectionButton(additional, gateWidth, gateHeight, signalWidth, signalHeight);
        }

        private void RenderAfterMoving(IElement element)
        {
            int gateWidth = 70, gateHeight = 40;//coef 0.575
            int signalWidth = 30, signalHeight = 30;

            if (element is IGate)
            {
                for (int i = 0; i < element.Controls.Count; i++)
                {
                    if (element.Controls[i].Name == "remove")
                        element.Controls[i].Location = new Point(element.Location.X - gateWidth / 4, element.Location.Y - 4 * gateHeight / 5);
                    else if (element.Controls[i].Name == "move")
                        element.Controls[i].Location = new Point(element.Location.X - 2 * gateWidth / 5, element.Location.Y - 4 * gateHeight / 5);
                    else if (element.Controls[i].Name == "connection")
                    {
                        Point connLocation = new Point();
                        if (element is AND || element is NAND)
                            connLocation = new Point(element.Location.X - 2 * gateWidth / 6, element.Location.Y - 1 * gateHeight / 5);
                        else if (element is OR || element is NOR)
                            connLocation = new Point(element.Location.X - 2 * gateWidth / 7, element.Location.Y - 1 * gateHeight / 5);
                        else if (element is XOR || element is XNOR)
                            connLocation = new Point(element.Location.X - 2 * gateWidth / 9, element.Location.Y - 1 * gateHeight / 5);
                        else if (element is IMPLY)
                            connLocation = new Point(element.Location.X - 2 * gateWidth / 8, element.Location.Y - 1 * gateHeight / 5);
                        else if (element is Elements.Gates.Buffer || element is NOT)
                            connLocation = new Point(element.Location.X - 2 * gateWidth / 5, element.Location.Y - 1 * gateHeight / 5);
                        element.Controls[i].Location = connLocation;
                    }
                }
            }
            if (element is Input)
            {
                for (int i = 0; i < element.Controls.Count; i++)
                {
                    if (element.Controls[i].Name == "remove")
                        element.Controls[i].Location = new Point(element.Location.X, element.Location.Y - 7 * signalHeight / 8);
                    else if (element.Controls[i].Name == "move")
                        element.Controls[i].Location = new Point(element.Location.X - signalWidth / 3, element.Location.Y - 7 * signalHeight / 8);
                    else if (element.Controls[i].Name == "connection")
                        element.Controls[i].Location = new Point(element.Location.X - 6 * signalWidth / 5, element.Location.Y - 2 * signalHeight / 7);
                    else if (element.Controls[i].Name == "value")
                        element.Controls[i].Location = new Point(element.Location.X - 11, element.Location.Y - 12);
                    else if (element.Controls[i].Name == "branching")
                        element.Controls[i].Location = new Point(element.Location.X - 42, element.Location.Y + 15);

                    if ((element as Input).IsSupervisor)
                    {
                        Input father = (Input)element;
                        for (int j = 0; j < father.AdditionalOutputs.Count; j++)
                            for (int k = 0; k < father.AdditionalOutputs[j].Controls.Count; k++)
                                if (father.AdditionalOutputs[j].Controls[k].Name == "remove")
                                    father.AdditionalOutputs[j].Controls[k].Location = new Point(father.AdditionalOutputs[j].Location.X + 15, father.AdditionalOutputs[j].Location.Y - 16);
                                else if (father.AdditionalOutputs[j].Controls[k].Name == "connection")
                                    father.AdditionalOutputs[j].Controls[k].Location = new Point(father.AdditionalOutputs[j].Location.X - 7, father.AdditionalOutputs[j].Location.Y - 7);
                    }
                }
            }
            if (element is Output)
            {
                for (int i = 0; i < element.Controls.Count; i++)
                {
                    if (element.Controls[i].Name == "remove")
                        element.Controls[i].Location = new Point(element.Location.X, element.Location.Y - 7 * signalHeight / 8);
                    else if (element.Controls[i].Name == "move")
                        element.Controls[i].Location = new Point(element.Location.X - signalWidth / 3, element.Location.Y - 7 * signalHeight / 8);
                    else if (element.Controls[i].Name == "connection")
                        element.Controls[i].Location = new Point(element.Location.X - 2 * signalWidth / 7, element.Location.Y + 2 * signalHeight / 3);
                }
            }
            RenderOnlyGraphics();
        }

        private void RenderNewElement(IElement element)
        {
            Graphics g = panelCanvas.CreateGraphics();

            int gateWidth = 70, gateHeight = 40;//coef 0.575
            int signalWidth = 30, signalHeight = 30;

            if (element is IGate)
                g.DrawImage(DiagramImages.GetDiagram(element), element.Location.X - gateWidth / 2, element.Location.Y - gateHeight / 2, gateWidth, gateHeight);
            else
                g.DrawImage(DiagramImages.GetDiagram(element), element.Location.X - signalWidth / 2, element.Location.Y - signalHeight / 2, signalWidth, signalHeight);

            if (element is Input input)
                g.DrawString(input.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                    Brushes.Black, new Point(input.Location.X - 12, input.Location.Y - 3 * signalHeight / 2));
            if (element is Output output)
                g.DrawString(output.Name, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Italic, GraphicsUnit.Pixel),
                    Brushes.Black, new Point(output.Location.X + 14, output.Location.Y - 8));

            AddRemoveButton(element, gateWidth, gateHeight, signalWidth, signalHeight);

            AddMoveButton(element, gateWidth, gateHeight, signalWidth, signalHeight);

            AddConnectionButton(element, gateWidth, gateHeight, signalWidth, signalHeight);

            if (element is Input param)
            {
                AddValueButton(param);
                AddBranchingButton(param);
            }
        }

        private void DrawBackground(Graphics g)
        {
            int width = panelCanvas.Width, height = panelCanvas.Height;

            for (int i = -2; i < height / 30 + 2; i++)
                for (int j = -2; j < width / 30 + 2; j++)
                    g.FillRectangle(Brushes.Black, j * 30 - 12, i * 30 - 18, 2, 1);
        }

        private void DrawConnections(Graphics g, int inputs, IInputContainingElement element2, int gateWidth, int gateHeight, int signalWidth, int signalHeight)
        {
            Point[] points1 = new Point[inputs];
            Point[] points2 = new Point[inputs];

            IOutputContainingElement[] copy = new IOutputContainingElement[inputs];
            element2.Inputs.CopyTo(copy);
            List<IOutputContainingElement> sortedList = copy.ToList();
            sortedList.Sort((IOutputContainingElement i1, IOutputContainingElement i2) => i1.Location.Y < i2.Location.Y ? -1 : 1);

            for (int i = 0; i < inputs; i++)
            {
                if (sortedList[i] is IGate)
                    points1[i] = new Point(sortedList[i].Location.X + gateWidth / 2 - 1, sortedList[i].Location.Y);
                if (sortedList[i] is Input)
                    points1[i] = new Point(sortedList[i].Location.X + signalWidth - 1, sortedList[i].Location.Y);

                if (sortedList[i] is NOT) points1[i].Y++;
                if (sortedList[i] is AND) points1[i].X--;
            }

            if (element2 is IGate)
            {
                int inputsArea = gateHeight / 7 * 5;
                int gap = inputsArea / (inputs + 1);

                for (int k = 1; k < inputs + 1; k++)
                    points2[k - 1] = new Point(element2.Location.X - gateWidth / 2, element2.Location.Y - inputsArea / 2 + gap * k);
            }
            if (element2 is Output)
                points2[0] = new Point(element2.Location.X - signalWidth / 2, element2.Location.Y);

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

            for (int i = 0; i < inputs; i++)
            {
                if (!ready) pen.Color = Color.Black;
                else
                    for (int j = 0; j < registry.Count; j++)
                        if (registry[j].Item1 == sortedList[i])
                            if (registry[j].outputResult == 1)
                                pen.Color = Color.Blue;
                            else if (registry[j].outputResult == 0)
                                pen.Color = Color.Yellow;
                Point start = points1[i];
                Point end = points2[i];
                Point p1 = new Point(maxX, start.Y);
                Point p2 = new Point(maxX, end.Y);
                if (sortedList[i] is Input)
                    g.DrawLine(pen, new Point(sortedList[i].Location.X + signalWidth / 2 - 1, sortedList[i].Location.Y), start);
                g.DrawLine(pen, start, new Point(maxX, start.Y));
                if (inputs < 8)
                    g.DrawLine(pen, new Point(maxX, end.Y), end);
                else
                    g.DrawLine(new Pen(Color.Black, 1f), new Point(maxX, end.Y), end);
            }
        }

        private void DrawOutputValue(Graphics g, int signalWidth, int signalHeight)
        {
            (IElement element, int result) = registry.Last();

            Brush brush = result == 1 ? Brushes.LightSteelBlue : Brushes.LightGoldenrodYellow;
            g.FillEllipse(brush, element.Location.X - signalWidth / 2 + 3, element.Location.Y - signalHeight / 2 + 3, signalWidth - 6, signalHeight - 6);

            g.DrawString(result.ToString(), new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel),
                        Brushes.Black, new Point(element.Location.X - 9, element.Location.Y - 12));
        }
    }
}
