using System;
using System.Collections.Generic;
using LogicCircuits.Elements.Interfaces;
using System.Drawing;
using System.IO;
using System.Linq;
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

            bool empty = false;

            IEnumerable<string> circuits = null;
            try
            {
                circuits = from f in Directory.GetFiles("Circuits")
                           where Path.GetExtension(f) == ".dat"
                           select f;
                if (circuits == null || circuits.Count() == 0)
                    empty = true;
            }
            catch
            {
                empty = true;
            }

            if (!empty)
            {
                for (int i = circuits.Count() - 1; i >= 0; i--)
                {
                    Serialization.Serialization.DeserializeCircuit(circuits.ElementAt(i), out string name, out DateTime dateTime, out _);

                    Panel panel = new Panel
                    {
                        Dock = DockStyle.Top,
                        Height = 36,
                        Tag = circuits.ElementAt(i),
                        Padding = new Padding(0, 3, 0, 3),
                    };
                    form.Controls.Add(panel);

                    Label date = new Label
                    {
                        Dock = DockStyle.Fill,
                        AutoSize = false,
                        Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Regular),
                        TextAlign = ContentAlignment.TopCenter,
                        Text = dateTime.ToString("dd.MM (HH:mm)"),
                    };
                    panel.Controls.Add(date);

                    Button button = new Button
                    {
                        Dock = DockStyle.Left,
                        FlatStyle = FlatStyle.Flat,
                        Width = 150,
                        Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular),
                        TextAlign = ContentAlignment.TopCenter,
                        Text = name,
                        BackColor = SystemColors.ControlLight,
                    };

                    button.Click += (s, e) =>
                    {
                        string file = (s as Control).Parent.Tag as string;
                        Serialization.Serialization.DeserializeCircuit(file, out _, out _, out List<IElement> circuit);
                        foreach (var c in circuit)
                            if (c.Controls == null)
                                c.Controls = new List<Control>();
                        MainForm.draft = circuit;
                        MainForm.openedNewCircuit = true;
                        form.Close();
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

                    delete.Click += (s, e) =>
                    {
                        if (DialogResult.Yes == MessageBox.Show("Ви впевнені, що хочете видалити цю схему?", "Увага!", MessageBoxButtons.YesNo))
                        {
                            string file = (s as Control).Parent.Tag as string;
                            File.Delete(file);
                            form.Controls.Remove((s as Control).Parent);
                        }
                    };

                    panel.Controls.Add(delete);
                }
            }

            if (empty)
            {
                Label notfound = new Label
                {
                    Dock = DockStyle.Top,
                    Font = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Regular),
                    AutoSize = false,
                    TextAlign = ContentAlignment.TopCenter,
                    Text = "Ще немає збережених схем.",
                    Height = 45,
                };
                form.Controls.Add(notfound);
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
