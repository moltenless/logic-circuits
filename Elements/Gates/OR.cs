using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class OR : IGate
    {

        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "OR",
                Formula = Properties.Resources.formula4,
                Diagram =     Properties.Resources.gate4,
                TruthTable = Properties.Resources.table4
            };
        }
    }
}
