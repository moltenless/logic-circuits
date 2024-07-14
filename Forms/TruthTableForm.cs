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

            int[,] truthTable = GetTruthTable(registry, out string[] columnNames);

            DataGridView grid = GetFilledGridView(truthTable, columnNames);
            form.Controls.Add(grid);

            form.Size = grid.Size;

            return form;
        }

        private static int[,] GetTruthTable(List<(IElement, int outputResult)> registry, out string[] columnNames)
        {
            RetrieveDataFromRegistry(registry, out int inputsCount, out List<Input> inputs, out Output output);

            int[] inputsHistory = new int[inputsCount];
            for (int i = 0; i < inputsCount; i++)
                inputsHistory[i] = inputs[i].Value;

            bool[,] parameters = GetAllParametersSets(inputsCount);

            int[] results = LookOverEachSet(inputsCount, inputs, parameters, output);

            int[,] truthTable = new int[(int)Math.Pow(2, inputsCount), inputsCount + 1];
            for (int i = 0; i < (int)Math.Pow(2, inputsCount); i++)
            {
                for (int j = 0; j < inputsCount; j++)
                    truthTable[i, j] = parameters[i, j] ? 1 : 0;
                truthTable[i, inputsCount] = results[i];
            }

            for (int i = 0; i < inputsCount; i++)
                inputs[i].Value = inputsHistory[i];

            columnNames = new string[inputsCount + 1];
            for (int i = 0; i < inputsCount; i++)
                columnNames[i] = inputs[i].Name;
            columnNames[inputsCount] = output.Name;

            return truthTable;
        }

        public static void RetrieveDataFromRegistry(List<(IElement, int outputResult)> registry, out int inputsCount, out List<Input> inputs, out Output output)
        {
            (IElement, int outputResult)[] copiedRegistry = new (IElement, int outputResult)[registry.Count];
            registry.CopyTo(copiedRegistry);

            inputsCount = 0;
            inputs = new List<Input>();
            output = copiedRegistry.Last().Item1 as Output;

            for (int i = 0; i < copiedRegistry.Length; i++)
            {
                if (copiedRegistry[i].Item1 is Input)
                {
                    inputsCount++;
                    inputs.Add((Input)copiedRegistry[i].Item1);
                }
            }
        }

        public static bool[,] GetAllParametersSets(int inputsCount)
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
                        else if (j % 2 == 1)
                            parameters[j * groupsBy + k, i] = false;
                }
            }

            return parameters;
        }

        public static int[] LookOverEachSet(int inputsCount, List<Input> inputs, bool[,] parameters, Output output)
        {
            List<(IElement, int outputResult)> bufferRegistry = new List<(IElement, int outputResult)>();
            int[] results = new int[(int)Math.Pow(2, inputsCount)];

            for (int i = 0; i < results.Length; i++)
            {
                for (int j = 0; j < inputsCount; j++)
                    inputs[j].Value = parameters[i, j] ? 1 : 0;

                bufferRegistry.Clear();
                int result = output.CalculateOutput(bufferRegistry);
                results[i] = result;
            }

            return results;
        }

        public static DataGridView GetFilledGridView(int[,] truthTable, string[] columnNames)
        {
            DataGridView grid = new DataGridView();
            grid.Dock = DockStyle.Fill;

            int cols = truthTable.GetLength(1);
            int rows = truthTable.GetLength(0);
            grid.ColumnCount = cols;

            for (int i = 0; i < rows; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(grid);

                for (int j = 0; j < cols; j++)
                    row.Cells[j].Value = truthTable[i, j];

                grid.Rows.Add(row);
            }

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeight = 35;
            

            for (int i = 0; i < cols; i++)
                grid.Columns[i].Name = columnNames[i];

            return grid;
        }
    }
}
