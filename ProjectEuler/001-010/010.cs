using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P010()
        {
            // výsledok začína na 2, zakaždým pri prvočíslach automaticky uvažuješ s 2-kou špeciálne, pretože je jediné párne prvočíslo :)
            // vyradenie na začiatku ti umožní ďalej pracovať už len s nepárnymi číslami, čo ti zníži výpočtovú náročnosť na 50% :)
            long vysledok = 2;
            // všetky nepárne čísla od 3 do 2 000 000
            for (long i = 3; i < 2000000; i += 2)
            {
                // ak je prvočíslo, započítaj do výsledku :)
                if (JePrvocislo(i))
                {
                    vysledok += i;
                }
            }
            Console.Write("P010: ");
            Console.WriteLine(vysledok);
        }
    }
}
