using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class NOR : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.nor;


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


        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "NOR (NOT-OR)",
                Formula = Properties.Resources.formula6,
                Diagram =     Properties.Resources.gate6,
                TruthTable = Properties.Resources.table6,
            };
        }
    }
}
