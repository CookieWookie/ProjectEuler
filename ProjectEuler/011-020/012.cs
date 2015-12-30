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
            long result = GetTriangleNumbers()
                .Select(t => new { Number = t, DivisorCount = GetDivisors(t).Count })
                .Where(t => t.DivisorCount > 500)
                .Select(t => t.Number)
                .FirstOrDefault();
            Console.WriteLine($"P012: {result}");
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

        private static ISet<long> GetProperDivisors(long number)
        {
            HashSet<long> set = new HashSet<long>(new[] { 1L });
            long root = (long)Math.Sqrt(number);
            for (long i = 2; i <= root; i++)
            {
                if (number % i == 0)
                {
                    set.Add(i);
                    set.Add(number / i);
                }
            }
            return set;
        }
        private static ISet<long> GetDivisors(long number)
        {
            ISet<long> set = GetProperDivisors(number);
            set.Add(number);
            return set;
        }
    }
}
