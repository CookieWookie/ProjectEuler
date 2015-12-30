using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P001()
        {
            // slúži na uloženie výsledku
            int suma = 0;
            // začíname 3, pretože to je najmenší násobok 3 alebo 5
            for (int i = 3; i < 1000; i++)
            {
                // ak je bezo zvyšku dleiteľné 3 alebo 5, pripočítaj ho k sume
                if (i % 3 == 0 || i % 5 == 0)
                {
                    suma += i;
                }
            }
            Console.Write("P001: ");
            Console.WriteLine(suma);
        }
    }
}
