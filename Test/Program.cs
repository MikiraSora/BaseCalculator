using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCalculator;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();
            while (true)
            {
                Console.Write("input :");
                string input = Console.ReadLine();
                try
                {
                    Console.WriteLine("result :>{0}",calculator.Solve(input));
                }
                catch (Exception e) {
                    Console.WriteLine("error:{0}", e.Message);
                }
            }
        }
    }
}
