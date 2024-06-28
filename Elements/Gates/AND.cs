using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class AND : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.and;


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
                Name = "AND",
                Formula = Properties.Resources.formula3,
                Diagram =     Properties.Resources.gate3,
                TruthTable =  Properties.Resources.table3,
            };
        }
    }
}
