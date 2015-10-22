using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P035()
        {
            var primesBelow = PrimeNumbers.GetSequence().TakeWhile(t => t < 1000000);
            int result = 0;
            foreach (long number in primesBelow)
            {
                string s = number.ToString();
                if (GetRotations(s).Select(long.Parse).All(IsPrime))
                {
                    result++;
                    Console.WriteLine(number);
                }
            }
            Console.WriteLine($"P035: {result}");
        }

        private static IEnumerable<string> GetRotations(string input)
        {
            string s = input;
            do
            {
                yield return s;
                s = Rotate(s);
            }
            while (s != input);
        }
        private static string Rotate(string input)
        {
            return (input + input.First()).Substring(1);
        }
    }
}