using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            for (int gridRow = 0; gridRow < Grid._gridsAcross; gridRow++)
            {
                for (int tileRow = 0; tileRow < Grid._tilesAcross; tileRow++) // tile row
                {
                    for (int gridCol = 0; gridCol < Grid._gridsAcross; gridCol++)
                    {
                        for (int tileCol = 0; tileCol < Grid._tilesAcross; tileCol++)
                        {
                            int? tile = Grids[gridRow, gridCol].Tiles[tileRow, tileCol];
                            Console.Write(tile is null ? "-" : tile.ToString());
                            Console.Write(tileCol + 1 == Grid._tilesAcross ? "|" : "");
                        }
                        Console.Write(gridCol + 1 == Grid._gridsAcross ? "\n" : "");
                    }
                }
                Console.WriteLine("------------");
            }
            
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

            /// <summary>
            /// Grids have to be created by using static 'CreateGrids' method
            /// </summary>
            private Grid() 
            {
                //DEBUG LOG
                Console.WriteLine("Instantiating Grid Class");
                _tiles = new int?[_tilesAcross,_tilesAcross];
            }
            /// <summary>
            /// Checks if a number repeats horizontally or vertically <b>when 9x9 grid is complete!</b>
            /// </summary>
            /// <param name="list">A list of all previous numbers with their tile pos and grid num</param>
            /// <param name="gridCount">In what grid the current number is</param>
            /// <param name="tilePos">Position of current number</param>
            /// <param name="tileNum">A number</param>
            /// <returns></returns>
            private static bool RepeatsInGrids(ref List<string> list, int gridCount, string tilePos, int tileNum) 
            {
                int tileRow = int.Parse(new Regex("\\d+?(?=,)").Match(tilePos).Value);
                int tileCol = int.Parse(new Regex("(?<=,)\\d+").Match(tilePos).Value);

                Regex gridToCheckHoriz = null;
                Regex gridToCheckVert = null;
                switch (gridCount)
                {
                    case 1:
                    case 2:
                    case 3:
                        Console.WriteLine("Check number in top row");
                        gridToCheckHoriz = new Regex($"[{gridCount + 1}{gridCount + 2}]");
                        gridToCheckVert = new Regex($"[{gridCount + 3}{gridCount + 6}]");
                        break;
                    case 4:
                    case 5:
                    case 6:
                        Console.WriteLine("Check number in middle row");
                        gridToCheckHoriz = new Regex($"[{gridCount - 1}{gridCount + 1}]");
                        gridToCheckVert = new Regex($"[{gridCount - 3}{gridCount + 3}]");
                        break;
                    case 7:
                    case 8:
                    case 9:
                        Console.WriteLine("Check number in bottom row");
                        gridToCheckHoriz = new Regex($"[{gridCount - 1}{gridCount - 2}]");
                        gridToCheckVert = new Regex($"[{gridCount - 3}{gridCount - 6}]");
                        break; 
                }
                Regex anyNum = new Regex("\\d");
                if( list.Contains($"{tileNum}:{anyNum},{tileCol}|{gridToCheckVert}")
                    || list.Contains($"{tileNum}:{tileRow},{anyNum}|{gridToCheckHoriz}"))
                {
                    Console.WriteLine("Something was repeated!");
                    return true;
                }
                return false;
            }
            /// <summary>
            /// The only way to create grids outside of its class
            /// </summary>
            /// <returns>
            /// An array of grid objects with non-repeating tile numbers: horizontally and vertically
            /// </returns>
            internal static Grid[,] CreateGrids()
            {
                //DEBUG LOG
                Console.WriteLine("Creating Grid...");
                grids = new Grid[_gridsAcross, _gridsAcross];
                for (int gridRow = 0; gridRow < _gridsAcross; gridRow++)
                {
                    for (int gridCol = 0; gridCol < _gridsAcross; gridCol++)
                    {
                        grids[gridRow, gridCol] = new Grid();
                    }
                }

                //DEBUG LOG
                Console.WriteLine("Start filling the Grids");
                // checking for uniqueness of position of a tile in all directions and inside a grid
                int tileRow = -1;
                int tileCol = -1; // is -1 because these had to be assigned to something, I chose -1

                // row * 3 + col + 1
                ///<summary>
                ///Format: {number}:{tileRow},{tileCol},
                ///</summary>
                List<string> allWayTilesToSkip = new List<string>();
                
                for (int gridRow = 0; gridRow < _tilesAcross; gridRow++)
                {
                    for (int gridCol = 0; gridCol < _tilesAcross; gridCol++)
                    {
                        int gridCount = gridRow * 3 + gridCol + 1;

                        //DEBUG LOG
                        Console.WriteLine($"\nCreating {gridCount}th grid----------------");
                        string tilesToSkip = "";
                        List<int?> numbersToSkip = new List<int?>();

                        int tilesToFill = random.Next(5);
                        //DEBUG LOG
                        Console.WriteLine($"Tile Amount: {tilesToFill}");
                        for (int pos = 0; pos < tilesToFill; pos++) // variable in for loop determines how many positions will be filled
                        {
                            //DEBUG LOG
                            Console.WriteLine($"\nFilling {pos + 1}th position");

                            string checkUniqueTile = null;
                            int? number = null;
                            try // try to assign non-repeating number without stackoverflow
                            {
                                do // inside one grid
                                {
                                    tileRow = random.Next(_tilesAcross);
                                    tileCol = random.Next(_tilesAcross);
                                    checkUniqueTile = $"{tileRow},{tileCol}";
                                    Console.WriteLine($"choosing position {checkUniqueTile}");

                                    number = random.Next(1, _tileAmount + 1);
                                    //DEBUG LOG
                                    Console.WriteLine($"choosing number {number}");

                                    // method in while loop return true if something repeats!
                                } while (
                                tilesToSkip.Contains(checkUniqueTile)
                                || numbersToSkip.Contains(number)
                                || RepeatsInGrids(ref allWayTilesToSkip, gridCount, checkUniqueTile, number ?? default(int)));
                            }
                            catch (StackOverflowException ex)
                            {
                                Console.WriteLine("Stack overflow when choosing position " + ex);
                                CreateGrids(); // Restart the method
                            }
                            finally // finnaly assign a number into a position
                            {
                                allWayTilesToSkip.Add($"{number}:{tileRow},{tileCol}|{gridCount}");

                                //DEBUG LOG
                                Console.WriteLine($"chose position {checkUniqueTile}");
                                tilesToSkip += checkUniqueTile;

                                //DEBUG LOG
                                Console.WriteLine($"chose number {number}");
                                numbersToSkip.Add(number);
                                grids[gridRow, gridCol]._tiles[tileRow, tileCol] = number;
                            }
                            Console.WriteLine($"Added {pos + 1}th position; number {number} at [{tileRow},{tileCol}]");
                        }
                    }
                }
                Array.ForEach(allWayTilesToSkip.ToArray(), x => Console.WriteLine(x));
                return grids;
            }
        }
    }
}
// 1 2 3
// 4 5 6   row check => x -+ 1
// 7 8 9   col check => x -+ 3