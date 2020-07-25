using Severus;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserSample
{
    public class SqlLexer : LexerWand<SqlTokenType>
    {
        public SqlLexer(string input) : base(input)
        {

        }
        protected override Token<SqlTokenType> ReadToken()
        {
            SkipWhile(x => x.IsWhiteSpace());

            var ch = Peek();

            if (IsIdentifierCandidate(ch))
            {
                var value = ReadWhile(x => IsIdentifier(x));
                if (value.IsInEnum<SqlKeyword>(true))
                {
                    return new Token<SqlTokenType>(SqlTokenType.Keyword, value, Position, Line, Column);
                }
                else
                {
                    return new Token<SqlTokenType>(SqlTokenType.Id, value, Position, Line, Column);
                }
            }

            if (ch.IsDigit())
            {
                var value = ReadFloatNumber();
                if (value.Contains('.'))
                {
                    return new Token<SqlTokenType>(SqlTokenType.Float, value, Position, Line, Column);
                }
                else
                {
                    return new Token<SqlTokenType>(SqlTokenType.Int, value, Position, Line, Column);
                }
            }

            if (ch == ',')
            {
                var value = Read();
                return new Token<SqlTokenType>(SqlTokenType.Comma, value.ToString(), Position, Line, Column);
            }

            if (IsOperator(ch))
            {
                var value = Read();
                return new Token<SqlTokenType>(SqlTokenType.Operator, value.ToString(), Position, Line, Column);
            }
            return null;
        }



        private bool IsIdentifierCandidate(char c)
        {
            return c.IsRegexMatch("[_a-zA-Z]", false);
        }

        private bool IsIdentifier(char c)
        {
            return c.IsRegexMatch("[-_a-zA-Z0-9]", false);
        }

        private bool IsOperator(char c)
        {
            return "=<>".Contains(c);
        }
    }
}
