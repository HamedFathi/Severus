using System;
using System.Collections.Generic;
using System.Text;

namespace ParserSample
{
    public enum SqlTokenType
    {
        Int,
        Float,
        Id,
        Digit,
        Letter,
        Keyword,
        Operator,
        Comma,
        Query,
        IdList,
        CondList,
        Cond,
        Term
    }
}
