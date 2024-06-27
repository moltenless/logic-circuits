using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class NOR : IGate
    {




        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "NOR (NOT-OR)",
                Formula = Properties.Resources.formula6,
                Diagram =     Properties.Resources.gate6,
                TruthTable = Properties.Resources.table6
            };
        }
    }
}
