using CodeSmith.Model;
using SchemaExplorer;

namespace CodeSmith
{
    public class EFEntityTemplate : ISingleTemplate<Entity, TableSchema>
    {

        private readonly EntityContext _entityContext;
        private readonly TemplateContent _content;
        private readonly ISingleTemplate<Property, DataObjectBase> _propertyTemplate;

        public EFEntityTemplate(EntityContext entityContext, TemplateContent content, ISingleTemplate<Property, DataObjectBase> propertyTemplate)
        {
            _entityContext = entityContext;
            _content = content;
            _propertyTemplate = propertyTemplate;
        }

        public Entity Get(TableSchema tableSchema)
        {
            var entity = new Entity
            {
                FullName = tableSchema.FullName,
                TableName = tableSchema.Name,
                TableSchema = tableSchema.Owner
            };

            string className = _content.ToClassName(tableSchema.Name);
            className = _content.UniqueNamer.UniqueClassName(className);

            string mappingName = className + "Map";
            mappingName = _content.UniqueNamer.UniqueClassName(mappingName);

            string contextName = _content.GeneratorSettings.ContextName(className);
            contextName = _content.ToPropertyName(_entityContext.ClassName, contextName);
            contextName = _content.UniqueNamer.UniqueContextName(contextName);

            entity.ClassName = className;
            entity.ContextName = contextName;
            entity.MappingName = mappingName;

            entity.Properties = new PropertyCollection();

            foreach (ColumnSchema dataObjectBase in tableSchema.Columns.Selected())
            {
                entity.Properties.Add(_propertyTemplate.Get(dataObjectBase));
            }
            return entity;
        }
    }
}
