using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace sudoku_game
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid testGrid = new Grid();

            int count = 1;
            for (int i = 0; i < testGrid.Tiles.Length; i++)
            {
                if (count++ % 3 == 0)
                {
                    Console.WriteLine(testGrid.Tiles[i]);
                } else
                {
                    Console.Write(testGrid.Tiles[i]);
                }
                
            }
            Console.WriteLine($"The length is {testGrid.Tiles.Length}");
        }
    }
}
