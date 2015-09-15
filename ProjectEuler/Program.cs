using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProjectEuler
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the number of problem: ");
            string problem = $"P{Console.ReadLine().Trim().TrimStart('0').PadLeft(3, '0')}";
            typeof(Program)
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                .FirstOrDefault(t => t.Name == problem)
                .Invoke(null, null);
            Console.ReadLine();
        }
    }
}
