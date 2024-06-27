using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class XNOR : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.xnor;





        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "XNOR (Exclusive NOR)",
                Formula = Properties.Resources.formula8,
                Diagram =     Properties.Resources.gate8,
                TruthTable = Properties.Resources.table8,
            };
        }
    }
}
