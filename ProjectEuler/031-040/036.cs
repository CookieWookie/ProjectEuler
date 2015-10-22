using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P036()
        {
            long result = Enumerable.Range(1, 999999)
                .Where(t => t % 2 != 0)
                .Select(t => new
                {
                    Decimal = t.ToString(),
                    Binary = Convert.ToString(t, 2),
                    Integer = t
                })
                .Where(t => JePalindrom(t.Decimal) && JePalindrom(t.Binary))
                .Sum(t => t.Integer);
            Console.WriteLine($"P036: {result}");
        }
    }
}