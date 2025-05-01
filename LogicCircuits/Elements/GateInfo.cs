using LogicCircuits.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements
{
    internal class GateInfo
    {
        public string Name { get; set; }
        public Image Formula { get; set;  }
        public Image Diagram { get; set; }
        public Image TruthTable { get; set; }
    }
}
