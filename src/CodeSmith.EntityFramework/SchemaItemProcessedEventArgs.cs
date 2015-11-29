using System;

namespace CodeSmith.EntityFramework
{
    /// <summary>
    /// 数据库结构处理事件
    /// </summary>
    public class SchemaItemProcessedEventArgs : EventArgs
    {
        /// <summary>
        /// 数据库结构名称
        /// </summary>
        /// <param name="name">表名、视图名</param>
        public SchemaItemProcessedEventArgs(string name)
        {
            _name = name;
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }
    }
}