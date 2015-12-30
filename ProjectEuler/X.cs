using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    class X
    {
        public static bool IsPrime(long num)
        {
            if (num == 2 || num == 3)
            {
                return true;
            }
            if (num < 2)
            {
                return false;
            }
            if (num % 2 == 0 || num % 3 == 0)
            {
                return false;
            }
            long i = 5;
            long sqrt = (long)Math.Ceiling(Math.Sqrt(num));
            while (i <= sqrt)
            {
                if (num % i == 0 || num % (i + 2) == 0)
                {
                    return false;
                }
                i += 6;
            }
            return true;
        }

        public static IEnumerable<long> Factorize(long num)
        {
            while (num % 2 == 0)
            {
                yield return 2;
                num = num / 2;
            }
            for (int i = 3; i <= num; i += 2)
            {
                if (IsPrime(i))
                {
                    while (num % i == 0)
                    {
                        yield return i;
                        num = num / i;
                    }
                }
            }
        }
    }
}
