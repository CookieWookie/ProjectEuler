using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using System.Threading;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P034()
        {
            long max = 9;
            int digits = 1;
            long factorial9 = (long)FactorialMoessner(9);
            while (max <= digits * factorial9)
            {
                max = max * 10 + 9;
                digits++;
            }
            max = digits * factorial9;
            long result = 0;
            Parallel.For(10, max, i =>
            {
                if (GetDigits(i).Select(FactorialMoessner).Aggregate((a, b) => a + b) == i)
                {
                    Interlocked.Add(ref result, i);
                }
            });
            Console.WriteLine($"P034: {result}");
        }

        private static IEnumerable<int> GetDigits(BigInteger i)
        {
            return i.ToString().Select(char.GetNumericValue).Select(t => (int)t);
        }
    }
}