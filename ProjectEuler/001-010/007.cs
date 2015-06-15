using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P007()
        {
            // počet doteraz nájdených prvočísel, začína na 1, pretože si uľahčujeme prácu tým, že už počítame s 2-kou a tak môžeme uvažovať len o
            // nepárnych číslach
            int pocetPrvocisel = 1;
            // číslo, ktoré budeme overovať 
            long cislo = 3;
            // aktuálny výsledok
            long vysledok = 2;
            // kým je počet nájdených prvočísel menší ako 10 001, hľadaj ďalšie
            while (pocetPrvocisel < 10001)
            {
                // ak je prvočíslo, zvýš počítadlo a zapíš do výsledku
                if (JePrvocislo(cislo))
                {
                    vysledok = cislo;
                    pocetPrvocisel++;
                }
                // inkrementuj číslo o 2 (vieme, že prvočísla môžu byť len nepárne, okrem 2)
                cislo += 2;
            }
            Console.Write("P007: ");
            Console.WriteLine(vysledok);
        }
    }
}
