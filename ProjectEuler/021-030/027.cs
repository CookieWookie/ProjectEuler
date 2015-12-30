using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P027()
        {
            HashSet<long> primes = new HashSet<long>();
            for (int i = 1; i < 1000; i++)
            {
                primes.Add(i);
                primes.Add(-i);
            }
            long result = (
                from b in primes
                from c in primes
                let count = CountConsecutivePrimes(b, c)
                orderby count descending
                select b * c).FirstOrDefault();
            Console.WriteLine($"P026: {result}");
        }

        private static int CountConsecutivePrimes(long b, long c)
        {
            int count = 0;
            long n = 0;
            while (IsPrime(n * n + b * n + c))
            {
                count++;
                n++;
            }
            return count;
        }

        private static class PrimeNumbers
        {
            public static IEnumerable<long> GetSequence()
            {
                yield return 2;
                long n = 3;
                while (true)
                {
                    if (IsPrime(n))
                    {
                        yield return n;
                    }
                    n += 2;
                }
            }
        }
    }
}