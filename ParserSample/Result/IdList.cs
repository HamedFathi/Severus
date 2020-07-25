using Severus;
using System.Collections.Generic;

namespace ParserSample
{
    public class IdList : ParserErrorReporter
    {
        public List<string> Ids { get; set; }
        public IdList()
        {
            Ids = new List<string>();
        }
    }
}
