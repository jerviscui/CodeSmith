using System;
using System.Diagnostics;
using SchemaExplorer;

// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class EntityTemplate<TEntity, TProperty, TPropertyTemplate> : ISingleTemplate<TEntity, TableSchema>
        where TPropertyTemplate : PropertyTemplate<TEntity, TProperty>
        where TProperty : Property, new()
        where TEntity : Entity<TProperty>, new()
    {
        private readonly TemplateContent _content;
        private readonly TPropertyTemplate _propertyTemplate;

        public EntityContext<TEntity, TProperty> EntityContext { get; set; }

        public EntityTemplate(TemplateContent content, TPropertyTemplate propertyTemplate)
        {
            _content = content;
            _propertyTemplate = propertyTemplate;
        }

        public virtual TEntity GetAndCreate(TableSchema tableSchema)
        {
            if (_content == null)
                throw new ArgumentNullException("tableSchema");

            TEntity entity = EntityContext.Entities.ByTable(tableSchema.FullName);

            if (entity != null)
                return entity;

            entity = new TEntity
            {
                FullName = tableSchema.FullName,
                TableName = tableSchema.Name,
                TableSchema = tableSchema.Owner
            };

            string className = _content.ToClassName(tableSchema.Name);
            className = _content.UniqueNamer.UniqueClassName(className);

            string mappingName = className + "Map";
            mappingName = _content.UniqueNamer.UniqueClassName(mappingName);

            string contextName = _content.Settings.ContextName(className);
            contextName = _content.ToPropertyName(EntityContext.ClassName, contextName);
            contextName = _content.UniqueNamer.UniqueContextName(contextName);

            entity.ClassName = className;
            entity.ContextName = contextName;
            entity.MappingName = mappingName;

            _propertyTemplate.Entity = entity;

            foreach (ColumnSchema dataObjectBase in tableSchema.Columns.Selected())
            {
                // 过滤掉数据库自定义类型
                if (dataObjectBase.NativeType.Equals("hierarchyid", StringComparison.OrdinalIgnoreCase)
                  || dataObjectBase.NativeType.Equals("sql_variant", StringComparison.OrdinalIgnoreCase))
                {
                    Debug.WriteLine("跳过字段 '{0}'，因为目前不支持自定义类型'{1}'的处理.", dataObjectBase.Name, dataObjectBase.NativeType);
                    continue;
                }
                TProperty property = _propertyTemplate.GetAndCreate(dataObjectBase);
                entity.Properties.Add(property);             
            }
            entity.Properties.IsProcessed = true;
            EntityContext.Entities.Add(entity);
            return entity;
        }
    }
}
