using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class OR : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.or;

        public List<IOutputContainingElement> Inputs { get; set; } = new List<IOutputContainingElement>();
        public InputsMultiplicity InputsMultiplicity { get; } = InputsMultiplicity.Multiple;
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
            if (Inputs.Count < 2)
            {
                register.Add((this, -1));
                return -1;
            }

            int[] inputs = new int[Inputs.Count];
            for (int i = 0; i < Inputs.Count; i++)
            {
                inputs[i] = Inputs[i].CalculateOutput(register);
                if (inputs[i] == -1)
                {
                    register.Add((this, -1));
                    return -1;
                }
            }

            for (int i = 0; i < inputs.Length; i++)
                if (inputs[i] == 1)
                {
                    register.Add((this, 1));
                    return 1;
                }

            register.Add((this, 0));
            return 0;
        }

        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "OR",
                Formula = Properties.Resources.formula4,
                Diagram =     Properties.Resources.gate4,
                TruthTable = Properties.Resources.table4,
            };
        }
    }
}
