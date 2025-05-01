﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicCircuits.Elements.Gates
{
    [Serializable]
    internal class IMPLY : IGate
    {
        public Point Location { get; set; }


        public List<IOutputContainingElement> Inputs { get; set; } = new List<IOutputContainingElement>();
        public InputsMultiplicity InputsMultiplicity { get; } = InputsMultiplicity.Double;
        public IInputContainingElement Output { get; set; }

        [NonSerialized]
        private List<Control> controls;
        public List<Control> Controls { get => controls; set => controls = value; }
        public IMPLY()
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

            int output = input1 == 1 && input2 == 0 ? 0 : 1;

            register.Add((this, output));
            return output;
        }

        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "IMPLY or Material implication",
                Formula = Properties.Resources.formula9,
                Diagram =     Properties.Resources.gate9,
                TruthTable = Properties.Resources.table9,
            };
        }
    }
}
