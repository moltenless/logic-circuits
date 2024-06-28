using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements
{
    internal class Output : IElement, IInputContainingElement
    {
        public Point Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Image Diagram => throw new NotImplementedException();



        public List<IOutputContainingElement> Inputs => throw new NotImplementedException();

        public InputsMultiplicity InputsMultiplicity => throw new NotImplementedException();
    }
}
