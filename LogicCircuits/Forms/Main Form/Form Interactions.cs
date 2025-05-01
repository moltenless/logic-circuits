using LogicCircuits.Elements;
using LogicCircuits.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LogicCircuits.Elements.Interfaces;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

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
                RenderAfterMoving(moveableElement);
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

        public static bool openedNewCircuit = false;
        private void MenuClick(object sender, EventArgs e)
        {
            int tag = int.Parse((sender as Control).Tag.ToString());

            if (tag == 1)
            {
                Form openForm = FormsBuilder.GetOpenForm();
                openForm.ShowDialog();
                if (openedNewCircuit)
                {
                    UpdateStatus();
                    RenderCompletely();
                    openedNewCircuit = false;

                    bool hasOutput = false;
                    foreach (var element in draft)
                        if (element is Output)
                            hasOutput = true;
                    if (hasOutput)
                        panelParams.Controls[0].Enabled = false;
                    else
                        panelParams.Controls[0].Enabled = true;
                }
            }
            if (tag == 2)
            {
                Form saveForm = FormsBuilder.GetSaveForm(draft);
                saveForm.ShowDialog();
            }
            if (tag == 3)
            {
                UpdateStatus();
                if (ready)
                {
                    Form truthTableForm = FormsBuilder.GetTruthTableForm(registry);
                    truthTableForm.ShowDialog();
                }
                else
                    MessageBox.Show("Скласти таблицю істинності неможливо, бо схема неповна.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (tag == 4)
            {
                UpdateStatus();
                if (ready)
                {
                    Form dnfForm = FormsBuilder.GetDNFForm(registry);
                    dnfForm.ShowDialog();
                }
                else
                    MessageBox.Show("Скласти ДДНФ неможливо, бо схема неповна.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (tag == 5)
            {
                UpdateStatus();
                if (ready)
                {
                    Form cnfForm = FormsBuilder.GetCNFForm(registry);
                    cnfForm.ShowDialog();
                }
                else
                    MessageBox.Show("Скласти ДКНФ неможливо, бо схема неповна.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (tag == 6)
            {
                UpdateStatus();
                if (ready)
                {
                    Form minimizationForm = FormsBuilder.GetMinimizationForm(registry);
                    minimizationForm.ShowDialog();
                }
                else
                    MessageBox.Show("Мінімізувати функцію неможливо, бо схема неповна.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (tag == 7)
            {
                DialogResult res = MessageBox.Show("Ви впевнені, що хочете видалити схему?", "Увага", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (res == DialogResult.Yes)
                {
                    draft.Clear();
                    elementSelected = false;
                    elementMoveable = false;
                    elementConnectable = false;
                    panelParams.Controls[0].Enabled = true;
                    Cursor = Cursors.Default;
                    UpdateStatus();
                    RenderCompletely();
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

        private void UpperMenuButtonsMouseEnter(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = Color.LightGray;
        }

        private void UpperMenuButtonsMouseLeave(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = SystemColors.Control;
        }
    }
}
