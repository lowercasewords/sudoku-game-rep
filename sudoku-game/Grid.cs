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
    static Grid[] _grids;
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
    public static Grid[] CreateGrids()
    {
        _grids = new Grid[_tileAmount];
        _grids = (Grid[])Enumerable.Repeat(new Grid(), _tileAmount);
        
        void AddToEndRegex<T>(ref Regex regex, T adding) where T : struct
        {
            regex = new Regex(
                            regex.ToString().Substring(0, regex.ToString().Length - 1)
                            + adding + ']');
        }
        do
        {
        ///<summary>
        /// fills up tiles with numbers in grids one by one
        /// </summary>
        for (int gi = 0; gi < _grids.Length; gi++)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            /// <summary>
            /// do/while loop is here to ensure no duplicates
            /// </summary>
            do
            {
                _grids[gi]._tiles = new int?[_tileAmount];
                Regex excludeValue = new Regex("[.]");
                int randomValue;
                for (int i = 0; i < _tileAmount; i++)
                {
                    /// <summary>
                    /// ensures single non-duplicate grid
                    /// </summary>
                    do
                    {
                        randomValue = new Random().Next(1, 10);
                    } while (excludeValue.IsMatch(randomValue.ToString()));
                    AddToEndRegex(ref excludeValue, randomValue);

                    _grids[gi]._tiles[i] = randomValue;
                }
            } while (
            _grids[gi]._tiles.Where(x => x != null).GroupBy(x => x).Any(x => x.Count() > 1));

            // should be done in the end for diagnostics
            if (watch.ElapsedMilliseconds * 1000 >= 10)
                CreateGrids();
            watch.Reset();
        }
        } while (GridCheck(ref _grids));
        
        return _grids;
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
        for (int times = 0; times < 6; times++)
        {
            for (int i = 0; i < _tileAmount / 3; i++)
            {
                for (int s = 0; s < _tileAmount / 3; s++)
                {
                    grid._tiles[i] == grid._tilesiles[s];
                }
            }
        }
    }
}
