using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P021()
        {
            long sum = 0;
            HashSet<long> visited = new HashSet<long>();
            const long upperLimit = 10000;
            for (long a = 2; a < upperLimit; a++)
            {
                if (visited.Add(a))
                {
                    long b = SumOfProperDivisors(a);
                    visited.Add(b);
                    if (a != b && SumOfProperDivisors(b) == a)
                    {
                        sum += a + b;
                    }
                }
            }
            Console.WriteLine($"P021: {sum}");
        }

        private static long SumOfProperDivisors(long number)
        {
            return GetProperDivisors(number).Sum();
        }
    }
}