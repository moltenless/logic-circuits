using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements
{
    internal interface IInputContainingElement : IElement
    {
        List<IOutputContainingElement> Inputs { get; }
        InputsMultiplicity InputsMultiplicity { get; }
    }
}
