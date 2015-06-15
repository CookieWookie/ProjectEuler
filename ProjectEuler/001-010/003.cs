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
                    if (d1 > vysledok && JePrvocislo(d1))
                    {
                        vysledok = d1;
                    }
                    // to isté pre deliteľ 2
                    if (d2 > vysledok && JePrvocislo(d2))
                    {
                        vysledok = d2;
                    }
                }
            }
            Console.Write("P003: ");
            Console.WriteLine(vysledok);
        }

        private static bool JePrvocislo(long cislo)
        {
            // na začiatku overíme, či je párne, jednoduchá a rýchla operácia, ktorá nám umožní výrazne zrýchliť chod metódy
            if (cislo % 2 == 0)
            {
                return false;
            }
            // získaj odmocninu čísla
            long sqRoot = (long)Math.Sqrt(cislo);
            // začíname 3, a pretože párnosť sme overili, navyšujeme zakaždým o 2
            // ďalšie masívne zrýchlenie dosiahneme tým, že overujeme hodnoty len do odmocniny testovaného čísla
            // tie vyššie sú už v tom momente otestované 
            for (int i = 3; i <= sqRoot; i += 2)
            {
                // ak je číslo deliteľné aktuálnou hodnotou bezo zvyšku, nie je prvočíslo
                if (cislo % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
