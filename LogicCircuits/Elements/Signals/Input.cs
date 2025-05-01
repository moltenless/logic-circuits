﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicCircuits.Elements
{
    [Serializable]
    public class Input : IOutputContainingElement
    {
        public Point Location { get; set; }
        public string Name { get; set; } = null;
        public IInputContainingElement Output { get; set; } = null;
        public int Value { get; set; } = 0;

        public bool IsSupervisor { get; set; } = false;
        public List<Input> AdditionalOutputs { get; set; } = new List<Input>();
        public bool IsSlave { get; set; } = false;
        public Input Supervisor { get; set; } = null;
        public int NumberAsAdditional { get; set; }

        [NonSerialized]
        private List<Control> controls;
        public List<Control> Controls { get => controls; set => controls = value; }


        public Input(string name)
        {
            controls = new List<Control>();
            Name = name;
        }

        public bool Connect(IInputContainingElement elementToConnectWith)
        {
            if (Output != null) return false;

            if (elementToConnectWith.InputsMultiplicity == InputsMultiplicity.Single && elementToConnectWith.Inputs.Count != 0) return false;
            if (elementToConnectWith.InputsMultiplicity == InputsMultiplicity.Double && elementToConnectWith.Inputs.Count > 1) return false;

            Output = elementToConnectWith;
            elementToConnectWith.Inputs.Add(this);
            return true;
        }

        public int CalculateOutput(List<(IElement, int outputResult)> register)
        {
            register.Add((this, Value));
            return Value;
        }
    }
}
