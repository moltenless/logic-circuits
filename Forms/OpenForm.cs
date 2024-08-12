using LogicCircuits.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicCircuits.Forms
{
    public static partial class FormsBuilder
    {
        public static Form GetOpenForm()
        {
            Form form = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                Icon = Properties.Resources.diagram,
                Text = "Відкрити",
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                Width = 300,
                Height = 400,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MaximizeBox = false,
                AutoScroll = true,
                Padding = new Padding(30, 10, 30, 10)
            };


            (string, DateTime)[] db = new[] { ("scheme1", DateTime.Now), ("circuit2", DateTime.Now.AddDays(4)), ("draft random", DateTime.MaxValue) };
            for (int i = db.Length - 1; i >= 0; i--)
            {
                Panel panel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 36,
                    Tag = db[i],
                    Padding = new Padding(0, 3, 0, 3),
                };
                form.Controls.Add(panel);

                Label date = new Label
                {
                    Dock = DockStyle.Fill,
                    AutoSize = false,
                    Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Regular),
                    TextAlign = ContentAlignment.TopCenter,
                    Text = db[i].Item2.ToString("dd.MM (HH:mm)"),
                };
                panel.Controls.Add(date);

                Button button = new Button
                {
                    Dock = DockStyle.Left,
                    FlatStyle = FlatStyle.Flat,
                    Width = 150,
                    Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular),
                    TextAlign = ContentAlignment.TopCenter,
                    Text = db[i].Item1,
                    BackColor = SystemColors.ControlLight,
                };
                panel.Controls.Add(button);

                Button delete = new Button
                {
                    Dock = DockStyle.Right,
                    Width = 25,
                    FlatStyle = FlatStyle.Flat,
                    BackgroundImage = Properties.Resources.erase,
                    BackgroundImageLayout = ImageLayout.Zoom,
                    BackColor = SystemColors.ControlLight,
                };
                panel.Controls.Add(delete);
            }

            if (db.Length == 0)
            {
                Label empty = new Label
                {
                    Dock = DockStyle.Top,
                    Font = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Regular),
                    AutoSize = false,
                    TextAlign = ContentAlignment.TopCenter,
                    Text = "Ще немає збережених схем.",
                    Height = 45,
                };
                form.Controls.Add(empty);
            }

            Label label = new Label
            {
                Dock = DockStyle.Top,
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Regular),
                AutoSize = false,
                TextAlign = ContentAlignment.TopCenter,
                Text = "Відкрити схему:",
                Height = 45,
            };
            form.Controls.Add(label);

            return form;
        }
    }
}
