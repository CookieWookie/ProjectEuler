using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        // myslím, že toto je self-explanatory, tak to nebudem rozpisovať :D
        private static void P006()
        {
            long sucetMocnin = 0;
            long mocninaSuctov = 0;
            for (long i = 1; i <= 100; i++)
            {
                sucetMocnin += (i * i);
                mocninaSuctov += i;
            }
            mocninaSuctov *= mocninaSuctov;
            Console.Write("P006: ");
            Console.WriteLine(mocninaSuctov - sucetMocnin);
        }
    }
}
