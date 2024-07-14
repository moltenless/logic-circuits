using LogicCircuits.Elements;
using System;
using System.Collections.Generic;
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

            };

            int[,] truthTable = GetTruthTable(registry);

            DataGridView grid = new DataGridView();

            int cols = truthTable.GetLength(1);
            int rows = truthTable.GetLength(0);
            grid.ColumnCount = cols;
            grid.RowCount = rows;

            for (int i = 0; i < rows; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(grid);

                for (int j = 0; j < cols; j++)
                {
                    row.Cells[j].Value = truthTable[i, j];
                }

                grid.Rows.Add(row);
            }

            form.Controls.Add(grid);

            return form;
        }

        private static int[,] GetTruthTable(List<(IElement, int outputResult)> registry)
        {
            (IElement, int outputResult)[] copiedRegistry = new (IElement, int outputResult)[registry.Count];
            registry.CopyTo(copiedRegistry);

            int inputsCount = 0;
            List<Input> inputs = new List<Input>();
            Output output = copiedRegistry.Last().Item1 as Output;

            for (int i = 0; i < copiedRegistry.Length; i++)
            {
                if (copiedRegistry[i].Item1 is Input)
                {
                    inputsCount++;
                    inputs.Add((Input)copiedRegistry[i].Item1);
                }
            }

            bool[,] parameters = new bool[(int)Math.Pow(2, inputsCount), inputsCount];

            for (int i = 0; i < inputsCount; i++)
            {
                int groups = (int)Math.Pow(2, i + 1);
                int groupsBy = (int)Math.Pow(2, inputsCount) / groups;

                for (int j = 0; j < groups; j++)
                {
                    for (int k = 0; k < groupsBy; k++)
                    {
                        if (j % 2 == 1)
                            parameters[j * groupsBy + k, i] = true;
                        else if (j % 2 == 1)
                            parameters[j * groupsBy + k, i] = false;
                    }
                }
            }

            List<(IElement, int outputResult)> bufferRegistry = new List<(IElement, int outputResult)>();
            int[] results = new int[(int)Math.Pow(2, inputsCount)];

            for (int i = 0; i < results.Length; i++)
            {
                for (int j = 0; j < inputsCount; j++)
                {
                    inputs[j].Value = parameters[i, j] ? 1 : 0;
                }

                bufferRegistry.Clear();
                int result = output.CalculateOutput(bufferRegistry);
                results[i] = result;
            }

            int[,] truthTable = new int[(int)Math.Pow(2, inputsCount), inputsCount + 1];
            for (int i = 0; i < (int)Math.Pow(2, inputsCount); i++)
            {
                for (int j = 0; j < inputsCount; j++)
                    truthTable[i, j] = parameters[i, j] ? 1 : 0;
                truthTable[i, inputsCount] = results[i];
            }

            return truthTable;
        }
    }
}
