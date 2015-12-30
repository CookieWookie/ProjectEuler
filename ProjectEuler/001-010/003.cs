using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P003()
        {
            // nastavíme minimálnu hodnotu ako počiatočnú
            long vysledok = long.MinValue;
            // testované číslo
            long testovaneCislo = 600851475143;
            // odmocnina testované čísla
            long sqRoot = (long)Math.Sqrt(testovaneCislo);
            // začíname 3, pretože vidíme, že číslo je nepárne, preto preskakujeme 2 a všetky ostatné párne čísla
            for (int i = 3; i <= sqRoot; i += 2)
            {
                // "i" je deliteľ test. čísla
                if (testovaneCislo % i == 0)
                {
                    long d1 = i;
                    long d2 = testovaneCislo / i;
                    // ak je deliteľ 1 väčší ako doterajší výsledok a zároveň je prvočíslom, nastav ho ako výsledok
                    if (d1 > vysledok && IsPrime(d1))
                    {
                        vysledok = d1;
                    }
                    // to isté pre deliteľ 2
                    if (d2 > vysledok && IsPrime(d2))
                    {
                        vysledok = d2;
                    }
                }
            }
            Console.Write("P003: ");
            Console.WriteLine(vysledok);
        }

        private static bool IsPrime(long num)
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
    }
}
