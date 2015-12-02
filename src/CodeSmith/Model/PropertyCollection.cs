using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class PropertyCollection<TProperty>
        : ObservableCollection<TProperty> where TProperty : Property
    {
        public bool IsProcessed { get; set; }

        /// <summary>
        /// 获取主键集合
        /// </summary>
        public IEnumerable<TProperty> PrimaryKeys
        {
            get { return this.Where(p => p.IsPrimaryKey == true); }
        }

        /// <summary>
        /// 获取外键集合
        /// </summary>
        public IEnumerable<TProperty> ForeignKeys
        {
            get { return this.Where(p => p.IsForeignKey == true); }
        }

        /// <summary>
        /// 根据数据库字段名获取属性对象
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public TProperty ByColumn(string columnName)
        {
            return this.FirstOrDefault(x => x.ColumnName == columnName);
        }

        /// <summary>
        /// 根据属性类名获取属性对象
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public TProperty ByProperty(string propertyName)
        {
            return this.FirstOrDefault(x => x.PropertyName == propertyName);
        }
    }
}