using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeSmith.EntityFramework
{
    public class PropertyCollection
        : ObservableCollection<Property>
    {
        public bool IsProcessed { get; set; }

        /// <summary>
        /// 获取主键集合
        /// </summary>
        public IEnumerable<Property> PrimaryKeys
        {
            get { return this.Where(p => p.IsPrimaryKey == true); }
        }

        /// <summary>
        /// 获取外键集合
        /// </summary>
        public IEnumerable<Property> ForeignKeys
        {
            get { return this.Where(p => p.IsForeignKey == true); }
        }

        /// <summary>
        /// 根据数据库字段名获取属性对象
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Property ByColumn(string columnName)
        {
            return this.FirstOrDefault(x => x.ColumnName == columnName);
        }

        /// <summary>
        /// 根据属性类名获取属性对象
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public Property ByProperty(string propertyName)
        {
            return this.FirstOrDefault(x => x.PropertyName == propertyName);
        }
    }
}