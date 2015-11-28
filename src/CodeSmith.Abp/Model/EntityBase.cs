using System.Xml.Serialization;

namespace CodeSmith.Abp.Model
{
    public class EntityBase
    {
        [XmlIgnore]
        public bool IsProcessed { get; set; }
    }
}