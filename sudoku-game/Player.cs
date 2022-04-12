using System;
using System.Threading.Tasks;

namespace sudoku_game
{
    [Serializable]
    public class Player
    {
        private Map playersMap;
        public event EventHandler<MoveInfoArgs> ClearNumbers;
        public event EventHandler<MoveInfoArgs> PlaceNumber;
        public Player(Map playersMap)
        {
            this.playersMap = playersMap;
            ClearNumbers += playersMap.DeleteNumberInfo;
            PlaceNumber += playersMap.PlaceMoveCheck;
        }

        //MoveInfoArgs moveInfoArgs = new MoveInfoArgs();

        public void MakeMove()
        {
            playersMap.PrintMap();

            Array.ForEach(playersMap.numberInfoList.ToArray(), x => Console.WriteLine(x));
            Console.WriteLine(playersMap.numberInfoList.Count);
            var i = 0;
            for (int gR = 0; gR < 3; gR++)
            {
                for (int tR = 0; tR < 3; tR++)
                {
                    for (int gC = 0; gC < 3; gC++)
                    {
                        for (int tC = 0; tC < 3; tC++)
                        {
                            if (playersMap.Grids[gR, gC].Tiles[tR, tC].Value != null)
                            {
                                i++;
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"In actuallity: {i}");
            Console.WriteLine("Choosing Grid:");
            Console.Write("\tGrid Row: ");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int gridRow)
                || gridRow <= 0
                || gridRow > Map.GridsAcross)
            {
                Console.WriteLine(" Invalid Grid Row! Try Again\n");
                return;
            }
            Console.Write("\n\tGrid Column: ");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int gridCol)
                || gridCol <= 0
                || gridCol > Map.GridsAcross)
            {
                Console.WriteLine(" Invalid Grid Column! Try Again\n");
                return;
            }
            Console.WriteLine("");

            Console.WriteLine("Choosing Tile:");
            Console.Write("\tTile Row: ");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int tileRow)
                || tileRow <= 0
                || tileRow > Grid.TilesAcross)
            {
                Console.WriteLine(" Invalid Tile Row! Try Again\n");
                return;
            }
            Console.Write("\n\tTile Column: ");
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out int tileCol)
                || tileCol <= 0
                || tileCol > Grid.TilesAcross)
            {
                Console.WriteLine(" Invalid Tile Column! Try Again\n");
                return;
            }
            Console.WriteLine("");


            Console.Write("Choose a Number (you can override existing numbers by pressing enter:");
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
                return;
            }
            Console.WriteLine("");


            // instance with info about the current number information for its subscribers to work with!
            MoveInfoArgs moveInfoArgs = new MoveInfoArgs(--gridRow, --gridCol, --tileRow, --tileCol, new Number(number));

            //throw new Exception("Delete event is not called when a number is replaced by another number!");
            // publishing events
            if (moveInfoArgs.NumberObj.Value is null)
            {
                ClearNumbers(this, moveInfoArgs);
            }
            PlaceNumber(this, moveInfoArgs);

            // receiving callback, recurse if a number is repetitive
            if (!moveInfoArgs.ValidNumber)
            {
                MakeMove();
            }
            else
            {
                playersMap.Grids[gridRow, gridCol].Tiles[tileRow, tileCol].Value = number;
            }
        }
        public class MoveInfoArgs : EventArgs
        {
            // Valid Number is used as a callback to publisher
            public bool ValidNumber { get; set; }

            public string NumberInfo { get; }
            public Number NumberObj { get; }
            public int GridCount { get; }

            public MoveInfoArgs(int gridRow, int gridCol, int tileRow, int tileCol, Number number)
            {
                GridCount = gridRow * 3 + 1 + gridCol;

                this.NumberObj = number;
                NumberInfo = $"{number}:{tileRow},{tileCol}|{GridCount}";

                Console.WriteLine(NumberInfo);
            }

            ~MoveInfoArgs()
            {
                Console.WriteLine("I'm dead");
            }
        }
    }
}