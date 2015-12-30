using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Collections;
using System.Diagnostics;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P024()
        {
            Console.WriteLine($"P024: {Permutation.GetNth(Enumerable.Range(0, 10), 1000000)}");
        }

        private static class Permutation
        {
            public static IEnumerable<Permutation<T>> GetAll<T>(IEnumerable<T> data)
            {
                return GetAll(data.ToArray());
            }
            public static IEnumerable<Permutation<T>> GetAll<T>(T[] data)
            {
                for (int i = 0; i < FactorialMoessner(data.Length); i++)
                {
                    yield return GetNth(data, i + 1);
                }
            }
            public static Permutation<T> GetNth<T>(IEnumerable<T> data, int rank)
            {
                return GetNth(data.ToArray(), rank);
            }
            public static Permutation<T> GetNth<T>(T[] data, int rank)
            {
                int N = data.Length;
                int remain = rank - 1;
                List<T> result = new List<T>();
                List<T> items = new List<T>(data);
                int factorial = (int)FactorialMoessner(N - 1);

                for (int i = 1; i < N; i++)
                {
                    int j = Math.DivRem(remain, factorial, out remain);
                    factorial /= N - i;
                    result.Add(items[j]);
                    items.RemoveAt(j);
                    if (remain == 0)
                    {
                        break;
                    }
                }

                for (int i = 0; i < items.Count; i++)
                {
                    result.Add(items[i]);
                }

                return new Permutation<T>(result.ToArray());
            }
        }
        [DebuggerDisplay("Count={Length}")]
        private class Permutation<T> : IEnumerable<T>
        {
            public Permutation(T[] data)
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }
                this.Data = new T[data.Length];
                data.CopyTo(this.Data, 0);
            }

            private T[] Data
            {
                get;
            }
            public int Length
            {
                get
                {
                    return this.Data.Length;
                }
            }
            public T this[int index]
            {
                get
                {
                    if (index < 0 || index > this.Length)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }
                    return this.Data[index];
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return ((IEnumerable<T>)this.Data).GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<T>)this.Data).GetEnumerator();
            }

            public override string ToString()
            {
                return $"[{string.Join(";", this.Data)}]";
            }
        }
    }
}