using System.Collections.Generic;
using System.Linq;
using SchemaExplorer;

namespace CodeSmith.Abp.Extensions
{
    public static class TableSchemaExtensions
    {
        /// <summary>
        /// 判断<see cref="TableSchema"/>是否有主键
        /// </summary>
        /// <param name="tableSchema"></param>
        /// <returns></returns>
        public static bool IsHavePrimaryKey(this TableSchema tableSchema)
        {
            return tableSchema.PrimaryKey.MemberColumns.FirstOrDefault() != null;
        }

        /// <summary>
        /// 返回<see cref="TableSchema"/>中的主键对象,无主键返回null
        /// </summary>
        /// <param name="tableSchema"></param>
        /// <returns></returns>
        public static ColumnSchema GetPrimaryKey(this TableSchema tableSchema)
        {
            return tableSchema.PrimaryKey.MemberColumns.FirstOrDefault();
        }

      
    }
}
