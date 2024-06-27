using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Gates
{
    internal class AND : IGate
    {

        public static GateInfo GetInfo()
        {
            return new GateInfo
            {
                Name = "AND",
                Formula = Properties.Resources.formula3,
                Diagram =     Properties.Resources.gate3,
                TruthTable =  Properties.Resources.table3
            };
        }
    }
}
