using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P016()
        {
            long result = BigInteger.Pow(2, 1000)
                .ToString()
                .Select(t => t.ToString())
                .Select(long.Parse)
                .Sum();
            Console.WriteLine($"P016: {result}");
        }
    }
}
