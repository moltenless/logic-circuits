using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class OR : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.or;







        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "OR",
                Formula = Properties.Resources.formula4,
                Diagram =     Properties.Resources.gate4,
                TruthTable = Properties.Resources.table4,
            };
        }
    }
}
