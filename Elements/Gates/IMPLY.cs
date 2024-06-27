using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class IMPLY : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.imply;



        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "IMPLY or Material implication",
                Formula = Properties.Resources.formula9,
                Diagram =     Properties.Resources.gate9,
                TruthTable = Properties.Resources.table9,
            };
        }
    }
}
