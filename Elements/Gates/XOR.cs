using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class XOR : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.xor;

        public List<IOutputContainingElement> Inputs { get; set; } = new List<IOutputContainingElement>();
        public InputsMultiplicity InputsMultiplicity { get; } = InputsMultiplicity.Double;
        public bool SetNewInput(IOutputContainingElement elementForInput)
        {
            if (Inputs.Count > 1 || elementForInput == this || elementForInput.Output != null) return false;

            Inputs.Add(elementForInput);
            elementForInput.Output = this;
            return true;
        }

        public IInputContainingElement Output { get; set; }
        public bool SetOutput(IInputContainingElement elementForOutput)
        {
            return elementForOutput.SetNewInput(this);
        }



        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "XOR (Exclusive OR)",
                Formula = Properties.Resources.formula7,
                Diagram =     Properties.Resources.gate7,
                TruthTable = Properties.Resources.table7,
            };
        }
    }
}
