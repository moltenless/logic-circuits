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
            RetrieveDataFromRegistry(registry, out int inputsCount, out List<(Input input, bool slave)> inputs, out Output output);

            int[] inputsHistory = new int[inputsCount];
            for (int i = 0; i < inputsCount; i++)
                inputsHistory[i] = inputs[i].input.Value;

            bool[,] parameters = GetAllParametersSetsWithSlaves(inputsCount);

            List<List<int>> truthTable = LookOverEachSet(inputsCount, inputs, parameters, output);

            for (int i = 0; i < inputsCount; i++)
                inputs[i].input.Value = inputsHistory[i];

            columnNames = new List<string>();
            for (int j = 0; j < inputsCount; j++)
                if (!inputs[j].slave)
                    columnNames.Add(inputs[j].input.Name);
            columnNames.Add(output.Name);

            return truthTable;
        }

        public static void RetrieveDataFromRegistry(List<(IElement, int outputResult)> registry, out int inputsCount, out List<(Input input, bool slave)> inputs, out Output output)
        {
            (IElement, int outputResult)[] copiedRegistry = new (IElement, int outputResult)[registry.Count];
            registry.CopyTo(copiedRegistry);

            inputsCount = 0;
            inputs = new List<(Input input, bool slave)>();
            output = copiedRegistry.Last().Item1 as Output;

            for (int i = 0; i < copiedRegistry.Length; i++)
            {
                if (copiedRegistry[i].Item1 is Input)
                {
                    inputsCount++;
                    inputs.Add(((Input)copiedRegistry[i].Item1, (copiedRegistry[i].Item1 as Input).IsSlave));
                }
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

        public static List<List<int>> LookOverEachSet(int inputsCount, List<(Input input, bool slave)> inputs, bool[,] parameters, Output output)
        {
            List<(IElement, int outputResult)> bufferRegistry = new List<(IElement, int outputResult)>();
            List<List<int>> table = new List<List<int>>();

            for (int i = 0; i < (int)Math.Pow(2, inputs.Count(inp => { return !inp.slave; })); i++)
            {
                table.Add(new List<int>());

                for (int j = 0; j < inputsCount; j++)
                    if (!inputs[j].slave)
                    {
                        inputs[j].input.Value = parameters[i, j] ? 1 : 0;
                        table.Last().Add(inputs[j].input.Value);
                    }

                for (int j = 0; j < inputsCount; j++)
                    if (inputs[j].slave)
                        inputs[j].input.Value = inputs[j].input.Supervisor.Value;

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
