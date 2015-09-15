using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P018()
        {
            long[][] numbers = {
                new[] { 75L },
                new[] { 95L, 64 },
                new[] { 17L, 47, 82 },
                new[] { 18L, 35, 87, 10 },
                new[] { 20L, 04, 82, 47, 65 },
                new[] { 19L, 01, 23, 75, 03, 34 },
                new[] { 88L, 02, 77, 73, 07, 63, 67 },
                new[] { 99L, 65, 04, 28, 06, 16, 70, 92 },
                new[] { 41L, 41, 26, 56, 83, 40, 80, 70, 33 },
                new[] { 41L, 48, 72, 33, 47, 32, 37, 16, 94, 29 },
                new[] { 53L, 71, 44, 65, 25, 43, 91, 52, 97, 51, 14 },
                new[] { 70L, 11, 33, 28, 77, 73, 17, 78, 39, 68, 17, 57 },
                new[] { 91L, 71, 52, 38, 17, 14, 91, 43, 58, 50, 27, 29, 48 },
                new[] { 63L, 66, 04, 68, 89, 53, 67, 30, 73, 16, 69, 87, 40, 31 },
                new[] { 04L, 62, 98, 27, 23, 09, 70, 98, 73, 93, 38, 53, 60, 04, 23 }
            };
            TrianglePathFinder tpf = new TrianglePathFinder(numbers);
            Console.WriteLine($"P018: {tpf.GetLength()}");
        }

        private class TrianglePathFinder
        {
            private readonly long[] input;
            private readonly int size;

            public TrianglePathFinder(long[][] input)
            {
                this.size = input.Length;
                this.input = new long[this.size * this.size];
                for (int i = 0; i < input.Length; i++)
                {
                    long[] array = input[i];
                    array.CopyTo(this.input, i * this.size);
                }
            }

            public long GetLength()
            {
                for (int i = this.size - 1; i > 0; i--)
                {
                    for (int j = 0; j < this.size - 1; j++)
                    {
                        long left = this.input[i * this.size + j];
                        long right = this.input[i * this.size + j + 1];
                        this.input[(i - 1) * this.size + j] += left > right ? left : right;
                    }
                }
                return this.input[0];
            }
        }
    }
}
