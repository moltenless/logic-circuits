using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements
{
    internal class Input : IOutputContainingElement
    {
        public Point Location { get; set; }

        public Image Diagram { get; } = Properties.Resources._input;


        public IInputContainingElement Output { get; set; } = null;

        public bool Connect(IInputContainingElement elementToConnectWith)
        {
            if (Output != null) return false;

            if (elementToConnectWith.InputsMultiplicity == InputsMultiplicity.Single && elementToConnectWith.Inputs.Count != 0) return false;
            if (elementToConnectWith.InputsMultiplicity == InputsMultiplicity.Double && elementToConnectWith.Inputs.Count > 1) return false;

            Output = elementToConnectWith;
            elementToConnectWith.Inputs.Add(this);
            return true;
        }
    }
}
