using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


// if you assign numbers that repeat with pre-created numbers, they won't be assigned until you assing a non-repeating number.
// if your number intersects with a number you have created, it will skip map printing and grid row assignment or will just put
//it without checking?
// all happens because they are not put into the database
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

        ///<summary>
        ///Format: {number}:{tileRow},{tileCol}|{tileCount}
        ///Is used to store all created numbers 
        ///</summary>
        public List<string> numberInfoList = new List<string>();

        int createdNumbers = 0;

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
            return numberInfoList.Count == GridAmount * Grid.TileAmount;
        }
        // when a number is placed in place of null
        public void PlaceMoveCheck(object source, Player.MoveInfoArgs args)
        {
            // assingning args.ValidNumber as a call back to the source
            if (RepeatsInGrids(args.GridCount, args.NumberInfo) || RepeatInTile(args.NumberInfo)){
                Console.WriteLine("Your number is repetitive! Choose another one");
                args.ValidNumber = false;
            }
            else
            {
                // add a number to numberInfo if there was one
                if (args.NumberObj.Value != null)
                {
                    foreach (var numberInfo in numberInfoList)
                    {
                        if(args.NumberInfo.Substring(2, 5) == numberInfo.Substring(2, 5))
                        {
                            DeleteNumberInfo(source, new Player.MoveInfoArgs(numberInfo));
                            break;
                        }
                    }
                        numberInfoList.Add(args.NumberInfo);
                }
                args.ValidNumber = true;
            }
        }
        // is called when the number is replaced by anothe number or deleted 
        public void DeleteNumberInfo(object source, Player.MoveInfoArgs args)
        {
            Regex numberInfoToCheck = new Regex($".*{args.NumberInfo.Substring(2, 5)}");
            //numberInfoList.Remove(args.NumberInfo);
            Array.ForEach(numberInfoList.ToArray(), x =>
            {
                // can only try to remove a number if it was created by user
                if (numberInfoToCheck.IsMatch(x) && numberInfoList.IndexOf(x) >= createdNumbers)
                {
                    numberInfoList.Remove(x);
                    //throw new Exception("This doesn't work, string doesn't want to be" +
                    //    "deleted if replaced with another number because it's being called " +
                    //    "only if number is replaced by null");

                }
            });
        }
        
        /// <summary>
        /// The only way to create grids outside of its class
        /// </summary>
        /// <returns>
        /// An array of grid objects with non-repeating tile numbers: horizontally and vertically
        /// </returns>
        private void CreateGrids()
        {
            // checking for uniqueness of position of a tile in all directions and inside a grid
            int tileRow = -1;
            int tileCol = -1; // is -1 because these had to be assigned to something, I chose -1

            for (int gridRow = 0; gridRow < Grid.TilesAcross; gridRow++)
            {
                for (int gridCol = 0; gridCol < Grid.TilesAcross; gridCol++)
                {
                    int gridCount = gridRow * 3 + gridCol + 1;

                    int tilesToFill = _random.Next(5);
                    for (int pos = 0; pos < tilesToFill; pos++) // variable in for loop determines how many positions will be filled
                    {
                        int number = default;
                        string numberInfoCreator = null;
                        try // try to assign non-repeating number without stackoverflow
                        {
                            do // tries to assign valid number inside one grid
                            {
                                tileRow = _random.Next(Grid.TilesAcross);
                                tileCol = _random.Next(Grid.TilesAcross);

                                number = _random.Next(1, Grid.TileAmount + 1);

                                numberInfoCreator = $"{number}:{tileRow},{tileCol}|{gridCount}x";
                                // method in while loop return true if something repeats!
                            } while (RepeatInTile(numberInfoCreator) || RepeatsInGrids(gridCount, numberInfoCreator));
                        }
                        catch (StackOverflowException ex)
                        {
                            Console.WriteLine($"Restarting a Grid Creator Method: Stack Overflow when choosing position {ex}");
                            CreateGrids(); // Restart the method
                        }
                        catch(NullReferenceException ex)
                        {
                            Console.WriteLine($"Restarting a Grid Creator Method: Null Reference when accessing Number Info Creator {ex}");
                        }
                        finally // finnaly assign a number into a position
                        {
                            //the numbers created in the grid has extra 'x' because this will prevent the game from deleting these
                            numberInfoList.Add(numberInfoCreator);
                            Grid grid = Grids[gridRow, gridCol];
                            grid.Tiles[tileRow, tileCol].Value = number;
                        }
                    }
                }
            }
            createdNumbers = numberInfoList.Count;
            //Console.Clear();
        }

        public void DebugInfo()
        {
            Array.ForEach(numberInfoList.ToArray(), x =>
            {
                Console.WriteLine(x);
            });
            Console.WriteLine($"In Number Info: {numberInfoList.Count}");
            var i = 0;
            for (int gridRow = 0; gridRow < GridsAcross; gridRow++) // grid row
            {
                for (int tileRow = 0; tileRow < Grid.TilesAcross; tileRow++) // tile row
                {
                    for (int gridCol = 0; gridCol < GridsAcross; gridCol++) // grid col
                    {
                        for (int tileCol = 0; tileCol < Grid.TilesAcross; tileCol++) // tile col
                        {
                            var number = Grids[gridRow, gridCol].Tiles[tileRow, tileCol].Value;
                            if (number != null)
                            {
                                i++;
                                Console.WriteLine(number);
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"In Grid property {i}");
        }

        /// <summary>
        /// Checks if a number repeats in the specific tile
        /// </summary>
        /// <param name="numberInfoToCheck">a number info string to be checked as a string: "{number}:{tileRow},{tileCol}|{gridCount}" if it number was made by a game, it would have an 'x' in the end</param>
        /// <returns>true if a duplicate found</returns>
        private bool RepeatInTile(string numberInfoToCheck)
        {
            foreach (var numberInfo in numberInfoList)
            {
                // if tries to override pre-made numbers, does look at its own position
                if(numberInfo.Substring(2, 5) == numberInfoToCheck.Substring(2, 5) && numberInfo.Contains('x'))
                {
                    Console.WriteLine("Cannot override pre-made numbers");
                    return true;
                }
                // if tries to put the same number in the same grid, doesn't look at its own position
                else if (numberInfo[0] == numberInfoToCheck[0] && numberInfo[6] == numberInfoToCheck[6])
                {
                    Console.WriteLine("Repeats in the tile!");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a number repeats horizontally or vertically <b>when 9x9 grid is complete!</b>
        /// </summary>
        /// <param name="gridCount">In what grid the current number is</param>
        /// <param name="numberInfoToCheck">Number's position represented as a string: "{number}:{tileRow},{tileCol}|{gridCount} if it number was made by a game, it would have an 'x' in the end"</param>
        /// <returns>true if a duplicate found</returns>
        private bool RepeatsInGrids(int gridCount, string numberInfoToCheck)
        {
            int tileRow = int.Parse(new Regex("\\d+?(?=,)").Match(numberInfoToCheck).Value);
            int tileCol = int.Parse(new Regex("(?<=,)\\d+").Match(numberInfoToCheck).Value);

            string gridToCheckHoriz = null;
            string gridToCheckVert = null;
            switch (gridCount) // grids to check horizontally
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
            switch (gridCount) // grids to check vertically
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
                    gridToCheckVert = $"[{gridCount - 3}{gridCount - 6}]";
                    break;
            }
            Regex horizCheck = new Regex($"{numberInfoToCheck[0]}:{tileRow},\\d\\|{gridToCheckHoriz}");
            Regex vertCheck = new Regex($"{numberInfoToCheck[0]}:\\d,{tileCol}\\|{gridToCheckVert}");

            foreach (string numberInfo in numberInfoList)
            {
                if (horizCheck.IsMatch(numberInfo))
                {
                    Console.WriteLine("Repeats In The Grid Horizontally!");
                    return true;
                }
                else if (vertCheck.IsMatch(numberInfo))
                {
                    Console.WriteLine("Repeats In The Grid Vertically!");
                    return true;
                }
            }
            return false;
        }
    }
}