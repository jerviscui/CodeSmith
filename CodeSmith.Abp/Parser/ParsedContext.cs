using System.Collections.Generic;
using System.Diagnostics;

namespace CodeSmith.Abp.Parser
{
    [DebuggerDisplay("Context: {ContextClass}")]
    public class ParsedContext
    {
        public ParsedContext()
        {
            Properties = new List<ParsedEntitySet>();
        }

        public string ContextClass { get; set; }

        public List<ParsedEntitySet> Properties { get; private set; }
    }
}