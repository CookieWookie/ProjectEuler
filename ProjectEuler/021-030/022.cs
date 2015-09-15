using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using ProjectEuler.Properties;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P022()
        {
            BigInteger result =
                Resources.p022_names
                .Split(',')
                .OrderBy(t => t, StringComparer.InvariantCultureIgnoreCase)
                .AsParallel()
                .Select(t => t.Select(x => x % 32).Sum())
                .Select(t => new BigInteger(t))
                .Select((t, i) => t * (i + 1))
                .Aggregate((a, b) => a + b);
            Console.WriteLine($"P022: {result}");
        }
    }
}