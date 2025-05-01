using LogicCircuits.Elements.Interfaces;
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
    internal class AND : IGate
    {
        public Point Location { get; set; }


        public List<IOutputContainingElement> Inputs { get; set; } = new List<IOutputContainingElement>();
        public InputsMultiplicity InputsMultiplicity { get; } = InputsMultiplicity.Multiple;
        public IInputContainingElement Output { get; set; }

        [NonSerialized]
        private List<Control> controls;
        public List<Control> Controls { get => controls; set => controls = value; }
        public AND()
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
                if (inputs[i] == 0)
                {
                    register.Add((this, 0));
                    return 0;
                }

            register.Add((this, 1));
            return 1;
        }

        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "AND",
                Formula = Properties.Resources.formula3,
                Diagram = Properties.Resources.gate3,
                TruthTable = Properties.Resources.table3,
            };
        }
    }
}
