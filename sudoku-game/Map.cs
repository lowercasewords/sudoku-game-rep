using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
            //Console.Clear();
            //for (int gridRow = 0; gridRow < Grids.GetLength(0); gridRow++)
            //{
            //    Console.WriteLine("------------");
            //    for (int gridCol = 0; gridCol < Grids.GetLength(1); gridCol++)
            //    {
            //        Console.Write($"[{gridRow * 3 + gridCol+1}]");
            //        int rowLength = Grids[gridRow, gridCol].Tiles.GetLength(0);
            //        int colLength = Grids[gridRow, gridCol].Tiles.GetLength(1);

            //        // printing tiles in the current grid
            //        for (int row = 0; row < rowLength; row++) // tile row
            //        {
            //            for (int col = 0; col < colLength; col++) // tile col
            //            {
            //                int? tile = Grids[gridRow, gridCol].Tiles[row, col];
            //                Console.Write(tile is null ? "-" : tile.ToString());
            //                var something = tile.ToString();
            //            }
            //            Console.Write(row + 1 == rowLength ? "|\n" : "|");
            //            //Console.WriteLine('|');
            //        }
            //    }
            //}

            //for (int gridRow = 0; gridRow < Grids.GetLength(0); gridRow++)
            //{
            //    int counter = 0;
            //    while (counter++ < Grid._gridsAcross)
            //    {
            //        for (int gridCol = 0; gridCol < Grids.GetLength(1); gridCol++)
            //        {
            //            Console.Write($"[{gridRow * 3 + gridCol + 1}]");
            //            for (int row = 0; row < Grid._tilesAcross; row++) // tile row
            //            {
            //                for (int col = 0; col < Grid._tilesAcross; col++)
            //                {
            //                    int? tile = Grids[gridRow, gridCol].Tiles[row, col];
            //                    Console.Write(tile is null ? "-" : tile.ToString());
            //                }
            //                Console.Write(counter + 1 == Grid._tilesAcross ? "|\n" : "|");
            //            }
            //        }
            //    }
            //}
            for (int gridRow = 0; gridRow < Grids.GetLength(0); gridRow++)
            {

            }
        public class Grid
        {
            private static Random random = new Random();

            private static Grid[,] grids;
            internal static readonly int _gridAmount = 9;
            internal static readonly int _gridsAcross = (int)Math.Sqrt(_gridAmount);

            internal static readonly int _tileAmount = 9;
            internal static readonly int _tilesAcross = (int)Math.Sqrt(_tileAmount);
            private int?[,] _tiles;
            public int?[,] Tiles
            {
                get { return _tiles; }
            }

            private Grid()
            {
                Console.WriteLine("Instantiating Grid Class");
                _tiles = new int?[_tilesAcross,_tilesAcross];
            }
            /// <summary>
            /// The only way to create grids outside of its class
            /// </summary>
            /// <returns> An array of grid objects with non-repeating tile numbers: horizontally and vertically (b</returns>
            internal static Grid[,] CreateGrids()
            {
                Console.WriteLine("Creating Grid...");
                grids = new Grid[_gridsAcross, _gridsAcross];
                for (int row = 0; row < _gridsAcross; row++)
                {
                    for (int col = 0; col < _gridsAcross; col++)
                    {
                        grids[row, col] = new Grid(); 
                    }
                }
                int count = 0;
                Console.WriteLine("Start filling the Grids");
                foreach (Grid grid in grids)
                {
                    Console.WriteLine($"\nCreating {++count}th grid----------------");
                    List<string> tilesToSkip = new List<string>();
                    List<int?> numbersToSkip = new List<int?>();

                    int tilesToFill = random.Next(5);
                    Console.WriteLine($"Tile Amount: {tilesToFill}");
                    for (int pos = 0; pos < tilesToFill; pos++) // variable in for loop determines how many positions will be filled
                    {
                        Console.WriteLine($"\nFilling {pos + 1}th position");
                        // checking for uniqueness of position of a tile
                        int row = -1; 
                        int col = -1; // is -1 because these had to be assigned to something, I chose -1
                        string checkUniqueTile = null;
                        try
                        {
                            do
                            {
                                row = random.Next(_tilesAcross);
                                col = random.Next(_tilesAcross);
                                checkUniqueTile = $"{row},{col}";
                                Console.WriteLine($"choosing position {checkUniqueTile}");
                            } while (tilesToSkip.Contains(checkUniqueTile));
                        }
                        catch (StackOverflowException ex)
                        {
                            Console.WriteLine("Stack overflow when choosing position " + ex);
                            CreateGrids(); // Restart the method
                        }
                        finally
                        {
                            Console.WriteLine($"chose position {checkUniqueTile}");
                            tilesToSkip.Add(checkUniqueTile);
                        }

                        // giving a unique number to that tile
                        int? number = null;
                        try
                        {
                            do
                            {
                                number = random.Next(1, _tileAmount + 1);
                                Console.WriteLine($"choosing number {number}");
                            } while (numbersToSkip.Contains(number));
                        }
                        catch (StackOverflowException ex)
                        {
                            Console.WriteLine("Stack overflow when choosing number" + ex);
                            CreateGrids(); // Restart the method
                        }
                        finally
                        {
                            Console.WriteLine($"chose number {number}");
                            numbersToSkip.Add(number);
                            grid._tiles[row, col] = number;
                        }
                        Console.WriteLine($"Added {pos + 1}th position; number {number} at [{row},{col}]");
                    }
                }
                return grids;
            }
        }
    }
}
// 1 2 3
// 4 5 6   row check => x -+ 1
// 7 8 9   col check => x -+ 3