using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P025()
        {
            int result = Fibonacci.GetSequence().Select((t, i) => new { t, i }).Where(t => t.t.ToString().Length == 1000).Select(t => t.i).FirstOrDefault() + 1;
            Console.WriteLine($"P025: {result}");
        }

        private static class Fibonacci
        {
            public static IEnumerable<BigInteger> GetSequence()
            {
                BigInteger a = BigInteger.One;
                BigInteger b = BigInteger.One;
                do
                {
                    yield return a;
                    BigInteger temp = a + b;
                    a = b;
                    b = temp;
                }
                while (true);
            }
        }
    }
}