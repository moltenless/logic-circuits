using LogicCircuits.Elements.Interfaces;
using System.Collections.Generic;
using System.Drawing;
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

            string dnf = GetDNF(truthTable, columnNames, cols, rows);
                textBox.Text = dnf;
            textBox.Select(0, 0);

            return form;
        }

        public static string GetDNF(List<List<int>> truthTable, List<string> columnNames, int cols, int rows)
        {
            string prefix = $"{columnNames[cols - 1]}({columnNames[0]}";
            for (int i = 1; i < cols - 1; i++)
                prefix += $",{columnNames[i]}";
            prefix += ") = ";

            string function = string.Empty;
            bool firstconj = true;
            for (int i = 0; i < rows; i++)
            {
                if (truthTable[i][cols - 1] == 1)
                {
                    if (!firstconj)
                        function += " ˅   ";

                    bool firstparam = true;
                    for (int j = 0; j < cols - 1; j++)
                    {
                        if (truthTable[i][j] == 1)
                        {
                            if (!firstparam) function += " ˄ ";
                            function += columnNames[j];
                            firstparam = false;
                        }
                        else
                        {
                            if (!firstparam) function += " ˄ ";
                            function += $"¬{columnNames[j]}";
                            firstparam = false;
                        }
                    }
                    firstconj = false;
                    function += "\r\n";
                }
            }

            List<List<int>> maxterms = new List<List<int>>();
            for (int i = 0; i < rows; i++)
                if (truthTable[i][cols - 1] == 0)
                    maxterms.Add(truthTable[i]);

            if (maxterms.Count == 0) return prefix + "1";
            else if (maxterms.Count == rows) return prefix + "0";
            else
                return prefix + function;
        }
    }
}
