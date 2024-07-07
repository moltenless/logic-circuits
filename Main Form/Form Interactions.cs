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
                moveableElement.Location = newLoc.X < 37 && newLoc.Y < 33 ? new Point(37, 33) : newLoc.X < 37
                    ? new Point(37, newLoc.Y) : newLoc.Y < 33 ? new Point(newLoc.X, 33) : newLoc;
                if (moveableElement is Input input && input.IsSupervisor)
                {
                    for (int k = 0; k < input.AdditionalOutputs.Count; k++)
                        input.AdditionalOutputs[k].Location = new Point(input.Location.X, input.Location.Y + 33 * (k + 1));
                }
                RenderCompletely();
            }
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
    }
}
