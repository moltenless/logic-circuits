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
        Output output = null;
        int result; 

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
            this.result = output.CalculateOutput(registry);

            if (this.result == -1)
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
                    bmp.SetPixel(i, j, Color.FromArgb((255 - (i > 255 ? 255 : i)), color));
            labelStatus.Image = bmp;
        }
    }
}
