using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;
namespace sudoku_game
{
    class Program
    {
        static async Task Main(string[] args)
        { 
            Console.WriteLine("Does it even start?");
            Console.WriteLine("Numbers are not checked by uniqueness across grids, " +
                "only checked in its own grid, change that!");

            Stopwatch overallTime = new Stopwatch();
            Stopwatch individualTime = new Stopwatch();
            overallTime.Start();
            individualTime.Start();

            Map map = new Map();
            Console.WriteLine($"Created a map with {individualTime.ElapsedMilliseconds} milliseconds");
            individualTime.Reset();
            individualTime.Start();

            map.PrintMap();
            Console.WriteLine($"Printed a map with {individualTime.ElapsedMilliseconds} milliseconds");
            Console.WriteLine($"Overything took: {overallTime.ElapsedMilliseconds} milliseconds");
            Console.Read();
        }
    }
}

