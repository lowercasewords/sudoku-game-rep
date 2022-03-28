using System;
using System.Text.RegularExpressions;
using System.Linq;
class Grid
{
    /// <summary>
    /// first element in the 'tiles' array represented in the game in
    /// upper-left corner, the last element in bottom-right
    /// </summary>
    int?[] _tiles;
    public int?[] Tiles
    {
        get
        {
            return _tiles;
        }
    }

    public Grid() : this(9) { }
    public Grid(int tileAmount) 
    {
        do
        {
            _tiles = new int?[tileAmount];
            Regex excludePos = new Regex("[.]");
            int randomPos;
            for (int i = 0; i < _tiles.Length; i++)
            {
                randomPos = new Random().Next(1, 10);
                /// <summary>
                /// if the position was already taken,
                /// repeat for-loop and exclude the position 
                /// </summary>
                if (excludePos.IsMatch(randomPos.ToString()))
                {
                    i--;
                    Console.WriteLine("-");
                    continue;
                }
                else
                { AddToEndRegex(ref excludePos, randomPos); }

                Console.WriteLine($"for-loop was completed {i} times");
                Console.WriteLine($"random position: {randomPos}");
                Console.WriteLine($"exclude position: {excludePos}");

                _tiles[i] = randomPos;
            } 
        } while (
 _tiles.Where(x => x != null).GroupBy(x => x).Any(x => x.Count() > 1)
            );
    }
    private void AddToEndRegex<T>(ref Regex regex, T adding)
    {
        Console.WriteLine("Match was found");
        regex = new Regex(
                        regex.ToString().Substring(0, regex.ToString().Length - 1)
                        + adding + ']');
    }
    public void Ensure()
    {
        
    }
}