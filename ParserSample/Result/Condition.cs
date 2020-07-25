using Severus;

namespace ParserSample
{
    public class Condition : ParserErrorReporter
    {
        public string Id { get; set; }
        public string Operator { get; set; }
        public Term Term { get; set; }
    }
}
