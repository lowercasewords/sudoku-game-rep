using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Diagnostics;

class Grid
{
    /// <summary>
    /// first element in the 'tiles' array represented in the game in
    /// upper-left corner, the last element in bottom-right.
    /// It's not multidimensinal because it's easier
    /// to manipulate one array
    /// </summary>
    private static readonly int _tileAmount = 9;
    int?[] _tiles = new int?[_tileAmount];
    Grid[] grids;
    public int?[] Tiles
    {
        get { return _tiles; }
    }
    //private int _;
    private Grid()
    {
        //ResetGrid(ref grids);
    }
    /// <summary>
    /// The only way to create grids outside of its class
    /// </summary>
    /// <returns> An array of grid objects with non-repeating tile numbers: horizontally and vertically (b</returns>
    public static Grid[] CreateGrids(ref Grid[] grids)
    {
        grids = new Grid[_tileAmount];
        grids = (Grid[])Enumerable.Repeat(new Grid(), _tileAmount);
        
        void AddToEndRegex<T>(ref Regex regex, T adding) where T : struct
        {
            regex = new Regex(
                            regex.ToString().Substring(0, regex.ToString().Length - 1)
                            + adding + ']');
        }
        ///<summary>
        /// fills up tiles with numbers in grids one by one
        /// </summary>
        for (int gi = 0; gi < grids.Length; gi++)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            /// <summary>
            /// do/while loop is here to ensure no duplicates
            /// </summary>
            do
            {
                grids[gi]._tiles = new int?[_tileAmount];
                Regex excludePos = new Regex("[.]");
                int randomValue;
                for (int i = 0; i < _tileAmount; i++)
                {
                    /// <summary>
                    /// ensures single non-duplicate grid
                    /// </summary>
                    do
                    {
                        randomValue = new Random().Next(1, 10);
                    } while (excludePos.IsMatch(randomValue.ToString()));
                    AddToEndRegex(ref excludePos, randomValue);

                    grids[gi]._tiles[i] = randomValue;
                }
            } while (
            grids[gi]._tiles.Where(x => x != null).GroupBy(x => x).Any(x => x.Count() > 1)
            && GridCheck(ref grids));

            // should be done in the end for diagnostics
            if (watch.ElapsedMilliseconds * 1000 >= 10)
                CreateGrids(ref grids);
            watch.Reset();
        }
        return grids;
    }
    // 1 2 3
    // 4 5 6
    // 7 8 9
    /// <summary>
    /// works with 'CreateGrids' method, checks if there are no duplicates horizontally & vertically
    /// </summary>
    /// <param name="grid">grids to make non-repeating</param>
    /// <returns>if </returns>
    private static bool GridCheck(ref Grid[] grid)
    {
        //throw new NotImplementedException();
        
        
        do
        {

        } while (true);

        
    }
}