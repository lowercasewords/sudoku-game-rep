using System;

namespace sudoku_game
{
    class Map
    {
        Grid[] grids;
        
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

            grids = new Grid[gridAmount];
            for (int i = 0; i < gridAmount; i++)
            {
                grids[i] = new Grid();
            }
        }
    }
}
// 1 2 3
// 4 5 6   horizontal check => x - 1
// 7 8 9     vertical check => x - 3