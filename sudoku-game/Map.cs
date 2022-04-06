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
        private static Random random = new Random();
        public Grid[,] Grids { get; }
        private static int _singleTonCount = 0;

        internal static readonly int _gridAmount = 9;
        internal static readonly int _gridsAcross = (int)Math.Sqrt(_gridAmount);

        internal static readonly int _tileAmount = 9;
        internal static readonly int _tilesAcross = (int)Math.Sqrt(_tilesAcross);

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

            
        }

        public void PrintMap()
        {
            for (int gridRow = 0; gridRow < _gridsAcross; gridRow++)
            {
                for (int tileRow = 0; tileRow < _tilesAcross; tileRow++) // tile row
                {
                    for (int gridCol = 0; gridCol < _gridsAcross; gridCol++)
                    {
                        for (int tileCol = 0; tileCol < _tilesAcross; tileCol++)
                        {
                            int? tile = Grids[gridRow, gridCol].Tiles[tileRow, tileCol];
                            Console.Write(tile is null ? "-" : tile.ToString());
                            Console.Write(tileCol + 1 == _tilesAcross ? "|" : "");
                        }
                        Console.Write(gridCol + 1 == _gridsAcross ? "\n" : "");
                    }
                }
                Console.WriteLine("------------");
            }
            
        }

        /// <summary>
        /// The only way to create grids outside of its class
        /// </summary>
        /// <returns>
        /// An array of grid objects with non-repeating tile numbers: horizontally and vertically
        /// </returns>
        internal void CreateGrids()
        {
            //DEBUG LOG
            Console.WriteLine("Creating Grid...");
            for (int gridRow = 0; gridRow < _gridsAcross; gridRow++)
            {
                for (int gridCol = 0; gridCol < _gridsAcross; gridCol++)
                {
                    Grids[gridRow, gridCol] = new Grid();
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
                            Grids[gridRow, gridCol].Tiles[tileRow, tileCol] = number;
                        }
                        Console.WriteLine($"Added {pos + 1}th position; number {number} at [{tileRow},{tileCol}]");
                    }
                }
            }
            Array.ForEach(allWayTilesToSkip.ToArray(), x => Console.WriteLine(x));
        }
    }
}
// 1 2 3
// 4 5 6   row check => x -+ 1
// 7 8 9   col check => x -+ 3