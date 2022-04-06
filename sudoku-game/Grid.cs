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
        /// <summary>
        /// Checks if a number repeats horizontally or vertically <b>when 9x9 grid is complete!</b>
        /// </summary>
        /// <param name="list">A list of all previous numbers with their tile pos and grid num</param>
        /// <param name="gridCount">In what grid the current number is</param>
        /// <param name="tilePos">Position of current number</param>
        /// <param name="tileNum">A number</param>
        /// <returns></returns>

        private bool RepeatsInGrids(ref List<string> list, int gridCount, string tilePos, int tileNum)
        {
            int tileRow = int.Parse(new Regex("\\d+?(?=,)").Match(tilePos).Value);
            int tileCol = int.Parse(new Regex("(?<=,)\\d+").Match(tilePos).Value);

            string gridToCheckHoriz = null;
            string gridToCheckVert = null;
            switch (gridCount)
            {
                case 1:
                case 2:
                case 3:
                    Console.WriteLine("Check number in top row");
                    gridToCheckVert = $"[{gridCount + 3}{gridCount + 6}]";
                    break;
                case 4:
                case 5:
                case 6:
                    Console.WriteLine("Check number in middle row");
                    gridToCheckVert = $"[{gridCount - 3}{gridCount + 3}]";
                    break;
                case 7:
                case 8:
                case 9:
                    Console.WriteLine("Check number in bottom row");
                    gridToCheckVert = $"[{gridCount - 3}{gridCount - 6}]";
                    break;
            }
            switch (gridCount)
            {
                case 1:
                case 4:
                case 7:
                    gridToCheckHoriz = $"[{gridCount + 1}{gridCount + 2}]";
                    break;
                case 2:
                case 5:
                case 8:
                    gridToCheckHoriz = $"[{gridCount - 1}{gridCount + 1}]";
                    break;
                case 3:
                case 6:
                case 9:
                    gridToCheckHoriz = $"[{gridCount - 1}{gridCount - 2}]";
                    break;
            }
            Regex anyNum = new Regex("\\d");
            Regex horizCheck = new Regex($"{tileNum}:\\d,{tileCol}\\|{gridToCheckVert}");
            Regex vertCheck = new Regex($"{tileNum}:{tileRow},\\d\\|{gridToCheckHoriz}");

            bool returnResult = false;
            Array.ForEach(list.ToArray(), (x) =>
            {
                if (horizCheck.IsMatch(x) || vertCheck.IsMatch(x))
                {
                    Console.WriteLine("Match!");
                    returnResult = true;
                }
            });
            return returnResult;
        }

    }
}
