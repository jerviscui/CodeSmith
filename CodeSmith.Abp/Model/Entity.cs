using System.Diagnostics;
using CodeSmith.Abp.Extensions;
using SchemaExplorer;

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

        /// <summary>
        ///IDset����
        /// </summary>
        public string ContextName { get; set; }

        /// <summary>
        ///���� 
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MappingName { get; set; }

        public string TableSchema { get; set; }

        public string TableName { get; set; }

        public string FullName { get; set; }


        public TableSchema TableSchemaObject { get; set; }

        public string Describe
        {
            get
            {
                if (TableSchemaObject == null) return "";
                return TableSchemaObject.Description.IsNull()
                    ? TableSchema : TableSchemaObject.Description;
            }
        }

        /// <summary>
        /// ���� 
        /// </summary>
        public PropertyCollection Properties { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public RelationshipCollection Relationships { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public MethodCollection Methods { get; set; }
    }
}