using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P014()
        {
            ConcurrentDictionary<long, long> sequenceLengths = new ConcurrentDictionary<long, long>();
            Parallel.For(500000, 1000000, t => GetSequenceLength(t, sequenceLengths));
            long result = sequenceLengths.OrderByDescending(t => t.Value).FirstOrDefault().Key;
            Console.WriteLine($"P014: {result}");
        }

        private static IEnumerable<T> Series<T>(T seed, Func<T, T> selector, Func<T, bool> canContinue = null)
        {
            if (canContinue == null)
            {
                canContinue = t => true;
            }
            while (canContinue(seed))
            {
                yield return seed;
                seed = selector(seed);
            }
            yield break;
        }
        private static long GetNextInCollatzSequence(long input)
        {
            if (input < 2)
            {
                return 1;
            }
            else if (input % 2 == 0)
            {
                return input / 2;
            }
            else
            {
                return input * 3 + 1;
            }
        }
        private static long GetSequenceLength(long input, ConcurrentDictionary<long, long> sequenceLengths)
        {
            long length;
            if (!sequenceLengths.TryGetValue(input, out length))
            {
                length = 1;
                if (input > 1)
                {
                    length = 1 + GetSequenceLength(GetNextInCollatzSequence(input), sequenceLengths);
                }
                sequenceLengths.AddOrUpdate(input, length, (k, v) => length);
            }
            return length;
        }
    }
}
