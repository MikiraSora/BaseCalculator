using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCalculator
{
    abstract class Node
    {
        protected String rawValueText;
        internal enum NodeType
        {
            Digit,
            Symbol,
            Unknown
        }
        public virtual NodeType GetNodeType() { return NodeType.Unknown; }
        public virtual void Check() { }
    }
}
