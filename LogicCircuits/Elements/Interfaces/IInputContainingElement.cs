using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Interfaces
{
    public interface IInputContainingElement : IElement
    {
        List<IOutputContainingElement> Inputs { get; }
        InputsMultiplicity InputsMultiplicity { get; }
    }
}
