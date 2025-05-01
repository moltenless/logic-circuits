using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuits.Elements.Interfaces
{
    public interface IOutputContainingElement : IElement
    {
        IInputContainingElement Output { get; set; }
        bool Connect(IInputContainingElement elementToConnectWith);
    }
}
