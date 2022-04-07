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
        public Grid[,] Grids { get; } = new Grid[,]
        {   { new Grid(), new Grid(), new Grid() },
            { new Grid(), new Grid(), new Grid() },
            { new Grid(), new Grid(), new Grid() } };

        private static int _singleTonCount = 0;

        internal static readonly int _gridAmount = 9;
        internal static int GridsAcross { get; } = (int)Math.Sqrt(_gridAmount);

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

            CreateGrids();
        }

        public void PrintMap()
        {
            //Console.WriteLine(_gridAmount);
            //Console.WriteLine(_gridsAcross);
            //Console.WriteLine(_tileAmount);
            //Console.WriteLine(_tilesAcross);
            for (int gridRow = 0; gridRow < GridsAcross; gridRow++) // grid row
            {
                for (int tileRow = 0; tileRow < Grid.TilesAcross; tileRow++) // tile row
                {
                    for (int gridCol = 0; gridCol < GridsAcross; gridCol++) // grid col
                    {
                        for (int tileCol = 0; tileCol < Grid.TilesAcross; tileCol++) // tile col
                        {
                            int? number = Grids[gridRow, gridCol].Tiles[tileRow, tileCol]; 
                            Console.Write(number is null ? "-" : number.ToString()); // print if not number exists
                            Console.Write(tileCol + 1 == Grid.TilesAcross ? "|" : ""); // place right boundry after last column in a grid
                        }
                        Console.Write(gridCol + 1 == GridsAcross ? "\n" : ""); // go on new grid line
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
        private void CreateGrids()
        {
            //DEBUG LOG
            //Console.WriteLine("Creating Grid...");
            //for (int gridRow = 0; gridRow < _gridsAcross; gridRow++)
            //{
            //    for (int gridCol = 0; gridCol < _gridsAcross; gridCol++)
            //    {
            //        Grids[gridRow, gridCol] = new Grid();
            //    }
            //}

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

            for (int gridRow = 0; gridRow < Grid.TilesAcross; gridRow++)
            {
                for (int gridCol = 0; gridCol < Grid.TilesAcross; gridCol++)
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
                            do // tries to assign valid number inside one grid
                            {
                                tileRow = random.Next(Grid.TilesAcross);
                                tileCol = random.Next(Grid.TilesAcross);
                                checkUniqueTile = $"{tileRow},{tileCol}";
                                Console.WriteLine($"choosing position {checkUniqueTile}");

                                number = random.Next(1, Grid._tileAmount + 1);
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
        //private bool RepeatInTile(string tilesToSkip, List<int?> numbersToSkip, NotFiniteNumberException)
        //{
        //    return tilesToSkip.Contains(checkUniqueTile) || numbersToSkip.Contains(number)
        //}



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
            return returnResult;
        }
    }
}
// 1 2 3
// 4 5 6   row check => x -+ 1
// 7 8 9   col check => x -+ 3