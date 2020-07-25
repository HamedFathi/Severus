using Severus;
using System;
using System.Collections.Generic;

namespace ParserSample
{
    public class ConditionList : ParserErrorReporter
    {
        public List<ConditionItem> Conditions { get; set; }
        public ConditionList()
        {
            Conditions = new List<ConditionItem>();
        }
    }
}
