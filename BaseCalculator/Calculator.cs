using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCalculator
{
    public class Calculator
    {
        public delegate string OnRequestVariableValue(string name);

        private List<Node> NodeChain = new List<Node>();
        private List<Node> BSEChain;
        private String specialSymbol = "+-/*%^()";
        public String Solve(String expression)
        {

            Reset();
            String sub_expression = "";
            char c;
            for (int position = 0; position < expression.Length; position++)
            {
                c = expression[(position)];
                if (specialSymbol.Contains(c.ToString()))
                {
                    if (sub_expression.Length != 0)

                        addDigit(position - 1 - sub_expression.Length, sub_expression);
                    NodeChain.Add(new SymbolNode(c.ToString()));
                    sub_expression = "";
                }
                else
                {
                    sub_expression += c;
                }
            }
            if (!(sub_expression.Length == 0))

                addDigit(expression.Length - sub_expression.Length, sub_expression);

            CheckExpressionChain();
            try
            {

                ConverToBackSymbolExpression();
            }
            catch (Exception e)
            {
                throw new InvaildOperationException(-1, e.Message);
            }
            return ExecuteBSE();
        }

        private void CheckExpressionChain()
        {
            try
            {
                if (NodeChain.Count == 0)
                    throw new Exception("no any Node in chain list.");
                if (NodeChain.Count == 1 && (NodeChain[(0)].GetNodeType() == Node.NodeType.Symbol))
                    throw new Exception("invalid chain list because size is 1 and  the first of node of type is Symbol.");
                Node previewNode = null;
                foreach (Node node in NodeChain)
                {
                    node.Check();
                    /*鐣欏潙
                    if(previewNode!=null)
                        if(previewNode.GetNodeType()==node.GetNodeType())
                            throw new Exception("two same symbol is close");*/
                    previewNode = node;
                }
                if (previewNode.GetNodeType() == Node.NodeType.Symbol ? (((SymbolNode)previewNode).GetSymbolType() != SymbolNode.SymbolType.Bracket_Right) : false)
                    throw new Exception("the end of chain list is symbol");
            }
            catch (Exception e)
            {
                throw new InvaildOperationException(-1, e.Message);
            }
        }

        private void ConverToBackSymbolExpression()
        {
            List<Node> result_list = new List<Node>();
            Stack<SymbolNode> operation_stack = new Stack<SymbolNode>();
            SymbolNode symbol = null;
            foreach (Node _node in NodeChain)
            {
                if (_node.GetNodeType() == Node.NodeType.Digit)
                    result_list.Add(_node);
                else
                {
                    if (operation_stack.Count == 0)
                        operation_stack.Push((SymbolNode)_node);
                    else
                    {
                        if (((SymbolNode)_node).symbol_type != SymbolNode.SymbolType.Bracket_Right)
                        {
                            symbol = operation_stack.Peek();
                            while ((symbol == null ? false : (symbol.symbol_type != SymbolNode.SymbolType.Bracket_Left && symbol.CompareToOperationPriority((SymbolNode)_node) >= 0)))
                            {
                                result_list.Add(operation_stack.Pop());
                                symbol = operation_stack.Count != 0 ? operation_stack.Peek() : null;
                            }
                            operation_stack.Push((SymbolNode)_node);
                        }
                        else
                        {
                            symbol = operation_stack.Peek();
                            while (true)
                            {
                                if (operation_stack.Count == 0)
                                    throw new Exception("no left bracket and take a pair of close bracket.");
                                if (symbol.symbol_type == SymbolNode.SymbolType.Bracket_Left)
                                {
                                    operation_stack.Pop();
                                    break;
                                }
                                result_list.Add(operation_stack.Pop());
                                symbol = operation_stack.Peek();
                            }
                        }
                    }
                }
            }
            while (!(operation_stack.Count == 0))
            {
                result_list.Add(operation_stack.Pop());
            }
            Node node;
            for (int i = 0; i < result_list.Count; i++)
            {
                node = result_list[(i)];
                if (node.GetNodeType() == Node.NodeType.Symbol)
                    if (((SymbolNode)node).symbol_type == SymbolNode.SymbolType.Bracket_Left)
                        result_list.Remove(node);
            }
            BSEChain = result_list;
        }

        private String ExecuteBSE()
        {
            if (BSEChain.Count == 1)
                if (BSEChain[(0)].GetNodeType() == Node.NodeType.Digit)
                    return (((((DigitNode)BSEChain[(0)]).isFloat() ? ((DigitNode)BSEChain[(0)]).toFloat() : ((DigitNode)BSEChain[(0)]).toInteger()))).ToString();
            Stack<DigitNode> digit_stack = new Stack<DigitNode>();
            DigitNode digit_a, digit_b, digit_result;
            SymbolNode _operator;
            try
            {
                foreach (Node node in BSEChain)
                {
                    if (node.GetNodeType() == Node.NodeType.Symbol)
                    {
                        _operator = (SymbolNode)node;
                        digit_b = digit_stack.Pop();
                        digit_a = digit_stack.Pop();
                        digit_result = Execute(digit_a, _operator, digit_b);
                        digit_stack.Push(digit_result);
                    }
                    else
                    {
                        if (node.GetNodeType() == Node.NodeType.Digit)
                        {
                            digit_stack.Push((DigitNode)node);
                        }
                        else
                            throw new Exception("Unknown Node");
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvaildOperationException(-1, e.Message);
            }
            return digit_stack.Pop().rawText;
        }

        private DigitNode Execute(DigitNode a, SymbolNode op, DigitNode b)
        {
            if (op.symbol_type == SymbolNode.SymbolType.Add)
                return new DigitNode((a.toValue() + b.toValue()).ToString());
            if (op.symbol_type == SymbolNode.SymbolType.Multiply)
                return new DigitNode((a.toValue() * b.toValue()).ToString());
            if (op.symbol_type == SymbolNode.SymbolType.Subtract)
                return new DigitNode((a.toValue() - b.toValue()).ToString());
            if (op.symbol_type == SymbolNode.SymbolType.Mod)
                return new DigitNode((((a.isFloat() ? a.toFloat() : a.toInteger()) % (b.isFloat() ? b.toFloat() : b.toInteger()))).ToString());
            if (op.symbol_type == SymbolNode.SymbolType.Divide)
                return new DigitNode((((a.isFloat() ? a.toFloat() : a.toInteger()) / (b.isFloat() ? b.toFloat() : b.toInteger()))).ToString());
            if (op.symbol_type == SymbolNode.SymbolType.Power)
                return new DigitNode(((Math.Pow((a.isFloat() ? a.toFloat() : a.toInteger()), (b.isFloat() ? b.toFloat() : b.toInteger())))).ToString());
            if (op.symbol_type == SymbolNode.SymbolType.Unknown)
                throw new Exception("Unknown Operator");

            return null;
        }

        private void addDigit(int position, String sub_expression)
        {
            if (isDigit(sub_expression))
                NodeChain.Add(new DigitNode(sub_expression));
            else
            if (isValidVariable(sub_expression))
                NodeChain.Add(new DigitNode(requestVariable(sub_expression)));
            else
                throw new InvaildOperationException(position, String.Format("{0} is invalid variable in table", sub_expression));
        }

        private OnRequestVariableValue requestVariableValue;
        public void setRequestVariableValue(OnRequestVariableValue r) { requestVariableValue = r; }
        private String requestVariable(String name) { return requestVariableValue?.Invoke(name); }

        public static bool isDigit(String expression)
        {
            if (expression.Length == 0)
                return false;
            foreach (char c in expression)
            {
                if (!(Char.IsDigit(c) || ((c == '.'))))
                    return false;
            }
            return true;
        }

        public static bool isValidVariable(String expression)
        {
            if (expression.Length == 0)
                return false;
            if (isDigit((expression[(0)]).ToString()))
                return false;
            if (isDigit(expression))
                return false;
            foreach (char c in expression)
                if (!(Char.IsLetterOrDigit(c) || ((c == '_'))))
                    return false;
            return true;
        }

        public void Reset()
        {
            if (BSEChain != null)
                BSEChain.Clear();
            if (NodeChain != null)
                NodeChain.Clear();
        }
    }
}
