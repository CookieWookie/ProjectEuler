using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P030()
        {
            long max = 9;
            int numbers = 1;
            while (max < Math.Pow(9, 5) * numbers)
            {
                max = max * 10 + 9;
                numbers++;
            }
            max = (long)Math.Pow(9, 5) * numbers;
            long result = EnumerableEx.Generate(10L, t => t + 1)
                .TakeWhile(t => t <= max)
                .AsParallel()
                .Where(t =>
                    t ==
                    t.ToString()
                     .Select(x => x.ToString())
                     .Select(x => long.Parse(x))
                     .Select(x => Math.Pow(x, 5))
                     .Sum())
                .Sum();
            Console.WriteLine($"P030: {result}");
        }
    }
}