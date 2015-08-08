using System.Diagnostics;

namespace CodeSmith.Abp.Parser
{
    [DebuggerDisplay("Column: {ColumnName}, Property: {PropertyName}")]
    public class ParsedProperty
    {
        public string ColumnName { get; set; }
        public string PropertyName { get; set; }
    }
}