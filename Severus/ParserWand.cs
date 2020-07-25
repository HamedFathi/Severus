using System;
using System.Collections.Generic;

namespace Severus
{
    public abstract class ParserWand<TToken, TResult>
        where TToken : Enum
        where TResult : ParserErrorReporter, new()
    {
        private readonly IEnumerator<Token<TToken>> _tokens;
        // private readonly string _input;

        protected ParserWand(LexerResult<TToken> lexerResult)
        {
            _tokens = lexerResult.Tokens.GetEnumerator();
            // _input = lexerResult.Input;
            Read(); // Start tokens processing, now 'Peek()' has a value and Current is not null.
        }

        public abstract TResult Parse();

        protected bool Read()
        {
            return _tokens.MoveNext();
        }

        protected void Read<T>(Func<T> func)
        {
            var status = Read();
            if (!status) func();
        }

        protected void Skip()
        {
            Read();
        }
        protected Token<TToken> Peek()
        {
            return _tokens.Current;
        }

        protected bool IsEndOfInput()
        {
            return _tokens.Current == null;
        }


        protected bool IsMatchType(TToken tokenType)
        {
            if (!IsEndOfInput())
            {
                return tokenType.Equals(Peek().Type);
            }
            return false;
        }

        protected bool IsMatchTypes(params TToken[] tokenTypes)
        {
            if (!IsEndOfInput())
            {
                foreach (var item in tokenTypes)
                {
                    return item.Equals(Peek().Type);
                }
            }
            return false;
        }

        protected bool IsMatchValue(string value, bool ignoreCase = true)
        {
            if (!IsEndOfInput())
            {
                return ignoreCase ? value.ToLower() == Peek().Value.ToLower() : value == Peek().Value;
            }
            return false;
        }

        protected bool IsMatchValues(params string[] values)
        {
            if (!IsEndOfInput())
            {
                foreach (var item in values)
                {
                    return item == Peek().Value;
                }
            }
            return false;
        }

        protected bool IsMatchValuesIgnoreCase(params string[] values)
        {
            if (!IsEndOfInput())
            {
                foreach (var item in values)
                {
                    return item.ToLower() == Peek().Value.ToLower();
                }
            }
            return false;
        }

        protected virtual string Error(Token<TToken> currentToken, string expected, string message = "")
        {
            if (currentToken != null && !string.IsNullOrEmpty(expected))
            {
                return $"Expecting '{expected}' but got '{currentToken.Value}' ({currentToken.Line}:{currentToken.Start})"
                    + (string.IsNullOrEmpty(message) ? "" : Environment.NewLine + message);
            }
            else if (currentToken == null)
            {
                throw new ArgumentNullException($"'{nameof(currentToken)}' argument is null.");
            }
            else
            {
                throw new ArgumentNullException($"'{nameof(expected)}' argument is null or empty.");
            }
        }

    }
}
