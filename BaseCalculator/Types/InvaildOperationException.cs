using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCalculator
{
    public class InvaildOperationException : Exception
    {
        public InvaildOperationException(int pos, String msg) : base(String.Format("In position {0} , {1} ", pos, msg)){ }
    }
}
