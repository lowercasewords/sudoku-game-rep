using System;
namespace sudoku_game
{
    public class Map
    {
        Grid[] grids;

        /// <summary>
        ///  Creates a Map, note to me at start of project: be careful with Grid constructors,
        ///  you might have changed the required paramaters
        /// </summary>
        public Map() : this(9) { }
        public Map(int gridAmount)
        {
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