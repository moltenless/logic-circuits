using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class NAND : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.nand;


        public List<IOutputContainingElement> Inputs { get; set; } = new List<IOutputContainingElement>();
        public InputsMultiplicity InputsMultiplicity { get; } = InputsMultiplicity.Multiple;
        public bool SetNewInput(IOutputContainingElement elementForInput)
        {
            if (elementForInput == this || elementForInput.Output != null) return false;

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
                Name = "NAND (NOT-AND)",
                Formula = Properties.Resources.formula5,
                Diagram =     Properties.Resources.gate5,
                TruthTable = Properties.Resources.table5,
            };
        }
    }
}
