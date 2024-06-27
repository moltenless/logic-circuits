using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class Buffer : IGate
    {


        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "Buffer",
                Formula = Properties.Resources.formula1,
                Diagram = Properties.Resources.gate1,
                TruthTable = Properties.Resources.table1
            };
        }
    }
}
