using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace sudoku_game
{
    /// <summary>
    /// Represents a Sudoku game map with nested grid class
    /// </summary>
    public class Map
    {
        public Grid[,] Grids { get; }
        public static int _singleTonCount = 0;

        public Map()
        {
            ///<summary>
            /// Implementing SingleTon
            ///</summary>
            if (++_singleTonCount > 1)
                throw new Exception("You can't have more " +
                                    "than one instance of Map Class");
            int gridAmount = 9;

            if ((int)Math.Sqrt(gridAmount) != Math.Sqrt(gridAmount))
                throw new Exception("A map cannot contain");

            Grids = Grid.CreateGrids();
        }
        public void PrintMap()
        {
            Console.Clear();
            for (int gridRow = 0; gridRow < Grids.Length; gridRow++)
            {
                for (int gridCol = 0; gridCol < Grids[gridCol, gridCol].Tiles.Length; gridCol++)
                {
                    for (int row = 0; row < Grids[gridRow, gridCol].Tiles.Length; row++) // tile row
                    {
                        for (int col = 0; col < Grids[gridRow, gridCol].Tiles.Length; col++) // tile col
                        {
                            int? tile = Grids[gridRow, gridCol].Tiles[row, col];
                            Console.WriteLine(tile is null ? ' ' : 3);
                            // jumpnig on the new row
                            if (col + 1 == Grids[gridRow, gridCol].Tiles.Length)
                            {
                                Console.WriteLine('|');
                            }
                        }
                    }
                }
            }
        }
        public class Grid
        {
            private static Random random = new Random();

            private static Grid[,] grids;
            private static readonly int _gridAmount = 9;
            private static readonly int _gridsAcross = (int)Math.Sqrt(_gridAmount);

            private static readonly int _tileAmount = 9;
            private static readonly int _tilesAcross = (int)Math.Sqrt(_tileAmount);
            private int?[,] _tiles;
            public int?[,] Tiles
            {
                get { return _tiles; }
            }

            private Grid()
            {
                _tiles = new int?[_tilesAcross,_tilesAcross];
            }
            /// <summary>
            /// The only way to create grids outside of its class
            /// </summary>
            /// <returns> An array of grid objects with non-repeating tile numbers: horizontally and vertically (b</returns>
            internal static Grid[,] CreateGrids()
            {
                grids = new Grid[_gridsAcross, _gridsAcross];
                for (int row = 0; row < _gridsAcross; row++)
                {
                    for (int col = 0; col < _gridsAcross; col++)
                    {
                        grids[row, col] = new Grid(); 
                    }
                }
                ///<summary>
                /// add numbers to regex with this method to ignore them
                /// while assigning numbers to tiles
                ///</summary>
                void AddToEndRegex<Nullable>(ref Regex regex, Nullable adding) 
                {
                    regex = new Regex(
                                    regex.ToString().Substring(0, regex.ToString().Length - 1)
                                    + adding + ']');
                }

                foreach (Grid grid in grids)
                {
                    
                }
                ///<summary>
                ///// In each grid, fills up few random tiles with random numbers
                ///// </summary>
                //for (int gridRow = 0; gridRow < _gridsAcross; gridRow++) //loops through grid rows
                //{
                //    for (int gridCol = 0; gridCol < _tilesAcross; gridCol++) //loops through grid rows
                //    {
                //        Regex numberExceptions = new Regex("[.]");
                //        bool repeatGrid = false; // if a number can't be in a tile, repeat the loop above
                //        for (int s = 0; s < random.Next(5); s++) //loops how many numbers to put
                //        {
                //            int row = random.Next(1, _tilesAcross);
                //            int col = random.Next(1, _tilesAcross);
                //            for (int rowCheck = 0; rowCheck < _tilesAcross; rowCheck++) // checking if row is occupied
                //            {
                //                for (int colCheck = 0; colCheck < _tilesAcross; colCheck++) // checking if col is occupied
                //                {
                //                    try
                //                    {
                //                        int? currentTile = grids[gridRow, gridCol]._tiles[row, col];
                //                        int? toCheckTile = grids[gridRow, gridCol]._tiles[rowCheck, colCheck];

                //                        // and tiles shouldn't be equal horizontally and vertically!
                //                        if (currentTile == toCheckTile) 
                //                        {
                //                            gridRow--;
                //                            repeatGrid = true;
                //                            break;
                //                        }
                //                    }
                //                    catch (IndexOutOfRangeException ex) // still trying to understand why I have try-catch… 
                //                    {
                //                        Console.WriteLine(ex.ToString());
                //                        continue;
                //                    }
                //                }
                //                if (repeatGrid)
                //                { break; }
                //            }
                //            if (repeatGrid)
                //            { break; }
                //            // Assigning a number to a tile
                //            int? numberToPut;
                //            do
                //            {
                //                numberToPut = random.Next(1, _tileAmount + 1);
                //            } while (numberExceptions.IsMatch(numberToPut.ToString()));
                //            AddToEndRegex(ref numberExceptions, numberToPut);
                //            Console.WriteLine(numberExceptions);
                //            grids[gridRow, gridCol]._tiles[row, col] = numberToPut;
                //        }
                //    }
                //}
                return grids;
            }
        }
    }
}
// 1 2 3
// 4 5 6   row check => x -+ 1
// 7 8 9   col check => x -+ 3