using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class NOT : IGate
    {

        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "NOT or Inverter",
                Formula = Properties.Resources.formula2,
                Diagram = Properties.Resources.gate2,
                TruthTable = Properties.Resources.table2
            };
        }
    }
}
