using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace sudoku_game
{
    /// <summary>
    /// Represents a Sudoku game map with nested grid class
    /// </summary>
    [Serializable]
    public class Map
    {
        private static readonly Random _random = new Random();
        public Grid[,] Grids { get; } = new Grid[,]
        {   { new Grid(), new Grid(), new Grid() },
            { new Grid(), new Grid(), new Grid() },
            { new Grid(), new Grid(), new Grid() } };

        private static int _singleTonCount = 0;

        public static int GridAmount { get; } = 9;
        public static int GridsAcross { get; } = (int)Math.Sqrt(GridAmount);

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
            Console.WriteLine("------------");
            for (int gridRow = 0; gridRow < GridsAcross; gridRow++) // grid row
            {
                for (int tileRow = 0; tileRow < Grid.TilesAcross; tileRow++) // tile row
                {
                    for (int gridCol = 0; gridCol < GridsAcross; gridCol++) // grid col
                    {
                        for (int tileCol = 0; tileCol < Grid.TilesAcross; tileCol++) // tile col
                        {
                            Number number = Grids[gridRow, gridCol].Tiles[tileRow, tileCol];
                            Console.Write(number); // print the number
                            Console.Write(tileCol + 1 == Grid.TilesAcross ? "|" : ""); // place right boundry after last column in a grid
                        }
                        Console.Write(gridCol + 1 == GridsAcross ? "\n" : ""); // go on new grid line
                    }
                }
                Console.WriteLine("------------");
            }
        }
        /// <summary>
        /// if the amount of existing numbers equal to max possible numbers on grid: the game is over
        /// </summary>
        /// <returns>true if game is over</returns>
        public bool GameOver()
        {
            Console.WriteLine(numberInfoList.Count);
            Console.WriteLine(GridAmount * Grid.TileAmount);
            return numberInfoList.Count == GridAmount * Grid.TileAmount;
        }
        public void ValidMoveCheck(object source, Player.MoveInfoArgs args)
        {
            // assingning args.ValidNumber as a call back to the source
            if (RepeatsInGrids(args.GridCount, args.NumberInfo) || RepeatInTile(args.NumberInfo)){
                Console.WriteLine("Your number is repetitive! Choose another one");
                args.ValidNumber = false;
            }
            else
            {
                args.ValidNumber = true;
            }
        }

        public void DeleteNumberInfo(object source, Player.MoveInfoArgs args)
        {
            Console.WriteLine(numberInfoList.Remove(args.NumberInfo));
            Console.WriteLine("");
        }
        ///<summary>
        ///Format: {number}:{tileRow},{tileCol}|{tileCount}
        ///Is used to store all created numbers 
        ///</summary>
        List<string> numberInfoList = new List<string>();

        /// <summary>
        /// The only way to create grids outside of its class
        /// </summary>
        /// <returns>
        /// An array of grid objects with non-repeating tile numbers: horizontally and vertically
        /// </returns>
        private void CreateGrids()
        {
            //DEBUG LOG
            Console.WriteLine("Start filling the Grids");
            // checking for uniqueness of position of a tile in all directions and inside a grid
            int tileRow = -1;
            int tileCol = -1; // is -1 because these had to be assigned to something, I chose -1

            for (int gridRow = 0; gridRow < Grid.TilesAcross; gridRow++)
            {
                for (int gridCol = 0; gridCol < Grid.TilesAcross; gridCol++)
                {
                    int gridCount = gridRow * 3 + gridCol + 1;

                    //DEBUG LOG
                    Console.WriteLine($"\nCreating {gridCount}th grid----------------");

                    int tilesToFill = _random.Next(5);
                    //DEBUG LOG
                    Console.WriteLine($"Tile Amount: {tilesToFill}");
                    for (int pos = 0; pos < tilesToFill; pos++) // variable in for loop determines how many positions will be filled
                    {
                        //DEBUG LOG
                        Console.WriteLine($"\nFilling {pos + 1}th position");

                        int number = default;
                        string numberInfoCreator = null;
                        try // try to assign non-repeating number without stackoverflow
                        {
                            do // tries to assign valid number inside one grid
                            {
                                tileRow = _random.Next(Grid.TilesAcross);
                                tileCol = _random.Next(Grid.TilesAcross);
                                Console.WriteLine($"choosing position [{tileRow},{tileCol}]");

                                number = _random.Next(1, Grid.TileAmount + 1);
                                //DEBUG LOG
                                Console.WriteLine($"choosing number {number}");

                                numberInfoCreator = $"{number}:{tileRow},{tileCol}|{gridCount}";
                                // method in while loop return true if something repeats!
                            } while (RepeatInTile(numberInfoCreator) || RepeatsInGrids(gridCount, numberInfoCreator));
                        }
                        catch (StackOverflowException ex)
                        {
                            Console.WriteLine("Restarting a Grid Creator Method: Stack Overflow when choosing position " + ex);
                            CreateGrids(); // Restart the method
                        }
                        catch(NullReferenceException ex)
                        {
                            Console.WriteLine("Restarting a Grid Creator Method: Null Reference when accessing Number Info Creator " + ex);
                        }
                        finally // finnaly assign a number into a position
                        {
                            numberInfoList.Add($"{number}:{tileRow},{tileCol}|{gridCount}");
                            //DEBUG LOG
                            //if (numberInfoCreator is not null) {
                                Console.WriteLine($"chose position [{tileRow},{tileCol}]");
                            //}
                            //DEBUG LOG
                            Console.WriteLine($"chose number {number}");
                            Grid grid = Grids[gridRow, gridCol];
                            grid.Tiles[tileRow, tileCol].Value = number;
                        }
                        Console.WriteLine($"Added {pos + 1}th position; number {number} at [{tileRow},{tileCol}]");
                    }
                }
            }
            Array.ForEach(numberInfoList.ToArray(), x => Console.WriteLine(x));
        }
        /// <summary>
        /// Checks if a number repeats in the specific tile
        /// </summary>
        /// <param name="numberInfoToCheck">a number info string to be checked as a string: "{number}:{tileRow},{tileCol}|{gridCount}"</param>
        /// <returns>true if a duplicate found</returns>
        private bool RepeatInTile(string numberInfoToCheck)
        {
            foreach (var numberInfo in numberInfoList)
            {
                if ((numberInfo[0] == numberInfoToCheck[0]
                    && numberInfo[numberInfo.Length - 1] == numberInfoToCheck[numberInfoToCheck.Length - 1])
                    || (numberInfo.Substring(2,5) == numberInfoToCheck.Substring(2, 5) && numberInfoToCheck[0].Equals("-")))
                {
                    Console.WriteLine("Repeats in the tile!");
                    Console.WriteLine($"{numberInfo} \\ {numberInfoToCheck}");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a number repeats horizontally or vertically <b>when 9x9 grid is complete!</b>
        /// </summary>
        /// <param name="gridCount">In what grid the current number is</param>
        /// <param name="numberInfo">Number's position represented as a string: "{number}:{tileRow},{tileCol}|{gridCount}"</param>
        /// <returns>true if a duplicate found</returns>
        private bool RepeatsInGrids(int gridCount, string numberInfo)
        {
            bool returnResult = false;

            int tileRow = int.Parse(new Regex("\\d+?(?=,)").Match(numberInfo).Value);
            int tileCol = int.Parse(new Regex("(?<=,)\\d+").Match(numberInfo).Value);

            string gridToCheckHoriz = null;
            string gridToCheckVert = null;
            switch (gridCount)
            {
                case 1:
                case 2:
                case 3:
                    gridToCheckVert = $"[{gridCount + 3}{gridCount + 6}]";
                    break;
                case 4:
                case 5:
                case 6:
                    gridToCheckVert = $"[{gridCount - 3}{gridCount + 3}]";
                    break;
                case 7:
                case 8:
                case 9:
                    Console.WriteLine("Check number in bottom grid row");
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

            Regex horizCheck = new Regex($"{numberInfo[0]}:\\d,{tileCol}\\|{gridToCheckVert}");
            Regex vertCheck = new Regex($"{numberInfo[0]}:{tileRow},\\d\\|{gridToCheckHoriz}");

            Array.ForEach(numberInfoList.ToArray(), x =>
            {
                if (horizCheck.IsMatch(x))
                {
                    Console.WriteLine($"{x} has matched {horizCheck}");
                    returnResult = true;
                    if (!vertCheck.IsMatch(x))
                    {
                        Console.WriteLine($"{x} has matched {vertCheck}");

                    }
                    return;
                }
                if (vertCheck.IsMatch(x))
                {
                    Console.WriteLine($"{x} has matched {vertCheck}");
                    returnResult = true;
                    return;
                }
            });
            //DEBUG START:
            if (returnResult)
            {
                Console.WriteLine("Repeats In The Grid!");
            }
            //DEBUG END
            return returnResult;
        }
    }
}
// 1 2 3
// 4 5 6   row check => x -+ 1
// 7 8 9   col check => x -+ 3