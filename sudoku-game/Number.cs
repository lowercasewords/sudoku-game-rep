using System;
namespace sudoku_game
{
    public class Number
    {
        private int? _value;
        public int? Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value > 10 || value < 0)
                {
                    throw new InvalidOperationException("Your number should be in range of 1-9");
                }
                _value = value;
            }
        }

        public override string ToString()
        {
            return _value is null ? "-" : _value.ToString();
        }
    }
}
