using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCalculator
{
    class SymbolNode:Node
    {
        public String rawText;
        public enum SymbolType
        {
            Add,//+
            Subtract,//-
            Multiply,//*
            Divide,///
			Power,//^
            Mod,//%
            Bracket_Left,//(
            Bracket_Right,//)
            Unknown
        }

        public override string ToString()
        {
            return rawValueText;
        }

        internal SymbolType symbol_type = SymbolType.Unknown;
        public SymbolType GetSymbolType() { return symbol_type; }

        public SymbolNode(String op)
        {
            rawValueText = op;
            OperationPriority = new Dictionary<SymbolType, float>();
            OperationPriorityInit();
            switch (op[(0)])
            {
                case '+': symbol_type = SymbolType.Add; break;
                case '-': symbol_type = SymbolType.Subtract; break;
                case '*': symbol_type = SymbolType.Multiply; break;
                case '/': symbol_type = SymbolType.Divide; break;
                case '^': symbol_type = SymbolType.Power; break;
                case '%': symbol_type = SymbolType.Mod; break;
                case '(': symbol_type = SymbolType.Bracket_Left; break;
                case ')': symbol_type = SymbolType.Bracket_Right; break;
                default: symbol_type = SymbolType.Unknown;break;
            }
        }

        public override BaseCalculator.Node.NodeType GetNodeType()
        { return Node.NodeType.Symbol; }

        public override void Check()
        {
			if(symbol_type==SymbolType.Unknown)
				throw new Exception("Unknown symbol like "+rawText);
    }

    private Dictionary<SymbolType, float> OperationPriority = new Dictionary<SymbolType, float>();

    private void OperationPriorityInit()
    {
        setOperationPriority(SymbolType.Add, 1);
        setOperationPriority(SymbolType.Subtract, 1);
        setOperationPriority(SymbolType.Divide, 3);
        setOperationPriority(SymbolType.Multiply, 3);
        setOperationPriority(SymbolType.Mod, 3);
        setOperationPriority(SymbolType.Power, 9);
        setOperationPriority(SymbolType.Bracket_Left, 12);
        setOperationPriority(SymbolType.Bracket_Right, 12);
    }

    private void setOperationPriority(SymbolType type, float level)
    {
        OperationPriority.Add(type, level);
    }

    public int CompareToOperationPriority(SymbolNode node)
    {
        float result = this.OperationPriority[symbol_type] - node.OperationPriority[node.symbol_type];
        if (result == 0)
            return 0;
        if (result > 0)
            return 1;
        return -1;
    }


}
}
