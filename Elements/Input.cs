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
        public Point Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Image Diagram => throw new NotImplementedException();


        public IInputContainingElement Output { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Connect(IInputContainingElement elementForOutput)
        {
            throw new NotImplementedException();
        }
    }
}
