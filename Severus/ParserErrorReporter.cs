using System.Collections.Generic;
using System.Linq;

namespace Severus
{
    public abstract class ParserErrorReporter
    {
        private List<string> _errors = new List<string>();
        public IReadOnlyCollection<string> Errors => _errors;

        public bool HasError => Errors.Any();

        public void AddError(string error)
        {
            _errors.Add(error);
        }
    }
}
