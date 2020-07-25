using System;
using System.Collections.Generic;

namespace Severus
{
    public sealed class LexerResult<TToken> where TToken : Enum
    {
        public string Input { get; internal set; }
        public IEnumerable<Token<TToken>> Tokens { get; internal set; }

        public LexerResult()
        {
            Input = "";
            Tokens = new List<Token<TToken>>();
        }

    }
}
