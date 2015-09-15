using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P028()
        {
            const int size = 1001;
            long number = (long)size * size;
            long result = 0;
            Corner currentCorner = Corner.TopRight;
            int finishedSquare = 0;
            while (true)
            {
                result += number;
                if (number <= 1)
                {
                    break;
                }
                number -= (size - 1 - finishedSquare);

                if (currentCorner == Corner.TopRight)
                {
                    currentCorner = Corner.TopLeft;
                }
                else if (currentCorner == Corner.TopLeft)
                {
                    currentCorner = Corner.BottomLeft;
                }
                else if (currentCorner == Corner.BottomLeft)
                {
                    currentCorner = Corner.BottomRight;
                }
                else
                {
                    currentCorner = Corner.TopRight;
                    finishedSquare += 2;
                }
            }
            Console.WriteLine($"P028: {result}");
            P028_Alt();
        }

        private static void P028_Alt()
        {
            const int max = 1001;
            // Top Right:       n^2 - 0n + 0
            // Top Left:        n^2 - 1n + 1
            // Bottom Left:     n^2 - 2n + 2
            // Bottom Right:    n^2 - 3n + 3
            // Top Right corners are 2nd powers of odd numbers
            long result = 1 + EnumerableEx.Generate(3, t => t + 2).TakeWhile(t => t <= max).AsParallel().Select(t => (long)t).Select(t => 4 * t * t - 6 * t + 6).Sum();
            Console.WriteLine($"P028: {result}");
        }

        private enum Corner
        {
            TopRight,
            TopLeft,
            BottomLeft,
            BottomRight
        }
    }

    public static class EnumerableEx
    {
        public static IEnumerable<T> Generate<T>(T seed, Func<T, T> generator)
        {
            do
            {
                yield return seed;
                seed = generator(seed);
            }
            while (true);
        }
    }
}