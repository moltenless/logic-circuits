using LogicCircuits.Elements;
using LogicCircuits.Elements.Interfaces;
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
        public static Form GetCNFForm(List<(IElement, int outputResult)> registry)
        {
            List<List<int>> truthTable = GetTruthTable(registry, out List<string> columnNames);
            int cols = truthTable[0].Count;
            int rows = truthTable.Count;

            Form form = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                Icon = Properties.Resources.diagram,
                Text = "ДКНФ",
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                Tag = (truthTable, columnNames),
                Width = 750,
                Height = 400,
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

            string prefix = $"{columnNames[cols - 1]}({columnNames[0]}";
            for (int i = 1; i < cols - 1; i++)
                prefix += $",{columnNames[i]}";
            prefix += ") = ";

            string function = "(";
            bool firstconj = true;
            for (int i = 0; i < rows; i++)
            {
                if (truthTable[i][cols - 1] == 0)
                {
                    if (!firstconj)
                        function += ") \r\n˄  (";

                    bool firstparam = true;
                    for (int j = 0; j < cols - 1; j++)
                    {
                        if (truthTable[i][j] == 1)
                        {
                            if (!firstparam) function += " ˅ ";
                            function += $"¬{columnNames[j]}";
                            firstparam = false;
                        }
                        else
                        {
                            if (!firstparam) function += " ˅ ";
                            function += columnNames[j];
                            firstparam = false;
                        }
                    }
                    firstconj = false;
                }
            }
            function += ")";

            List<List<int>> minterms = new List<List<int>>();
            for (int i = 0; i < rows; i++)
                if (truthTable[i][cols - 1] == 1)
                    minterms.Add(truthTable[i]);

            if (minterms.Count == 0) textBox.Text = prefix + "0";
            else if (minterms.Count == rows) textBox.Text = prefix + "1";
            else
                textBox.Text = prefix + function;
            textBox.Select(0, 0);

            return form;
        }
    }
}
