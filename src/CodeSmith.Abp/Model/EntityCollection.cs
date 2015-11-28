using System.Collections.ObjectModel;
using System.Linq;

namespace CodeSmith.Abp.Model
{
    public class EntityCollection
        : ObservableCollection<Entity>
    {
        public bool IsProcessed { get; set; }

        public Entity ByTable(string fullName)
        {
            return this.FirstOrDefault(x => x.FullName == fullName);
        }

        public Entity ByTable(string tableName, string tableSchema)
        {
            return this.FirstOrDefault(x => x.TableName == tableName && x.TableSchema == tableSchema);
        }

        public Entity ByClass(string className)
        {
            return this.FirstOrDefault(x => x.ClassName == className);
        }

        public Entity ByContext(string contextName)
        {
            return this.FirstOrDefault(x => x.ContextName == contextName);
        }
    }
}