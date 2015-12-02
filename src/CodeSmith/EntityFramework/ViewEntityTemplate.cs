using System;
using SchemaExplorer;

// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class ViewEntityTemplate<TEntity, TProperty> : ISingleTemplate<TEntity, ViewSchema>
        where TEntity : Entity<TProperty>, new()
        where TProperty : Property, new()
    {
        private readonly TemplateContent _content;

        public EntityContext<TEntity, TProperty> EntityContext { get; set; }

        public ViewEntityTemplate(TemplateContent content)
        {
            _content = content;
        }

        public TEntity GetAndCreate(ViewSchema viewSchema)
        {
            if (_content == null)
                throw new ArgumentNullException("viewSchema");

            TEntity entity = EntityContext.Entities.ByTable(viewSchema.FullName);
            if (entity != null)
                return entity;

            entity = new TEntity
            {
                FullName = viewSchema.FullName,
                TableName = viewSchema.Name,
                TableSchema = viewSchema.Owner
            };

            string className = _content.ToClassName(viewSchema.Name);
            className = _content.UniqueNamer.UniqueClassName(className);

            string mappingName = className + "Map";
            mappingName = _content.UniqueNamer.UniqueClassName(mappingName);

            string contextName = _content.Settings.ContextName(className);
            contextName = _content.ToPropertyName(EntityContext.ClassName, contextName);
            contextName = _content.UniqueNamer.UniqueContextName(contextName);

            entity.ClassName = className;
            entity.ContextName = contextName;
            entity.MappingName = mappingName;

            entity.Properties = new PropertyCollection<TProperty>();

            PropertyTemplate<TEntity, TProperty> propertyTemplate = new PropertyTemplate<TEntity,TProperty>(_content)
            {
                Entity = entity
            };

            foreach (ViewColumnSchema dataObjectBase in viewSchema.Columns.Selected())
            {
                propertyTemplate.GetAndCreate(dataObjectBase);
            }

            EntityContext.Entities.Add(entity);

            return entity;
        }
    }
}