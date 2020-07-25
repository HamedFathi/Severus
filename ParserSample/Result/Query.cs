using Severus;
using System.Linq;
using System.Text;

namespace ParserSample
{

    public class Query : ParserErrorReporter
    {
        public IdList Select { get; set; }
        public IdList From { get; set; }
        public ConditionList Where { get; set; }

    }
}
