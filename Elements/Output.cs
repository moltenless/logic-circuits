using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements
{
    internal class Output : IInputContainingElement
    {
        public Point Location { get; set; }

        public Image Diagram { get; } = Properties.Resources._out;

        public string Name { get; } = null;

        public List<IOutputContainingElement> Inputs { get; set; } = new List<IOutputContainingElement>();

        public int? Result { get; set; } = null;

        public Output(string name)
        {
            Name = name;
        }

        public InputsMultiplicity InputsMultiplicity => InputsMultiplicity.Single;
    }
}
