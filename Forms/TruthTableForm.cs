using LogicCircuits.Elements;
using Microsoft.Win32;
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
        public static Form GetTruthTableForm(List<(IElement, int outputResult)> registry)
        {
            Form form = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                Icon = Properties.Resources.diagram,
                Text = "Таблиця істинності",
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
            };

            List<List<int>> truthTable = GetTruthTable(registry, out List<string> columnNames);

            DataGridView grid = GetFilledGridView(truthTable, columnNames);
            form.Controls.Add(grid);

            form.Size = grid.Size;

            return form;
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

        public static DataGridView GetFilledGridView(List<List<int>> truthTable, List<string> columnNames)
        {
            DataGridView grid = new DataGridView();
            grid.Dock = DockStyle.Fill;

            int cols = truthTable[0].Count;
            int rows = truthTable.Count;
            grid.ColumnCount = cols;

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
                            row.Cells[j].Style.ForeColor = Color.Blue;
                        }
                        else
                        {
                            row.Cells[j].Style.BackColor = Color.LightGoldenrodYellow;
                            row.Cells[j].Style.ForeColor = Color.Yellow;
                        }
                    else if (truthTable[i][j] == 1)
                        if (j == cols - 1)
                        {
                            row.Cells[j].Style.BackColor = Color.Blue;
                            row.Cells[j].Style.ForeColor = Color.Yellow;
                        }
                        else
                        {
                            row.Cells[j].Style.BackColor = Color.LightSteelBlue;
                            row.Cells[j].Style.ForeColor = Color.DarkBlue;
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

            return grid;
        }
    }
}
