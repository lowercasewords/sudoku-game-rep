using System;

namespace sudoku_game
{
    class Map
    {
        Grid[] grids;
        
        public static int SingleTonCount { get; private set; } = 0;
        public Map()
        {
            ///<summary>
            /// Implementing SingleTon
            ///</summary>
            if (++SingleTonCount > 1)
                throw new Exception("You can't have more " +
                                    "than one instance of Map Class");
            int gridAmount = 9;

            if ((int)Math.Sqrt(gridAmount) != Math.Sqrt(gridAmount))
                throw new Exception("A map cannot contain");

            grids = new Grid[gridAmount];
            for (int i = 0; i < gridAmount; i++)
            {
                grids[i] = new Grid();
            }
        }
    }
}