using System.Diagnostics;

namespace CodeSmith.EntityFramework.Model
{
    [DebuggerDisplay("Context: {ContextName}, Database: {DatabaseName}")]
    public class EntityContext : EntityBase
    {
        public EntityContext()
        {
            Entities = new EntityCollection();
        }

        public string ClassName { get; set; }
        public string DatabaseName { get; set; }

        public EntityCollection Entities { get; set; }

        public void RenameEntity(string originalName, string newName)
        {
            if (originalName == newName)
                return;

            Debug.WriteLine("Rename Entity '{0}' to '{1}'.", originalName, newName);
            foreach (var entity in Entities)
            {
                if (entity.ClassName == originalName)
                    entity.ClassName = newName;

                foreach (var relationship in entity.Relationships)
                {
                    if (relationship.ThisEntity == originalName)
                        relationship.ThisEntity = newName;
                    if (relationship.OtherEntity == originalName)
                        relationship.OtherEntity = newName;
                }
            }
        }

        public void RenameProperty(string entityName, string originalName, string newName)
        {
            if (originalName == newName)
                return;

            Debug.WriteLine("Rename Property '{0}' to '{1}' in Entity '{2}'.", originalName, newName, entityName);
            foreach (var entity in Entities)
            {
                if (entity.ClassName == entityName)
                {
                    var property = entity.Properties.ByProperty(originalName);
                    if (property != null)
                        property.PropertyName = newName;
                }

                foreach (var relationship in entity.Relationships)
                {
                    if (relationship.ThisEntity == entityName)
                        for (int i = 0; i < relationship.ThisProperties.Count; i++)
                            if (relationship.ThisProperties[i] == originalName)
                                relationship.ThisProperties[i] = newName;

                    if (relationship.OtherEntity == entityName)
                        for (int i = 0; i < relationship.OtherProperties.Count; i++)
                            if (relationship.OtherProperties[i] == originalName)
                                relationship.OtherProperties[i] = newName;
                }
            }

        }
    }
}