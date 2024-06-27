using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class NOR : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.nor;





        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "NOR (NOT-OR)",
                Formula = Properties.Resources.formula6,
                Diagram =     Properties.Resources.gate6,
                TruthTable = Properties.Resources.table6,
            };
        }
    }
}
