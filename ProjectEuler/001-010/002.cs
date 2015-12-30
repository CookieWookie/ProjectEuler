using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        private static void P002()
        {
            // na uloženie výsledku
            int suma = 0;
            // vstupné hodnoty fibonacciho postupnosti
            int a = 1;
            int b = 2;
            // kým sú hodnoty menšie alebo rovné 4 miliónom
            while (a <= 4000000)
            {
                // ak je hodnota párna, pripočítaj k výsledku
                if (a % 2 == 0)
                {
                    suma += a;
                }
                // posuň sa v postupnosti ďalej
                int temp = a;
                a = b;
                b = temp + b;
            }
            Console.Write("P002: ");
            Console.WriteLine(suma);
        }
    }
}
