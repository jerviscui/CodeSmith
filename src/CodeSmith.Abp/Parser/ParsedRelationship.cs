using System.Collections.Generic;
using System.Diagnostics;

namespace CodeSmith.Abp.Parser
{
    [DebuggerDisplay("This: {ThisPropertyName}, Other: {OtherPropertyName}")]
    public class ParsedRelationship
    {
        public ParsedRelationship()
        {
            ThisProperties = new List<string>();
            JoinThisColumn = new List<string>();
            JoinOtherColumn = new List<string>();
        }

        public string ThisPropertyName { get; set; }
        public List<string> ThisProperties { get; private set; }

        public string OtherPropertyName { get; set; }

        public string JoinTable { get; set; }
        public string JoinSchema { get; set; }
        public List<string> JoinThisColumn { get; private set; }
        public List<string> JoinOtherColumn { get; private set; }
    }
}