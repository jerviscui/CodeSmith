using System.Collections.ObjectModel;
using System.Linq;
// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    /// <summary>
    /// 实体集合
    /// </summary>
    public class EntityCollection<TEntity, TProperty>
        : ObservableCollection<TEntity>
        where TEntity : Entity<TProperty>
        where TProperty : Property
    {
        /// <summary>
        /// 是否已经处理
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// 根据表全名获取实体
        /// </summary>
        /// <param name="fullName">表全名,例如:dbo.BSUser。</param>
        /// <returns></returns>
        public TEntity ByTable(string fullName)
        {
            return this.FirstOrDefault(x => x.FullName == fullName);
        }

        /// <summary>
        /// 根据表名或用户名获取实体
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableSchema">例如：dbo</param>
        /// <returns></returns>
        public TEntity ByTable(string tableName, string tableSchema)
        {
            return this.FirstOrDefault(x => x.TableName == tableName && x.TableSchema == tableSchema);
        }

        /// <summary>
        /// 根据类名获取实体
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public TEntity ByClass(string className)
        {
            return this.FirstOrDefault(x => x.ClassName == className);
        }

        /// <summary>
        /// 根据上下文类名获取默认第一个实体
        /// </summary>
        /// <param name="contextName"></param>
        /// <returns></returns>
        public TEntity ByContext(string contextName)
        {
            return this.FirstOrDefault(x => x.ContextName == contextName);
        }
    }
}