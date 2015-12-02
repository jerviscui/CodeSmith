using System.Collections.Generic;
using System.Linq;
using SchemaExplorer;

// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class MethodTemplate<TEntity, TProperty> : SingleTemplate<Method, TableSchema>
        where TEntity : Entity<TProperty>, new()
        where TProperty : Property, new()
    {
        public TEntity Entity { get; set; }

        public override Method GetAndCreate(TableSchema tableSchema)
        {
            if (tableSchema.HasPrimaryKey)
            {
                var method = GetMethodFromColumns(Entity, tableSchema.PrimaryKey.MemberColumns);
                if (method != null)
                {
                    method.IsKey = true;
                    method.SourceName = tableSchema.PrimaryKey.FullName;

                    if (Entity.Methods.All(m => m.NameSuffix != method.NameSuffix))
                        Entity.Methods.Add(method);
                }
            }
            GetIndexMethods(Entity, tableSchema);
            GetForeignKeyMethods(Entity, tableSchema);
            Entity.Methods.IsProcessed = true;
            return null;
        }

        private static void GetForeignKeyMethods(TEntity entity, TableSchema table)
        {
            var columns = new List<ColumnSchema>();

            foreach (ColumnSchema column in table.ForeignKeyColumns)
            {
                columns.Add(column);

                Method method = GetMethodFromColumns(entity, columns);
                if (method != null && entity.Methods.All(m => m.NameSuffix != method.NameSuffix))
                    entity.Methods.Add(method);

                columns.Clear();
            }
        }

        /// <summary>
        /// 获取数据库表中索引，以创建针对索引列扩展查询方法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="table"></param>
        private static void GetIndexMethods(TEntity entity, TableSchema table)
        {
            foreach (IndexSchema index in table.Indexes)
            {
                Method method = GetMethodFromColumns(entity, index.MemberColumns);
                if (method == null)
                    continue;

                method.SourceName = index.FullName;
                method.IsUnique = index.IsUnique;
                method.IsIndex = true;

                if (entity.Methods.All(m => m.NameSuffix != method.NameSuffix))
                    entity.Methods.Add(method);
            }
        }

        /// <summary>
        /// 获取数据库表中外键，以创建针对索引列扩展查询方法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private static Method GetMethodFromColumns(TEntity entity, IEnumerable<ColumnSchema> columns)
        {
            IEnumerable<ColumnSchema> columnSchemata = columns as ColumnSchema[] ?? columns.ToArray();
            if (columnSchemata.Any(c => c.Selected() == false))
                return null;

            var method = new Method();
            string methodName = string.Empty;

            foreach (var column in columnSchemata)
            {
                var property = entity.Properties.ByColumn(column.Name);
                if (property == null)
                    continue;

                method.Properties.Add(property);
                methodName += property.PropertyName;
            }

            if (method.Properties.Count == 0)
                return null;

            method.NameSuffix = methodName;
            return method;
        }
    }
}
