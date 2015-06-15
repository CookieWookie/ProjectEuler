using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P009()
        {
            // cyklus pre c, maximálna hodnota 500 
            // ani neviem presne prečo, ale myslím, že to je kvôli mocninám, inak by sa nikdy nemohli rovnať :) 
            // alebo som to len tak strelil kedysi a vyšlo to :D
            for (int c = 500; c > 0; c--)
            {
                // cyklus pre b, musí byť menšie ako c, preto je počiatočná hodnota c - 1
                for (int b = c - 1; b > 0; b--)
                {
                    // rovnaké ako b, akurát miesto c je b :D snáď dáva zmysel :)
                    for (int a = b - 1; a > 0; a--)
                    {
                        // ak nie je súčet 1000, ignoruj
                        if (a + b + c != 1000)
                        {
                            continue;
                        }
                        // ľavá strana pytagorovej vety
                        int lavaStrana = a * a + b * b;
                        // pravá strana
                        int pravaStrata = c * c;
                        // ak sa nerovnajú, neplatné a ignoruj
                        if (lavaStrana != pravaStrata)
                        {
                            continue;
                        }
                        // po vynásobení našich premenných máme výsledok :)
                        Console.Write("P009: ");
                        Console.WriteLine(a * b * c);
                        return;
                    }
                }
            }
        }
    }
}
