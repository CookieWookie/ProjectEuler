using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            MethodInfo method = typeof(Program)
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                .FirstOrDefault(t => t.Name == problem);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            method.Invoke(null, null);
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed}");
            Console.ReadLine();
        }
    }
}
