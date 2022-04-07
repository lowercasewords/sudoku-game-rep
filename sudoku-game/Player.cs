using System;
namespace sudoku_game
{
    public class Player
    {
        private Map playersMap;
        public Player(Map playersMap)
        {
            this.playersMap = playersMap;
        }

        public void MakeMove()
        {
            Console.WriteLine("Choosing Grid:");
            Console.Write("\tGrid Row: ");
            if(!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int gridRow)
                || gridRow < Map.GridsAcross
                || gridRow > Map.GridsAcross)
            {
                Console.WriteLine("Invalid Grid Row!");
                MakeMove();
            }
            Console.Write("\n\tGrid columns: ");
            if(!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int gridCol)
                || gridCol < Map.GridsAcross
                || gridCol > Map.GridsAcross)
            {
                Console.WriteLine("Invalid Grid Column!");
                MakeMove();
            }
            Console.WriteLine("");


            Console.WriteLine("Choosing Tile:");
            Console.Write("\tTile Row: ");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int tileRow)
                || tileRow < Grid.TilesAcross
                || tileRow > Grid.TilesAcross)
            {
                Console.WriteLine("Invalid Tile Row!");
                MakeMove();
            }
            Console.Write("\n\tTile columns: ");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int tileCol)
                || tileCol < Grid.TilesAcross
                || tileCol > Grid.TilesAcross)
            {
                Console.WriteLine("Invalid Tile Column!");
                MakeMove();
            }
            Console.WriteLine("");


            Console.WriteLine("Choose a number");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int number)
                || number < 0
                || number > 9)
            {
                Console.WriteLine("Invalid number!");
                MakeMove();
            }
            playersMap.Grids[gridRow, gridCol].Tiles[tileCol, tileRow] = number;
        }
    }
}
