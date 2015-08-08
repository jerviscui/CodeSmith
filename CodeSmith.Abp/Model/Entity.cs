using System.Diagnostics;

namespace CodeSmith.Abp.Model
{
    [DebuggerDisplay("Class: {ClassName}, Table: {FullName}, Context: {ContextName}")]
    public class Entity : EntityBase
    {
        public Entity()
        {
            Properties = new PropertyCollection();
            Relationships = new RelationshipCollection();
            Methods = new MethodCollection();
        }

        public string ContextName { get; set; }
        public string ClassName { get; set; }
        public string MappingName { get; set; }

        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string FullName { get; set; }

        public PropertyCollection Properties { get; set; }
        public RelationshipCollection Relationships { get; set; }
        public MethodCollection Methods { get; set; }
    }
}