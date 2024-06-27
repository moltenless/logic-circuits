using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class Buffer : IGate
    {
        public Point Location { get; set; }
        public Image Diagram { get; } = Properties.Resources.buffer;



        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "Buffer",
                Formula = Properties.Resources.formula1,
                Diagram = Properties.Resources.gate1,
                TruthTable = Properties.Resources.table1,
            };
        }
    }
}
