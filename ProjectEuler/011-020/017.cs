using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P017()
        {
            long result = Enumerable.Range(1, 1000)
                .AsParallel()
                .Select(t => (long)t)
                .Select(NumberWordGenerator.Convert)
                .SelectMany(t => t)
                .Where(char.IsLetter)
                .LongCount();
            Console.WriteLine($"P017: {result}");
        }

        static class NumberWordGenerator
        {
            private const long Million = Thousand * Thousand;
            private const long Thousand = 1000;
            private const long Hundred = 100;

            private const string MillionName = "million";
            private const string ThousandName = "thousand";
            private const string HundredName = "hundred";

            private static ConcurrentDictionary<long, string> Numbers
            {
                get;
            }

            static NumberWordGenerator()
            {
                Numbers = new ConcurrentDictionary<long, string>
                {
                    [0] = "zero",
                    [1] = "one",
                    [2] = "two",
                    [3] = "three",
                    [4] = "four",
                    [5] = "five",
                    [6] = "six",
                    [7] = "seven",
                    [8] = "eight",
                    [9] = "nine",
                    [10] = "ten",
                    [11] = "eleven",
                    [12] = "twelve",
                    [13] = "thirteen",
                    [14] = "fourteen",
                    [15] = "fifteen",
                    [16] = "sixteen",
                    [17] = "seventeen",
                    [18] = "eighteen",
                    [19] = "nineteen",
                    [20] = "twenty",
                    [30] = "thirty",
                    [40] = "forty",
                    [50] = "fifty",
                    [60] = "sixty",
                    [70] = "seventy",
                    [80] = "eighty",
                    [90] = "ninety"
                };
            }

            public static string Convert(long number)
            {
                if (number < 0)
                {
                    return $"minus {Convert(-number)}";
                }
                string result;
                if (!Numbers.TryGetValue(number, out result))
                {
                    result = ConvertInternal(number);
                    Numbers.TryAdd(number, result);
                }
                return result;
            }
            private static string ConvertInternal(long number)
            {
                if (number < Hundred)
                {
                    long remainder;
                    long quotient = Math.DivRem(number, 10, out remainder);
                    return remainder == 0 ? $"{Convert(quotient * 10)}" : $"{Convert(quotient * 10)}-{Convert(remainder)}";
                }
                else
                {
                    long divisor;
                    string divisorName;
                    if (number >= Million)
                    {
                        divisor = Million;
                        divisorName = MillionName;
                    }
                    else if (number >= Thousand)
                    {
                        divisor = Thousand;
                        divisorName = ThousandName;
                    }
                    else
                    {
                        divisor = Hundred;
                        divisorName = HundredName;
                    }
                    long remainder;
                    long quotient = Math.DivRem(number, divisor, out remainder);
                    string result = $"{Convert(quotient)} {divisorName}";
                    if (remainder != 0)
                    {
                        string and = divisor > Hundred ? string.Empty : " and";
                        result = $"{result}{and} {Convert(remainder)}";
                    }
                    return result;
                }
            }
        }
    }
}
