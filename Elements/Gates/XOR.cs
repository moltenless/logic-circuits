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
