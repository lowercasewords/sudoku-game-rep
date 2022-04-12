using System;
namespace sudoku_game
{
    [Serializable]
    public class Number
    {
        public static int MaxValue { get; } = 9;
        public static int MinValue { get; } = 1;
        public bool UserMade { get; set;  }
        private int? _value;
        public int? Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value > MaxValue || value < MinValue)
                {
                    _value = null;
                }
                _value = value;
            }
        }
        public Number()
        {

        }
        public Number(int? value, bool userMade)
        {
            Value = value;
            UserMade = userMade;
        }
        public Number(int? value) : this(value, false)
        {
            Console.WriteLine("Found you!");
        }
        public override string ToString()
        {
            //Console.ForegroundColor = ConsoleColor.Red;
            //return _value is null ? "-" : _value.ToString();
            if(_value is null)
            {
                return "-";
            }
            else // I know I could omit else condition -_-
            {
                return _value.ToString();
            }
        }
    }
}
