using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace sudoku_game
{
    [Serializable]
    public class Grid
    {
        //private Random random = new Random();

        public Number[,] Tiles { get; private set; } = new Number[,]
        {
            { new Number(), new Number(), new Number() },
            { new Number(), new Number(), new Number() },
            { new Number(), new Number(), new Number() }
        };
        public static int TileAmount { get; } = 9;
        public static int TilesAcross { get; } = (int)Math.Sqrt(TileAmount);

        /// <summary>
        /// Grids have to be created by using static 'CreateGrids' method
        /// </summary>
        public Grid() 
        {
            //DEBUG LOG
            Console.WriteLine("Instantiating Grid Class");
        }
    }
}
