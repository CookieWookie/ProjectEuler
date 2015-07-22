using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P012()
        {
            foreach (long num in GetTriangleNumbers())
            {
                var divisors = GetDivisors(num);
                if (divisors.Count > 500)
                {
                    Console.Write("P012: ");
                    Console.WriteLine(num);
                    return;
                }
            }
        }

        private static IEnumerable<long> GetTriangleNumbers()
        {
            long i = 1;
            long number = 0;
            do
            {
                number += i++;
                yield return number;
            }
            while (i > 0);
        }

        private static ISet<long> GetDivisors(long number)
        {
            HashSet<long> set = new HashSet<long>(new[] { 1L, number });
            if (number < 4)
            {
                return set;
            }
            long root = (long)Math.Sqrt(number);
            for (long i = 2; i < root; i++)
            {
                if (number % i == 0)
                {
                    set.Add(i);
                    set.Add(number / i);
                }
            }
            return set;
        }
    }
}
