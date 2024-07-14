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


            (IElement, int outputResult)[] copiedRegistry = new (IElement, int outputResult)[registry.Count];
            registry.CopyTo(copiedRegistry);

            int inputsCount = 0;
            List<(Input input, int value)> inputs = new List<(Input, int)>();
            (Output output, int result) output = (copiedRegistry.Last().Item1 as Output, copiedRegistry.Last().outputResult);

            for (int i = 0; i < copiedRegistry.Length; i++)
            {
                if (copiedRegistry[i].Item1 is Input)
                {
                    inputsCount++;
                    inputs.Add(((Input)copiedRegistry[i].Item1, copiedRegistry[i].outputResult));
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

            string text = string.Empty;
            for (int i = 0; i < (int)Math.Pow(2, inputsCount); i++)
            {
                for (int j = 0; j < inputsCount; j++)
                    text += parameters[i, j] ? "1\t" : "0\t";
                text += "\n";
            }

            MessageBox.Show(text);

            return form;
        }
    }
}
