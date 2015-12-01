using System.Collections.Generic;
using System.Diagnostics;
using CodeSmith.Settings;

namespace CodeSmith.Model
{
    /// <summary>
    /// 映射对象
    /// </summary>
    [DebuggerDisplay("映射实体: {OtherEntity}, 属性: {OtherPropertyName}, 关系名称: {RelationshipName}")]
    public class Relationship : EntityBase
    {
        public Relationship()
        {
            OtherProperties = new List<string>();
            ThisProperties = new List<string>();
        }

        /// <summary>
        /// 映射名称
        /// </summary>
        public string RelationshipName { get; set; }
        /// <summary>
        /// 主键实体
        /// </summary>
        public string ThisEntity { get; set; }
        /// <summary>
        /// 主键实体属性名
        /// </summary>
        public string ThisPropertyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Cardinality ThisCardinality { get; set; }
        public List<string> ThisProperties { get; set; }

        public string OtherEntity { get; set; }
        public string OtherPropertyName { get; set; }
        public Cardinality OtherCardinality { get; set; }
        public List<string> OtherProperties { get; set; }

        public bool? CascadeDelete { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsMapped { get; set; }

        public bool IsManyToMany
        {
            get
            {
                return ThisCardinality == Cardinality.Many
                       && OtherCardinality == Cardinality.Many;
            }
        }

        public bool IsOneToOne
        {
            get
            {
                return ThisCardinality != Cardinality.Many
                       && OtherCardinality != Cardinality.Many;
            }
        }

        /// <summary>
        /// 连接表
        /// </summary>
        public string JoinTable { get; set; }
        /// <summary>
        /// 连接用户
        /// </summary>
        public string JoinSchema { get; set; }
        /// <summary>
        /// 主键字段名称集合
        /// </summary>
        public List<string> JoinThisColumn { get; set; }
        /// <summary>
        /// 外键字段名称集合
        /// </summary>
        public List<string> JoinOtherColumn { get; set; }

    }
}