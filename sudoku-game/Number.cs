using System;
namespace sudoku_game
{
    [Serializable]
    public class Number
    {
        public static int MaxValue { get; } = 9;
        public static int MinValue { get; } = 1;
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
                    //throw new InvalidOperationException("Invalid integer was assigned to Number value");
                    _value = null;
                }
                _value = value;
            }
        }
        public Number()
        {

        }
        public Number(int? value)
        {
            this.Value = value;
        }
        public override string ToString()
        {
            return _value is null ? "-" : _value.ToString();
        }
    }
}
