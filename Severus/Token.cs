using System;
using System.Linq;

namespace Severus
{
    public class Token<TToken> where TToken : Enum
    {
        public Token(TToken type, string value, int position, int line, int column)
        {
            Type = type;
            Value = value;
            End = position;
            Line = line;
            Column = column;
        }

        public TToken Type { get; }
        public string Value { get; }
        public int End { get; }
        public int Line { get; }
        public int Column { get; }
        public int Start
        {
            get
            {
                return End - Length;
            }
        }
        public int Length => Value.Length;
        public bool IsMultiLine => Value.Contains('\n');
    }
}
