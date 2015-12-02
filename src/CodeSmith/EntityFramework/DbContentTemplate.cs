using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CodeSmith.Engine;
using SchemaExplorer;

// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class DbContentTemplate<TEntity, TProperty, TEntityTemplate, TPropertyTemplate> : ISingleTemplate<EntityContext<TEntity, TProperty>, SchemaSelector>
        where TPropertyTemplate : PropertyTemplate<TEntity, TProperty>
        where TEntityTemplate : EntityTemplate<TEntity, TProperty, TPropertyTemplate>
        where TEntity : Entity<TProperty>, new()
        where TProperty : Property, new()
    {
        private readonly TemplateContent _content;
        private readonly TEntityTemplate _entityTemplate;


        private ViewEntityTemplate<TEntity, TProperty> _viewEntityTemplate;

        /// <summary>
        /// 数据库结构处理事件
        /// </summary>
        public event EventHandler<SchemaItemProcessedEventArgs> SchemaItemProcessed;

        public DbContentTemplate(TemplateContent content, TEntityTemplate entityTemplate)
        {
            _content = content;
            _entityTemplate = entityTemplate;
        }

        public EntityContext<TEntity, TProperty> GetAndCreate(SchemaSelector databaseSchema)
        {
            EntityContext<TEntity, TProperty> entityContext = new EntityContext<TEntity, TProperty>
            {
                DatabaseName = databaseSchema.Database.Name
            };

            string dataContextName = StringUtil.ToPascalCase(entityContext.DatabaseName) + "Context";
            dataContextName = _content.UniqueNamer.UniqueClassName(dataContextName);

            entityContext.ClassName = dataContextName;

            var tables = databaseSchema.Tables
                .Selected()
                .OrderBy(t => t.SortName)
                .ToTables();

            _entityTemplate.EntityContext = entityContext;

            _viewEntityTemplate = new ViewEntityTemplate<TEntity, TProperty>(_content)
            {
                EntityContext = entityContext
            };

            //先进行装载实体
            foreach (TableSchema t in tables)
            {
                if (_content.IsManyToMany(t))
                {
                    Debug.WriteLine("多对多表: " + t.FullName);
                    CreateManyToMany(t);
                }
                else
                {
                    Debug.WriteLine("表: " + t.FullName);
                    _entityTemplate.GetAndCreate(t);
                }

                OnSchemaItemProcessed(t.FullName);
            }

            RelationshipTemplate<TEntity, TProperty> relationshipTemplate = new RelationshipTemplate<TEntity, TProperty>(_content)
            {
                EntityContext = entityContext
            };

            MethodTemplate<TEntity, TProperty> methodTemplate = new MethodTemplate<TEntity, TProperty>();

            foreach (TEntity entity in entityContext.Entities)
            {
                TableSchema tableSchema = databaseSchema.Tables.FirstOrDefault(t => t.FullName == entity.FullName);
                if (tableSchema == null) continue;

                relationshipTemplate.ForeignEntity = entity;
                foreach (TableKeySchema tableKey in tableSchema.ForeignKeys.Selected())
                {
                    relationshipTemplate.GetAndCreate(tableKey);
                }

                methodTemplate.Entity = entity;
                methodTemplate.GetAndCreate(tableSchema);
            }

            if (!_content.Settings.IncludeViews) return entityContext;
            // 数据库视图生成EF实体
            var views = databaseSchema.Views
                .Selected()
                .OrderBy(t => t.SortName)
                .ToViews();
            foreach (ViewSchema v in views)
            {
                Debug.WriteLine("视图: " + v.FullName);
                _viewEntityTemplate.GetAndCreate(v);
                OnSchemaItemProcessed(v.FullName);
            }

            return entityContext;
        }

        protected void OnSchemaItemProcessed(string name)
        {
            var handler = SchemaItemProcessed;
            if (handler == null)
                return;

            handler(this, new SchemaItemProcessedEventArgs(name));
        }

        /// <summary>
        /// 获取多对多的映射关系
        /// </summary>
        /// <param name="joinTable"></param>
        private void CreateManyToMany(TableSchema joinTable)
        {
            if (joinTable.ForeignKeys.Count != 2)
                return;

            var joinTableName = joinTable.Name;
            var joinSchemaName = joinTable.Owner;

            // first fkey is always left, second fkey is right
            var leftForeignKey = joinTable.ForeignKeys[0];
            var leftTable = leftForeignKey.PrimaryKeyTable;
            var joinLeftColumn = leftForeignKey.ForeignKeyMemberColumns.Select(c => c.Name).ToList();

            var leftEntity = _entityTemplate.GetAndCreate(leftTable);

            var rightForeignKey = joinTable.ForeignKeys[1];
            var rightTable = rightForeignKey.PrimaryKeyTable;

            var joinRightColumn = rightForeignKey.ForeignKeyMemberColumns.Select(c => c.Name).ToList();

            var rightEntity = _entityTemplate.GetAndCreate(rightTable);

            string leftPropertyName = _content.Settings.RelationshipName(rightEntity.ClassName);
            leftPropertyName = _content.UniqueNamer.UniqueName(leftEntity.ClassName, leftPropertyName);

            string rightPropertyName = _content.Settings.RelationshipName(leftEntity.ClassName);
            rightPropertyName = _content.UniqueNamer.UniqueName(rightEntity.ClassName, rightPropertyName);

            string relationshipName = string.Format("{0}|{1}",
              leftForeignKey.Name,
              rightForeignKey.Name);

            relationshipName = _content.UniqueNamer.UniqueRelationshipName(relationshipName);

            var left = new Relationship
            {
                RelationshipName = relationshipName,
                IsForeignKey = false,
                IsMapped = true,
                ThisCardinality = Cardinality.Many,
                ThisEntity = leftEntity.ClassName,
                ThisPropertyName = leftPropertyName,
                OtherCardinality = Cardinality.Many,
                OtherEntity = rightEntity.ClassName,
                OtherPropertyName = rightPropertyName,
                JoinTable = joinTableName,
                JoinSchema = joinSchemaName,
                JoinThisColumn = new List<string>(joinLeftColumn),
                JoinOtherColumn = new List<string>(joinRightColumn)
            };

            leftEntity.Relationships.Add(left);

            var right = new Relationship
            {
                RelationshipName = relationshipName,
                IsForeignKey = false,
                IsMapped = false,
                ThisCardinality = Cardinality.Many,
                ThisEntity = rightEntity.ClassName,
                ThisPropertyName = rightPropertyName,
                OtherCardinality = Cardinality.Many,
                OtherEntity = leftEntity.ClassName,
                OtherPropertyName = leftPropertyName,
                JoinTable = joinTableName,
                JoinSchema = joinSchemaName,
                JoinThisColumn = new List<string>(joinRightColumn),
                JoinOtherColumn = new List<string>(joinLeftColumn)
            };

            rightEntity.Relationships.Add(right);
        }
    }
}
