using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;
namespace sudoku_game
{
    [Serializable]
    class Program
    {
        static bool repeat = true;
        static void Main(string[] args)
        {
            Map map = new Map();
            Player player = new Player(map);

            while (!map.GameOver())
            {
                player.MakeMove();
            }
            Console.WriteLine("Game is over!");
        }
    }
}

