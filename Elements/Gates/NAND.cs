using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class NAND : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.nand;




        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "NAND (NOT-AND)",
                Formula = Properties.Resources.formula5,
                Diagram =     Properties.Resources.gate5,
                TruthTable = Properties.Resources.table5,
            };
        }
    }
}
