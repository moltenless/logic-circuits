using LogicCircuits.Elements.Gates;
using LogicCircuits.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements
{
    public static class DiagramImages
    {
        internal static Image GetDiagram(IElement element)
        {
            if (element is AND)
                return Properties.Resources.and;
            else if (element is Gates.Buffer)
                return Properties.Resources.buffer;
            else if (element is IMPLY)
                return Properties.Resources.imply;
            else if (element is NAND)
                return Properties.Resources.nand;
            else if (element is NOR)
                return Properties.Resources.nor;
            else if (element is NOT)
                return Properties.Resources.not;
            else if (element is OR)
                return Properties.Resources.or;
            else if (element is XNOR)
                return Properties.Resources.xnor;
            else if (element is XOR)
                return Properties.Resources.xor;
            else if (element is Input input)
            {
                if (input.IsSlave)
                    return Properties.Resources.additional_input;
                else
                    return Properties.Resources._in;
            }
            else if (element is Output)
                return Properties.Resources._out;
            else throw new ArgumentException();
        }
    }
}
