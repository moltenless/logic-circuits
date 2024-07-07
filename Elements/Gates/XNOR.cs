using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class XNOR : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.xnor;

        public List<IOutputContainingElement> Inputs { get; set; } = new List<IOutputContainingElement>();
        public InputsMultiplicity InputsMultiplicity { get; } = InputsMultiplicity.Double;
        public IInputContainingElement Output { get; set; }

        public bool Connect(IInputContainingElement elementToConnectWith)
        {
            if (elementToConnectWith == this || Output != null) return false;

            if (elementToConnectWith.InputsMultiplicity == InputsMultiplicity.Single && elementToConnectWith.Inputs.Count != 0) return false;
            if (elementToConnectWith.InputsMultiplicity == InputsMultiplicity.Double && elementToConnectWith.Inputs.Count > 1) return false;

            Output = elementToConnectWith;
            elementToConnectWith.Inputs.Add(this);
            return true;
        }

        public int CalculateOutput(List<(IElement, int outputResult)> register)
        {
            if (Inputs.Count != 2)
            {
                register.Add((this, -1));
                return -1;
            }

            int input1 = Inputs[0].CalculateOutput(register);
            int input2 = Inputs[1].CalculateOutput(register);
            if (input1 == -1 || input2 == -1)
            {
                register.Add((this, -1));
                return -1;
            }

            int output = input1 == input2 ? 1 : 0;

            register.Add((this, output));
            return output;
        }

        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "XNOR (Exclusive NOR)",
                Formula = Properties.Resources.formula8,
                Diagram =     Properties.Resources.gate8,
                TruthTable = Properties.Resources.table8,
            };
        }
    }
}
