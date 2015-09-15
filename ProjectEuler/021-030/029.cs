using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P029()
        {
            const int limit = 100;
            HashSet<BigInteger> set = new HashSet<BigInteger>();
            for (BigInteger a = 2; a <= limit; a++)
            {
                for (int b = 2; b <= limit; b++)
                {
                    set.Add(BigInteger.Pow(a, b));
                }
            }
            Console.WriteLine($"P029: {set.Count}");
        }
    }
}