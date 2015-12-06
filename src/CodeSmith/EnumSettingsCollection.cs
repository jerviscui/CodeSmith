using System.Collections.Generic;
using System.Linq;
using SchemaExplorer;

namespace CodeSmith
{
    public class EnumSettingsCollection : List<EnumSettings>
    {
        public EnumSettings GetSettings(DataObjectBase dataObjectBase)
        {
            return this.FirstOrDefault(t => t.FullName == dataObjectBase.FullName);
        }
    }
}