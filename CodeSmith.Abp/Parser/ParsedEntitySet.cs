using System.Diagnostics;

namespace CodeSmith.Abp.Parser
{
    [DebuggerDisplay("Entity: {EntityClass}, Property: {ContextProperty}")]
    public class ParsedEntitySet
    {
        public string EntityClass { get; set; }
        public string ContextProperty { get; set; }
    }
}