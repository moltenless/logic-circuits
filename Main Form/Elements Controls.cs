using LogicCircuits.Elements;
using LogicCircuits.Elements.Gates;
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
        private void AddRemoveButton(IElement element, int gateWidth, int gateHeight, int signalWidth, int signalHeight)
        {
            PictureBox removeButton = new PictureBox
            {
                Tag = element,
                Size = new Size(10, 10),
                Image = Properties.Resources.close,
                SizeMode = PictureBoxSizeMode.Zoom,
                Name = "remove"
            };
            removeButton.Location = element is IGate ? new Point(element.Location.X - gateWidth / 4, element.Location.Y - 4 * gateHeight / 5)
            : new Point(element.Location.X, element.Location.Y - 7 * signalHeight / 8);
            toolTipMenu.SetToolTip(removeButton, "Видалити вентиль");

            if (element is Input slave && slave.IsSlave)
            {
                removeButton.Location = new Point(slave.Location.X + 15, slave.Location.Y - 16);
                toolTipMenu.SetToolTip(removeButton, "Видалити вентиль розгалуження");
                removeButton.Click += RemoveSlaveButtonClick;
            }
            removeButton.Click += RemoveButtonClick;

            element.Controls.Add(removeButton);
            panelCanvas.Controls.Add(removeButton);
        }

        private void RemoveButtonClick(object sender, EventArgs e)
        {
            List<IElement> elementsToRemove = new List<IElement>(); 

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

            elementsToRemove.Add(curr);

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

                    elementsToRemove.Add(supervisor.AdditionalOutputs[k]);
                }
            }
            UpdateStatus();
            RenderAfterRemoval(elementsToRemove);
        }

        private void RemoveSlaveButtonClick(object sender, EventArgs e)
        {
            Input slave1 = (sender as Control).Tag as Input;
            Input father = slave1.Supervisor;
            father.AdditionalOutputs.Remove(slave1);
            if (father.AdditionalOutputs.Count == 0)
                father.IsSupervisor = false;
            for (int k = 0; k < father.AdditionalOutputs.Count; k++)
                father.AdditionalOutputs[k].Location = new Point(father.Location.X, father.Location.Y + 33 * (k + 1));
            RenderAfterRemovalSlave(father);
        }

        private void AddMoveButton(IElement element, int gateWidth, int gateHeight, int signalWidth, int signalHeight)
        {
            PictureBox moveButton = new PictureBox
            {
                Tag = element,
                Size = new Size(10, 10),
                Image = Properties.Resources.move,
                SizeMode = PictureBoxSizeMode.Zoom,
            };
            moveButton.Location = element is IGate ? new Point(element.Location.X - 2 * gateWidth / 5, element.Location.Y - 4 * gateHeight / 5)
                : new Point(element.Location.X - signalWidth / 3, element.Location.Y - 7 * signalHeight / 8);
            toolTipMenu.SetToolTip(moveButton, "Перемістити вентиль");
            moveButton.Click += MoveButtonClick;
            element.Controls.Add(moveButton);
            panelCanvas.Controls.Add(moveButton);
        }

        private void MoveButtonClick(object sender, EventArgs e)
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
        }

        private void AddConnectionButton(IElement element, int gateWidth, int gateHeight, int signalWidth, int signalHeight)
        {
            PictureBox connectButton = new PictureBox
            {
                Tag = element,
                Size = new Size(15, 15),
                Image = Properties.Resources.connect,
                SizeMode = PictureBoxSizeMode.Zoom,
                Name = "connection"
            };
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
            else if (element is Input ordinaryInput && !ordinaryInput.IsSlave)
                connLocation = new Point(element.Location.X - 6 * signalWidth / 5, element.Location.Y - 2 * signalHeight / 7);
            else if (element is Input child && child.IsSlave)
                connLocation = new Point(element.Location.X - 7, element.Location.Y - 7);
            else if (element is Output)
                connLocation = new Point(element.Location.X - 2 * signalWidth / 7, element.Location.Y + 2 * signalHeight / 3);
            connectButton.Location = connLocation;
            toolTipMenu.SetToolTip(connectButton, "Приєднати вентиль або вихідний сигнал");
            connectButton.Click += ConnectionButtonClick;
            element.Controls.Add(connectButton);
            panelCanvas.Controls.Add(connectButton);
        }

        private void ConnectionButtonClick(object sender, EventArgs e)
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
                        RenderCompletely();
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
                    RenderCompletely();
                }
                if (current.Location.X <= connectableElement.Location.X)
                {
                    if (current is IOutputContainingElement outputting)
                        if (connectableElement is IInputContainingElement inputting)
                            outputting.Connect(inputting).ToString();
                    UpdateStatus();
                    RenderCompletely();
                }
            }
        }

        private void AddValueButton(Input param)
        {
            Label valueButton = new Label
            {
                AutoSize = false,
                Size = new Size(23, 23),
                Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel),
                Tag = param,
                Text = param.Value.ToString(),
                Location = new Point(param.Location.X - 11, param.Location.Y - 12)
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
                RenderCompletely();
            };
            param.Controls.Add(valueButton);
            panelCanvas.Controls.Add(valueButton);
        }

        private void AddBranchingButton(Input param)
        {
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
                RenderCompletely();
            };
            param.Controls.Add(branchingButton);
            panelCanvas.Controls.Add(branchingButton);
        }
    }
}
