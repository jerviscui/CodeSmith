using System.Diagnostics;

namespace CodeSmith.Model
{
    /// <summary>
    /// 实体
    /// </summary>
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
        /// 所在上下文中的名称
        /// </summary>
        public string ContextName { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 映射名
        /// </summary>
        public string MappingName { get; set; }
        /// <summary>
        /// 表的用户名称。例如：Sql Server 中的“dbo”
        /// </summary>
        public string TableSchema { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表全名。包含:用户名，例如:dbo.User
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 所有属性
        /// </summary>
        public PropertyCollection Properties { get; set; }
        /// <summary>
        /// 映射关系
        /// </summary>
        public RelationshipCollection Relationships { get; set; }
        /// <summary>
        /// 方法
        /// </summary>
        public MethodCollection Methods { get; set; }
    }
}