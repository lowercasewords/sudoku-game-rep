using System;
using System.Threading.Tasks;

namespace sudoku_game
{
    [Serializable]
    public class Player
    {
        private Map playersMap;
        public event EventHandler<MoveInfoArgs> DeleteNumber;
        public event EventHandler<MoveInfoArgs> PlaceNumber;
        public Player(Map playersMap)
        {
            this.playersMap = playersMap;
            DeleteNumber += playersMap.DeleteNumberInfo;
            PlaceNumber += playersMap.ValidMoveCheck;
        }

        //MoveInfoArgs moveInfoArgs = new MoveInfoArgs();

        public void MakeMove()
        {
            playersMap.PrintMap();

            Console.WriteLine("Choosing Grid:");
            Console.Write("\tGrid Row: ");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int gridRow)
                || gridRow <= 0
                || gridRow > Map.GridsAcross)
            {
                Console.WriteLine(" Invalid Grid Row! Try Again\n");
                MakeMove();
            }
            Console.Write("\n\tGrid Column: ");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int gridCol)
                || gridCol <= 0
                || gridCol > Map.GridsAcross)
            {
                Console.WriteLine(" Invalid Grid Column! Try Again\n");
                MakeMove();
            }
            Console.WriteLine("");

            Console.WriteLine("Choosing Tile:");
            Console.Write("\tTile Row: ");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int tileRow)
                || tileRow <= 0
                || tileRow > Grid.TilesAcross)
            {
                Console.WriteLine(" Invalid Tile Row! Try Again\n");
                MakeMove();
            }
            Console.Write("\n\tTile Column: ");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int tileCol)
                || tileCol <= 0
                || tileCol > Grid.TilesAcross)
            {
                Console.WriteLine(" Invalid Tile Column! Try Again\n");
                MakeMove();
            }
            Console.WriteLine("");


            Console.Write("Choose a Number (you can override existing numbers \\ press enter to delete):");
            int? ToNullableInt(string x)
            {
                if (int.TryParse(x, out int result)) return result;
                return null;
            }
            int? number = ToNullableInt(Console.ReadKey().KeyChar.ToString());
            if (number < Number.MinValue
                || number > Number.MaxValue)
            {
                Console.WriteLine(" Invalid number! Try Again\n");
                MakeMove();
            }
            Console.WriteLine("");


            // instance with info about the current number information for its subscribers to work with!
            MoveInfoArgs moveInfoArgs = new MoveInfoArgs(--gridRow, --gridCol, --tileRow, --tileCol, new Number(number));

            PlaceNumber(this, moveInfoArgs);
            if (!moveInfoArgs.ValidNumber)
            {
                MakeMove();
            }

            playersMap.Grids[gridRow, gridCol].Tiles[tileRow, tileCol].Value = number;
        }
        public class MoveInfoArgs : EventArgs
        {
            // Valid Number is used as a callback to publisher
            public bool ValidNumber { get; set; }

            public string NumberInfo { get; }
            public int GridCount { get; }

            public MoveInfoArgs(int gridRow, int gridCol, int tileRow, int tileCol, Number number)
            {
                GridCount = gridRow * 3 + 1 + gridCol;

                NumberInfo = $"{number}:{tileRow},{tileCol}|{GridCount}";

                Console.WriteLine(NumberInfo);
            }
        }
    }
}