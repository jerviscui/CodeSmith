using System.Xml.Serialization;

namespace CodeSmith.EntityFramework.Model
{
    public class EntityBase
    {
        [XmlIgnore]
        public bool IsProcessed { get; set; }
    }
}