using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P031()
        {
            Console.WriteLine($"P031: {CountCoinCombinations()}");
        }

        private static int CountCoinCombinations()
        {
            int count = 0;
            const int maxValue = 200;
            for (int a = 0; a <= maxValue; a += 200)
            {
                for (int b = a; b <= maxValue; b += 100)
                {
                    for (int c = b; c <= maxValue; c += 50)
                    {
                        for (int d = c; d <= maxValue; d += 20)
                        {
                            for (int e = d; e <= maxValue; e += 10)
                            {
                                for (int f = e; f <= maxValue; f += 5)
                                {
                                    for (int g = f; g <= maxValue; g += 2)
                                    {
                                        for (int h = g; h <= maxValue; h++)
                                        {
                                            if (h == maxValue)
                                            {
                                                count++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return count;
        }
    }
}