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
            int inputsCount = 0;
            List<Input> inputs = new List<Input>();
            string text = string.Empty;
            for (int i = 0; i < registry.Count; i++)
            {
                if (registry[i].Item1 is Input)
                {
                    inputsCount++;
                    inputs.Add((Input)registry[i].Item1);
                    text += inputs.Last().Name + ", ";
                }
            }
            MessageBox.Show(inputsCount.ToString() + " counted, " + text);

            return form;
        }
    }
}
