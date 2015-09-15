using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P032()
        {
            const int max = 9876;
            HashSet<long> results = new HashSet<long>();
            for (long a = 2; a < max; a++)
            {
                for (long b = 2; b < a && b < max / a; b++)
                {
                    long c = a * b;
                    string text = $"{a}{b}{c}";
                    if (text.Length == 9 && IsPandigital(long.Parse(text)))
                    {
                        results.Add(c);
                    }
                }
            }
            Console.WriteLine($"P032: {results.Sum()}");
        }

        private static bool IsPandigital(long n)
        {
            return n.ToString()
                .OrderBy(t => t)
                .Select(t => t.ToString())
                .Select(int.Parse)
                .Select((t, i) => t == (i + 1))
                .All(t => t);
        }
    }
}