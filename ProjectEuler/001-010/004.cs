using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P004()
        {
            // ukladáme max hodnotu
            int vysledok = int.MinValue;
            // 2 vnorené cykly, musíme zabezpečiť, aby sa násobili všetky čísla navzájom
            for (int i = 999; i > 99; i--)
            {
                for (int j = 999; j > 99; j--)
                {
                    // vynásobíme, ak je väčšie číslo ako doterajšia hodnota a je palindrom, uložíme
                    int x = i * j;
                    if (x > vysledok && JePalindrom(x.ToString()))
                    {
                        vysledok = x;
                    }
                }
            }
            // voila, máme výsledok :)
            Console.Write("P004: ");
            Console.WriteLine(vysledok);
        }

        // vstup musí byť string, aby sme dokázali porovnávať jednotlivé znaky
        private static bool JePalindrom(string text)
        {
            if (text.Length == 1)
            {
                return true;
            }
            // nájdeme si stred reťazca, pre istotu zvýšime o 1 (ak by bola dĺžka nepárna, desatinné miesta sa vždy odseknú preč [9,54 => 9])
            int polovica = (text.Length / 2);
            for (int i = 0; i <= polovica; i++)
            {
                // prvý znak
                char a = text[i];
                // jeho protejšok, na rovnakej pozícii od konca
                char b = text[text.Length - i - 1];
                // ak sa nerovnajú, nie je palindrom
                if (a != b)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
