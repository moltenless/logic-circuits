using LogicCircuits.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicCircuits.Forms
{
    public static partial class FormsBuilder
    {
        public static Form GetDNFForm(List<(IElement, int outputResult)> registry)
        {
            List<List<int>> truthTable = GetTruthTable(registry, out List<string> columnNames);
            int cols = truthTable[0].Count;
            int rows = truthTable.Count;

            Form form = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                Icon = Properties.Resources.diagram,
                Text = "ДДНФ",
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                Tag = (truthTable, columnNames),
                Width = 450,
                Height = 151,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MaximizeBox = false, 
                AutoScroll = true,
            };

            TextBox textBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
            };
            form.Controls.Add(textBox);

            return form;
        }
    }
}
