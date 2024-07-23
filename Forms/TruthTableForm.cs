using LogicCircuits.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LogicCircuits.Forms
{
    public static partial class FormsBuilder
    {
        public static Form GetTruthTableForm(List<(IElement, int outputResult)> registry)
        {
            List<List<int>> truthTable = GetTruthTable(registry, out List<string> columnNames);
            int cols = truthTable[0].Count;
            int rows = truthTable.Count;

            Form form = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                Icon = Properties.Resources.diagram,
                Text = "Таблиця істинності",
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                Tag = (truthTable, columnNames),
            };

            DataGridView grid = GetFilledGridView(truthTable, columnNames);
            form.Controls.Add(grid);

            Panel panel = GetLowerPanel(truthTable, columnNames);
            form.Controls.Add(panel);

            form.Height = (rows + 7) * 22 > Screen.PrimaryScreen.WorkingArea.Height ? Screen.PrimaryScreen.WorkingArea.Height : (rows + 7) * 22;
            form.Width = (cols + 1) * 55;

            return form;
        }

        static bool columnSelected = false;
        static int selectedColumn = -1;
        private static Panel GetLowerPanel(List<List<int>> truthTable, List<string> columnNames)
        {
            Panel panel = new Panel
            {
                Padding = new Padding(40, 0, 0, 0),
                Dock = DockStyle.Bottom,
                Height = 50,
            };

            Label label = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 18,
                Text = "Виберіть порядок змінних",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font(FontFamily.GenericSansSerif, 10)
            };

            Panel panelButton = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 2, 0, 0),
            };
            panel.Controls.Add(panelButton);
            panel.Controls.Add(label);

            for (int i = 0; i < columnNames.Count - 1; i++)
            {
                Button button = new Button
                {
                    Width = 40,
                    Dock = DockStyle.Left,
                    Text = columnNames[columnNames.Count - 2 - i],
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold),
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Tag = columnNames.Count - 2 - i,
                };
                button.Click += SwitchClick;
                panelButton.Controls.Add(button);
            }
            return panel;
        }

        private static void SwitchClick(object sender, EventArgs e)
        {
            Panel panelButton = (Panel)(sender as Control).Parent;

            if (sender is Button but)
            {
                int curr = (int)but.Tag;
                if (!columnSelected)
                {
                    columnSelected = true;
                    selectedColumn = curr;
                    but.BackColor = Color.Gray;
                }
                else
                {
                    if (curr == selectedColumn)
                    {
                        columnSelected = false;
                        but.BackColor = SystemColors.Control;
                    }
                    else
                    {
                        Button firstSwitcher = null;
                        for (int j = 0; j < panelButton.Controls.Count; j++)
                            if ((int)panelButton.Controls[j].Tag == selectedColumn)
                                firstSwitcher = (Button)panelButton.Controls[j];

                        Form form = but.FindForm();

                        (List<List<int>> table, List<string> names) table = ((List<List<int>>, List<string>))form.Tag;
                        for (int i = 0; i < table.table.Count; i++)
                        {
                            int buffer = table.table[i][selectedColumn];
                            table.table[i][selectedColumn] = table.table[i][curr];
                            table.table[i][curr] = buffer;
                        }
                        string buffer2 = table.names[selectedColumn];
                        table.names[selectedColumn] = table.names[curr];
                        table.names[curr] = buffer2;

                        form.Tag = table;
                        DataGridView grid = GetFilledGridView(table.table, table.names);

                        form.Controls.Remove(panelButton.Parent);
                        for (int i = 0; i < form.Controls.Count; i++)
                            if (form.Controls[i] is DataGridView view)
                                form.Controls.Remove(view);
                        form.Controls.Add(grid);
                        form.Controls.Add(panelButton.Parent);

                        columnSelected = false;
                        string buff = firstSwitcher.Text;
                        firstSwitcher.Text = but.Text;
                        but.Text = buff;

                        firstSwitcher.BackColor = SystemColors.Control;
                    }
                }
            }
        }

        public static DataGridView GetFilledGridView(List<List<int>> truthTable, List<string> columnNames)
        {
            int cols = truthTable[0].Count;
            int rows = truthTable.Count;
            DataGridView grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ColumnCount = cols,
            };

            for (int i = 0; i < rows; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(grid);

                for (int j = 0; j < cols; j++)
                {
                    row.Cells[j].Value = truthTable[i][j];
                    if (truthTable[i][j] == 0)
                        if (j == cols - 1)
                        {
                            row.Cells[j].Style.BackColor = Color.Yellow;
                            row.Cells[j].Style.ForeColor = Color.Black;
                        }
                        else
                        {
                            row.Cells[j].Style.BackColor = Color.LightGoldenrodYellow;
                            row.Cells[j].Style.ForeColor = Color.Black;
                        }
                    else if (truthTable[i][j] == 1)
                        if (j == cols - 1)
                        {
                            row.Cells[j].Style.BackColor = Color.Blue;
                            row.Cells[j].Style.ForeColor = Color.White;
                        }
                        else
                        {
                            row.Cells[j].Style.BackColor = Color.LightSteelBlue;
                            row.Cells[j].Style.ForeColor = Color.White;
                        }
                    row.Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                grid.Rows.Add(row);
            }
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeight = 35;


            grid.EnableHeadersVisualStyles = false;
            for (int i = 0; i < cols; i++)
            {
                grid.Columns[i].Name = columnNames[i];
                grid.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (i == cols - 1)
                {
                    grid.Columns[i].HeaderCell.Style.BackColor = Color.Black;
                    grid.Columns[i].HeaderCell.Style.ForeColor = Color.White;
                }
            }
            grid.CurrentCell = grid.Rows[rows].Cells[0];

            return grid;
        }

        private static List<List<int>> GetTruthTable(List<(IElement, int outputResult)> registry, out List<string> columnNames)
        {
            RetrieveDataFromRegistry(registry, out List<Input> allInputs, out List<Input> notSlaveInputs, out Output output);

            int[] inputsHistory = new int[allInputs.Count];
            for (int i = 0; i < allInputs.Count; i++)
                inputsHistory[i] = allInputs[i].Value;

            bool[,] parameters = GetAllParametersSetsWithSlaves(notSlaveInputs.Count);

            List<List<int>> truthTable = LookOverEachSet(allInputs, notSlaveInputs, parameters, output);

            for (int i = 0; i < allInputs.Count; i++)
                allInputs[i].Value = inputsHistory[i];

            columnNames = new List<string>();
            for (int j = 0; j < allInputs.Count; j++)
                if (!allInputs[j].IsSlave)
                    columnNames.Add(allInputs[j].Name);
            columnNames.Add(output.Name);

            return truthTable;
        }

        public static void RetrieveDataFromRegistry(List<(IElement, int outputResult)> registry, out List<Input> allInputs, out List<Input> notSlaveInputs, out Output output)
        {
            (IElement, int outputResult)[] copiedRegistry = new (IElement, int outputResult)[registry.Count];
            registry.CopyTo(copiedRegistry);

            allInputs = new List<Input>();
            notSlaveInputs = new List<Input>();
            output = copiedRegistry.Last().Item1 as Output;

            for (int i = 0; i < copiedRegistry.Length; i++)
                if (copiedRegistry[i].Item1 is Input input)
                {
                    allInputs.Add(input);

                    if (!input.IsSlave)
                        notSlaveInputs.Add(input);
                }
        }

        public static bool[,] GetAllParametersSetsWithSlaves(int inputsCount)
        {
            bool[,] parameters = new bool[(int)Math.Pow(2, inputsCount), inputsCount];

            for (int i = 0; i < inputsCount; i++)
            {
                int groups = (int)Math.Pow(2, i + 1);
                int groupsBy = (int)Math.Pow(2, inputsCount) / groups;

                for (int j = 0; j < groups; j++)
                {
                    for (int k = 0; k < groupsBy; k++)
                        if (j % 2 == 1)
                            parameters[j * groupsBy + k, i] = true;
                        else if (j % 2 == 0)
                            parameters[j * groupsBy + k, i] = false;
                }
            }

            return parameters;
        }

        public static List<List<int>> LookOverEachSet(List<Input> allInputs, List<Input> notSlaveInputs, bool[,] parameters, Output output)
        {
            List<(IElement, int outputResult)> bufferRegistry = new List<(IElement, int outputResult)>();
            List<List<int>> table = new List<List<int>>();

            for (int i = 0; i < (int)Math.Pow(2, notSlaveInputs.Count); i++)
            {
                table.Add(new List<int>());

                for (int j = 0; j < notSlaveInputs.Count; j++)
                {
                    notSlaveInputs[j].Value = parameters[i, j] ? 1 : 0;
                    table.Last().Add(notSlaveInputs[j].Value);
                }

                for (int j = 0; j < allInputs.Count; j++)
                    if (allInputs[j].IsSlave)
                        allInputs[j].Value = allInputs[j].Supervisor.Value;

                bufferRegistry.Clear();
                int result = output.CalculateOutput(bufferRegistry);
                table.Last().Add(result);
            }

            return table;
        }
    }
}
