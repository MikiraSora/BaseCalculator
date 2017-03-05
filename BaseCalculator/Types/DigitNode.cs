using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCalculator
{
    class DigitNode : Node
    {
        internal String rawText;
        enum DigitType
        {
            Float,
            Integer,
            Unknown
        }

        public override String ToString()
        {
            return rawText;
        }
        DigitType digit_type = DigitType.Unknown;
        public DigitNode(String digit)
        {
            rawText = digit;
            if (isFloat(digit))
                digit_type = DigitType.Float;
            else
                    if (isInteger(digit))
                digit_type = DigitType.Integer;
            else
                digit_type = DigitType.Unknown;
        }

        public override BaseCalculator.Node.NodeType GetNodeType() { return Node.NodeType.Digit; }

        public bool isInteger(String text)
        {
            try
            {
                return isDigit(text) && Int32.Parse(text).ToString().CompareTo(text) == 0;
            }
            catch (Exception e) { return false; }
        }
        public bool isFloat() { return isFloat(rawText); }
        public bool isFloat(String text)
        {
            return (!isInteger(text)) && text.Contains(".");
        }

        public Double toValue() { return Double.Parse(rawText); }

        public override void Check()
        {
            if (digit_type == DigitType.Unknown)
                throw new Exception("Unknown symbol like " + rawText);
        }

        public int toInteger() { return int.Parse(rawText); }
        public float toFloat() { return float.Parse(rawText); }

        public static bool isDigit(String expression)
        {
            if (expression.Length==0)
                return false;
            foreach (char c in expression)
            {
                if (!(Char.IsDigit(c) || ((c=='.'))))
                    return false;
            }
            return true;
        }
    }
}
