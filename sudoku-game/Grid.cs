using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace sudoku_game
{
    public class Grid
    {
        private Random random = new Random();

        public int?[,] Tiles { get; set; }

        internal static readonly int _tileAmount = 9;
        public static int TilesAcross { get; } = (int)Math.Sqrt(_tileAmount);

        /// <summary>
        /// Grids have to be created by using static 'CreateGrids' method
        /// </summary>
        public Grid() 
        {
            //DEBUG LOG
            Console.WriteLine("Instantiating Grid Class");
            Tiles = new int?[TilesAcross, TilesAcross];
        }
    }
}
