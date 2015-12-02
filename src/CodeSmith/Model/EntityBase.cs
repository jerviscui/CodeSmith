using System.Xml.Serialization;

// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    /// <summary>
    /// 是否已经处理
    /// </summary>
    public class EntityBase
    {
        [XmlIgnore]
        public bool IsProcessed { get; set; }
    }
}