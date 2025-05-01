using LogicCircuits.Elements;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LogicCircuits.Forms
{
    public static partial class FormsBuilder
    {
        public static Form GetMinimizationForm(List<(IElement, int outputResult)> registry)
        {
            List<List<int>> truthTable = FormsBuilder.GetTruthTable(registry, out List<string> columnNames);
            int cols = truthTable[0].Count;
            int rows = truthTable.Count;

            Form form = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                Icon = Properties.Resources.diagram,
                Text = "Мінімізація булевої функції методом Куайна (Куайна-Мак-Класкі)",
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                Tag = (truthTable, columnNames),
                Width = 750,
                Height = 800,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MaximizeBox = false,
                AutoScroll = true,
            };

            TextBox textBox1 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
            };
            form.Controls.Add(textBox1);

            string dnf = GetDNF(truthTable, columnNames, cols, rows);
            textBox1.Text = "ДДНФ:\r\n" + dnf;
            textBox1.Select(0, 0);

            TextBox textBox2 = new TextBox
            {
                Dock = DockStyle.Bottom,
                Height = 400,
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
            };
            form.Controls.Add(textBox2);

            string minimized = Minimization.MinimizeQuineMcCluskey(registry);
            textBox2.Text = "Мінімізована форма:\r\n" + minimized;
            textBox2.Select(0, 0);

            return form;
        }
    }
}