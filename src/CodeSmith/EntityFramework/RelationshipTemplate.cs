using System.Collections.Generic;
using System.Linq;
using SchemaExplorer;
// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class RelationshipTemplate<TEntity, TProperty> : SingleTemplate<Relationship, TableKeySchema>
        where TEntity : Entity<TProperty>, new()
        where TProperty : Property, new()
    {
        private readonly TemplateContent _content;

        public EntityContext<TEntity, TProperty> EntityContext { get; set; }

        public TEntity ForeignEntity { get; set; }

        public RelationshipTemplate(TemplateContent content)
        {
            _content = content;
        }

        public override Relationship GetAndCreate(TableKeySchema tableKeySchema)
        {
            //获取主键表对应实体
            TEntity primaryEntity = EntityContext.Entities.ByTable(tableKeySchema.PrimaryKeyTable.FullName);
            //主表类名
            string primaryName = primaryEntity.ClassName;
            //外键表名
            string foreignName = ForeignEntity.ClassName;
            //映射名称
            string relationshipName = tableKeySchema.Name;
            //获取唯一映射名称
            relationshipName = _content.UniqueNamer.UniqueRelationshipName(relationshipName);
            //判断是否级联删除
            bool isCascadeDelete = _content.IsCascadeDelete(tableKeySchema);
            bool foreignMembersRequired;
            bool primaryMembersRequired;
            //获取外键表所有键属性名称
            List<string> foreignMembers = _content.GetKeyMembers(ForeignEntity, tableKeySchema.ForeignKeyMemberColumns, tableKeySchema.Name, out foreignMembersRequired);
            //获取主表中所有键的成员属性名称
            List<string> primaryMembers = _content.GetKeyMembers(primaryEntity, tableKeySchema.PrimaryKeyMemberColumns, tableKeySchema.Name, out primaryMembersRequired);
            // 过滤没有外键主键的表处理
            if (foreignMembers == null || primaryMembers == null)
                return null;
            Relationship foreignRelationship = ForeignEntity.Relationships
              .FirstOrDefault(r => r.RelationshipName == relationshipName && r.IsForeignKey);

            if (foreignRelationship == null)
            {
                foreignRelationship = new Relationship { RelationshipName = relationshipName };
                ForeignEntity.Relationships.Add(foreignRelationship);
            }
            foreignRelationship.IsMapped = true;
            foreignRelationship.IsForeignKey = true;
            foreignRelationship.ThisCardinality = foreignMembersRequired ? Cardinality.One : Cardinality.ZeroOrOne;
            foreignRelationship.ThisEntity = foreignName;
            foreignRelationship.ThisProperties = new List<string>(foreignMembers);
            foreignRelationship.OtherEntity = primaryName;
            foreignRelationship.OtherProperties = new List<string>(primaryMembers);
            foreignRelationship.CascadeDelete = isCascadeDelete;

            string prefix = _content.GetMemberPrefix(foreignRelationship, primaryName, foreignName);

            string foreignPropertyName = _content.ToPropertyName(ForeignEntity.ClassName, prefix + primaryName);
            foreignPropertyName = _content.UniqueNamer.UniqueName(ForeignEntity.ClassName, foreignPropertyName);
            foreignRelationship.ThisPropertyName = foreignPropertyName;

            // add reverse
            Relationship primaryRelationship = primaryEntity.Relationships
              .FirstOrDefault(r => r.RelationshipName == relationshipName && r.IsForeignKey == false);

            if (primaryRelationship == null)
            {
                primaryRelationship = new Relationship { RelationshipName = relationshipName };
                primaryEntity.Relationships.Add(primaryRelationship);
            }

            primaryRelationship.IsMapped = false;
            primaryRelationship.IsForeignKey = false;
            primaryRelationship.ThisEntity = primaryName;
            primaryRelationship.ThisProperties = new List<string>(primaryMembers);
            primaryRelationship.OtherEntity = foreignName;
            primaryRelationship.OtherProperties = new List<string>(foreignMembers);
            primaryRelationship.CascadeDelete = isCascadeDelete;

            bool isOneToOne = _content.IsOneToOne(tableKeySchema, foreignRelationship);
            if (isOneToOne)
                primaryRelationship.ThisCardinality = primaryMembersRequired ? Cardinality.One : Cardinality.ZeroOrOne;
            else
                primaryRelationship.ThisCardinality = Cardinality.Many;

            string primaryPropertyName = prefix + foreignName;
            if (!isOneToOne)
                primaryPropertyName = _content.Settings.RelationshipName(primaryPropertyName);

            primaryPropertyName = _content.ToPropertyName(primaryEntity.ClassName, primaryPropertyName);
            primaryPropertyName = _content.UniqueNamer.UniqueName(primaryEntity.ClassName, primaryPropertyName);

            primaryRelationship.ThisPropertyName = primaryPropertyName;

            foreignRelationship.OtherPropertyName = primaryRelationship.ThisPropertyName;
            foreignRelationship.OtherCardinality = primaryRelationship.ThisCardinality;

            primaryRelationship.OtherPropertyName = foreignRelationship.ThisPropertyName;
            primaryRelationship.OtherCardinality = foreignRelationship.ThisCardinality;

            foreignRelationship.IsProcessed = true;
            primaryRelationship.IsProcessed = true;

            return primaryRelationship;
        }
    }
}
