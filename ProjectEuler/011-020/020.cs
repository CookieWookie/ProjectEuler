using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P020()
        {
            long result = FactorialMoessner(100)
                .ToString()
                .Select(t => t.ToString())
                .Select(long.Parse)
                .Sum();
            Console.WriteLine($"P020: {result}");
        }

        private static BigInteger FactorialMoessner(int number)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }
            BigInteger[] array = new BigInteger[number + 1];
            array[0] = BigInteger.One;
            for (int m = 1; m <= number; m++)
            {
                array[m] = BigInteger.Zero;
                for (int k = m; k >= 1; k--)
                {
                    for (int i = 0; i < k; i++)
                    {
                        array[i + 1] += array[i];
                    }
                }
            }
            return array[number];
        }
    }
}
