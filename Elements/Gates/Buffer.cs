using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class Buffer : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.buffer;

        public List<IOutputContainingElement> Inputs { get; set; } = new List<IOutputContainingElement>();
        public InputsMultiplicity InputsMultiplicity { get; } = InputsMultiplicity.Single;
        public IInputContainingElement Output { get; set; } = null;

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
                Name = "Buffer",
                Formula = Properties.Resources.formula1,
                Diagram = Properties.Resources.gate1,
                TruthTable = Properties.Resources.table1,
            };
        }
    }
}
