using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P037()
        {
            long result =
                PrimeNumbers.GetSequence()
                .AsParallel()
                .Where(t => t > 10)
                .Where(t => GetTruncates(TruncateRight, t).All(IsPrime))
                .Where(t => GetTruncates(TruncateLeft, t).All(IsPrime))
                .Take(11)
                .Sum();
            Console.WriteLine($"P037: {result}");
        }

        private static IEnumerable<long> GetTruncates(Func<long, long> truncate, long num)
        {
            do
            {
                yield return num;
                num = truncate(num);
            }
            while (num > 0);
        }
        private static long TruncateRight(long num)
        {
            return num / 10;
        }
        private static long TruncateLeft(long num)
        {
            if (num < 0)
            {
                return -TruncateLeft(-num);
            }
            if (num < 10)
            {
                return 0;
            }
            return long.Parse(num.ToString().Substring(1));
        }
    }
}