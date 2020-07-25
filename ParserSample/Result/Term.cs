using Severus;

namespace ParserSample
{
    public class Term : ParserErrorReporter
    {
        public string Value { get; set; }
        public SqlTokenType Type { get; set; }
    }
}
