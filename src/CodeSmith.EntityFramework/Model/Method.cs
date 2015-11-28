using System.Collections.Generic;
using System.Diagnostics;

namespace CodeSmith.EntityFramework.Model
{
    [DebuggerDisplay("Suffix: {NameSuffix}, IsKey: {IsKey}, IsUnique: {IsUnique}")]
    public class Method : EntityBase
    {
        public Method()
        {
            Properties = new List<Property>();
        }

        public string NameSuffix { get; set; }
        public string SourceName { get; set; }

        public bool IsKey { get; set; }
        public bool IsUnique { get; set; }
        public bool IsIndex { get; set; }

        public List<Property> Properties { get; set; }
    }
}