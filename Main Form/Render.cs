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
                    g.DrawImage(draft[i].Diagram, draft[i].Location.X - gateWidth / 2, draft[i].Location.Y - gateHeight / 2, gateWidth, gateHeight);
                else
                    g.DrawImage(draft[i].Diagram, draft[i].Location.X - signalWidth / 2, draft[i].Location.Y - signalHeight / 2, signalWidth, signalHeight);

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
            {
                AddOutputValue(g,)
            }
        }

        private void DrawBackground(Graphics g)
        {
            int width = panelCanvas.Width, height = panelCanvas.Height;

            for (int i = -2; i < height / 30 + 2; i++)
            {
                for (int j = -2; j < width / 30 + 2; j++)
                {
                    g.FillRectangle(Brushes.Black, j * 30 - 12, i * 30 - 18, 2, 1);
                }
            }
        }

        private void DrawConnections(Graphics g, int inputs, IInputContainingElement element2, int gateWidth, int gateHeight, int signalWidth, int signalHeight)
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
