using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements
{
    internal interface IElement
    {
        Point Location { get; set; }
        Image Diagram { get; }
        int CalculateOutput(List<(IElement, int outputResult)> register);
    }
}
