using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace sudoku_game
{
    public class Grid
    {
        private Random random = new Random();

        private int?[,] _tiles;
        public int?[,] Tiles
        {
            get { return _tiles; }
        }

        /// <summary>
        /// Grids have to be created by using static 'CreateGrids' method
        /// </summary>
        public Grid() 
        {
            //DEBUG LOG
            Console.WriteLine("Instantiating Grid Class");
            _tiles = new int?[Map._tilesAcross, Map._tilesAcross];
        }
    }
}
