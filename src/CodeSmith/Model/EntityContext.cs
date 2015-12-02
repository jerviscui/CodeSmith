using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    /// <summary>
    /// 上下文对象
    /// </summary>
    [DebuggerDisplay("上下文对象: {ContextName}, 数据库: {DatabaseName}")]
    public class EntityContext<TEntity, TProperty> : EntityBase
        where TProperty : Property
        where TEntity : Entity<TProperty>
    {
        public EntityContext()
        {
            Entities = new EntityCollection<TEntity, TProperty>();
        }

        /// <summary>
        /// 上下文类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 实体集合
        /// </summary>
        public EntityCollection<TEntity, TProperty> Entities { get; set; }

        /// <summary>
        /// 重新命名实体
        /// </summary>
        /// <param name="originalName"></param>
        /// <param name="newName"></param>
        public void RenameEntity(string originalName, string newName)
        {
            if (originalName == newName)
                return;

            Debug.WriteLine("重新命名实体 '{0}' 为 '{1}'.", originalName, newName);

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

        /// <summary>
        /// 重新命名属性名
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="originalName"></param>
        /// <param name="newName"></param>
        public void RenameProperty(string entityName, string originalName, string newName)
        {
            if (originalName == newName)
                return;

            Debug.WriteLine("将 '{2}' 实体中 '{0}' 属性，重新命名为 '{1}'.", originalName, newName, entityName);
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