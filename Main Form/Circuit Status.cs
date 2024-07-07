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
        bool ready = false;
        List<(IElement, int outputResult)> registry = new List<(IElement, int outputResult)>();
        int output; 

        private void UpdateStatus()
        {
            int outputs = OutputCount(out Output output);
            if (outputs != 1)
            {
                ready = false;
                SetStatusLabel(ready);
                return;
            }

            registry.Clear();
            this.output = output.CalculateOutput(registry);

            string text = "output is " + this.output.ToString() + "\n";
            for (int i = 0; i < registry.Count; i++)
                if (registry[i].Item1 is IGate)
                    text += "gate " + $" has {(registry[i].Item1 as IInputContainingElement).Inputs.Count} inputs and have the output: {registry[i].outputResult}\n"; 
            else if (registry[i].Item1 is Input)
                    text += $"input {(registry[i].Item1 as Input).Name}" + $" has the output: {registry[i].outputResult}\n";
            else if (registry[i].Item1 is Output)
                    text += $"output {(registry[i].Item1 as Output).Name}" + $" has the output: {registry[i].outputResult}\n";
            MessageBox.Show(text);

            if (this.output == -1)
            {
                ready = false;
                SetStatusLabel(ready);
                return;
            }
            else
            {
                ready = true;
                SetStatusLabel(ready);
                return;
            }
        }

        private int OutputCount(out Output lastOutput)
        {
            int counter = 0;
            lastOutput = null;
            for (int i = 0; i < draft.Count; i++)
            {
                if (draft[i] is Output output)
                {
                    counter++;
                    lastOutput = output;
                }
            }
            return counter;
        }

        private void SetStatusLabel(bool status)
        {
            labelStatus.Text = status ? "Статус: Схема складена." : "Статус: Схема не повна.";
            Color color = status ? Color.Green : Color.PaleVioletRed;

            Bitmap bmp = new Bitmap(labelStatus.Width, labelStatus.Height);
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                    bmp.SetPixel(i, j, Color.FromArgb((255 - (i > 255 ? 255 : i)) / 2, color));
            labelStatus.Image = bmp;
        }
    }
}
