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
        bool ready = false;
        private void UpdateStatus()
        {
            bool containsOutput = ContainsOutput();
            if (!containsOutput)
            {
                ready = false;
                SetStatusLabel(ready);
                return;
            }

            ///code

        }

        private bool ContainsOutput()
        {
            for (int i = 0; i < draft.Count; i++)
            {
                if (draft[i] is Output)
                {
                    return true;
                }
            }
            return false;
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
