using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSmith.Model;
using CodeSmith.Settings;
using SchemaExplorer;

namespace CodeSmith
{
    public class EFRelationshipTemplate : SingleTemplate<Relationship, TableKeySchema>
    {
        private readonly TemplateContent _content;

        private readonly Entity _foreignEntity;

        public EFRelationshipTemplate(TemplateContent content, Entity foreignEntity)
        {
            _content = content;
            this._foreignEntity = foreignEntity;
        }

        public override Relationship Get(TableKeySchema tableKeySchema)
        {
            //获取主键表对应实体
            Entity primaryEntity = GetEntity(entityContext, tableKeySchema.PrimaryKeyTable, false, false);
            //主表类名
            string primaryName = primaryEntity.ClassName;
            //外键表名
            string foreignName = _foreignEntity.ClassName;
            //映射名称
            string relationshipName = tableKeySchema.Name;
            //获取唯一映射名称
            relationshipName = _content.UniqueNamer.UniqueRelationshipName(relationshipName);
            //判断是否级联删除
            bool isCascadeDelete = _content.IsCascadeDelete(tableKeySchema);
            bool foreignMembersRequired;
            bool primaryMembersRequired;
            //获取外键表所有键属性名称
            List<string> foreignMembers = _content.GetKeyMembers(_foreignEntity, tableKeySchema.ForeignKeyMemberColumns, tableKeySchema.Name, out foreignMembersRequired);
            //获取主表中所有键的成员属性名称
            List<string> primaryMembers = _content.GetKeyMembers(primaryEntity, tableKeySchema.PrimaryKeyMemberColumns, tableKeySchema.Name, out primaryMembersRequired);
            // 过滤没有外键主键的表处理
            if (foreignMembers == null || primaryMembers == null)
                return;
            Relationship foreignRelationship = _foreignEntity.Relationships
              .FirstOrDefault(r => r.RelationshipName == relationshipName && r.IsForeignKey);

            if (foreignRelationship == null)
            {
                foreignRelationship = new Relationship { RelationshipName = relationshipName };
                _foreignEntity.Relationships.Add(foreignRelationship);
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

            string foreignPropertyName = _content.ToPropertyName(_foreignEntity.ClassName, prefix + primaryName);
            foreignPropertyName = _content.UniqueNamer.UniqueName(_foreignEntity.ClassName, foreignPropertyName);
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
                primaryPropertyName = _content.GeneratorSettings.RelationshipName(primaryPropertyName);

            primaryPropertyName = _content.ToPropertyName(primaryEntity.ClassName, primaryPropertyName);
            primaryPropertyName = _content.UniqueNamer.UniqueName(primaryEntity.ClassName, primaryPropertyName);

            primaryRelationship.ThisPropertyName = primaryPropertyName;

            foreignRelationship.OtherPropertyName = primaryRelationship.ThisPropertyName;
            foreignRelationship.OtherCardinality = primaryRelationship.ThisCardinality;

            primaryRelationship.OtherPropertyName = foreignRelationship.ThisPropertyName;
            primaryRelationship.OtherCardinality = foreignRelationship.ThisCardinality;

            foreignRelationship.IsProcessed = true;
            primaryRelationship.IsProcessed = true;
        }
    }
}
