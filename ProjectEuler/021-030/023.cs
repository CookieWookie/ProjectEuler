using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P023()
        {
            const int upperLimit = 28123;
            // divisor sum is greater than the number
            HashSet<long> abundantNumbers = new HashSet<long>();
            for (long t = 12; t < upperLimit; t++)
            {
                if (SumOfProperDivisors(t) > t)
                {
                    abundantNumbers.Add(t);
                }
            }
            long result = 0;
            for (int i = 1; i < upperLimit; i++)
            {
                bool x = true;
                foreach (long n1 in abundantNumbers)
                {
                    if (n1 >= i)
                    {
                        break;
                    }
                    if (abundantNumbers.Contains(i - n1))
                    {
                        x = false;
                        break;
                    }
                }
                if (x)
                {
                    result += i;
                }
            }
            Console.WriteLine($"P023: {result}");
        }
    }
}