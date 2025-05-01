using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicCircuits.Elements.Gates
{
    [Serializable]
    internal class NOT : IGate
    {
        public Point Location { get; set; }

        public List<IOutputContainingElement> Inputs { get; set; } = new List<IOutputContainingElement>();
        public InputsMultiplicity InputsMultiplicity { get; } = InputsMultiplicity.Single;
        public IInputContainingElement Output { get; set; }

        [NonSerialized]
        private List<Control> controls;
        public List<Control> Controls { get => controls; set => controls = value; }
        public NOT()
        {
            controls = new List<Control>();
        }

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
            if (Inputs.Count != 1)
            {
                register.Add((this, -1));
                return -1;
            }

            int input = Inputs[0].CalculateOutput(register);
            if (input == -1)
            {
                register.Add((this, -1));
                return -1;
            }

            int output = input == -1 ? -1 : input == 0 ? 1 : 0;
            register.Add((this, output));
            return output;
        }

        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "NOT or Inverter",
                Formula = Properties.Resources.formula2,
                Diagram = Properties.Resources.gate2,
                TruthTable = Properties.Resources.table2,
            };
        }
    }
}
