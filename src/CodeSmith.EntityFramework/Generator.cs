using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using CodeSmith.Engine;
using SchemaExplorer;

namespace CodeSmith.EntityFramework
{
    /// <summary>
    ///代码生成处理类
    /// </summary>
    public class Generator
    {
        private readonly UniqueNamer _namer;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Generator()
        {
            _settings = new GeneratorSettings();
            _namer = new UniqueNamer();
        }

        /// <summary>
        /// 数据库结构处理事件
        /// </summary>
        public event EventHandler<SchemaItemProcessedEventArgs> SchemaItemProcessed;

        protected void OnSchemaItemProcessed(string name)
        {
            var handler = SchemaItemProcessed;
            if (handler == null)
                return;

            handler(this, new SchemaItemProcessedEventArgs(name));
        }

        private readonly GeneratorSettings _settings;

        /// <summary>
        /// 代码生成配置
        /// </summary>
        public GeneratorSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// 根据数据库结构获取实体上下文对象
        /// </summary>
        /// <param name="databaseSchema">数据库结构</param>
        /// <returns></returns>
        public EntityContext Generate(SchemaSelector databaseSchema)
        {
            EntityContext entityContext = new EntityContext { DatabaseName = databaseSchema.Database.Name };

            string dataContextName = StringUtil.ToPascalCase(entityContext.DatabaseName) + "Context";
            dataContextName = _namer.UniqueClassName(dataContextName);

            entityContext.ClassName = dataContextName;

            var tables = databaseSchema.Tables
                .Selected()
                .OrderBy(t => t.SortName)
                .ToTables();

            foreach (TableSchema t in tables)
            {
                if (IsManyToMany(t))
                {
                    Debug.WriteLine("多对多表: " + t.FullName);
                    CreateManyToMany(entityContext, t);
                }
                else
                {
                    Debug.WriteLine("表: " + t.FullName);
                    GetEntity(entityContext, t);
                }

                OnSchemaItemProcessed(t.FullName);
            }


            if (!Settings.IncludeViews) return entityContext;
            // 数据库视图生成EF实体
            var views = databaseSchema.Views
                .Selected()
                .OrderBy(t => t.SortName)
                .ToViews();
            foreach (ViewSchema v in views)
            {
                Debug.WriteLine("视图: " + v.FullName);
                GetEntity(entityContext, v);
                OnSchemaItemProcessed(v.FullName);
            }

            return entityContext;
        }

        /// <summary>
        /// 获取数据库表对应实体对象
        /// </summary>
        /// <param name="entityContext">数据库上下文处理对象</param>
        /// <param name="tableSchema">表结构对象</param>
        /// <param name="processRelationships">是否处理实体映射关系</param>
        /// <param name="processMethods">是否处理实体扩展查询方法</param>
        /// <returns></returns>
        private Entity GetEntity(EntityContext entityContext, TableSchema tableSchema, bool processRelationships = true, bool processMethods = true)
        {
            string key = tableSchema.FullName;

            Entity entity = entityContext.Entities.ByTable(key)
              ?? CreateEntity(entityContext, tableSchema);

            if (!entity.Properties.IsProcessed)
                CreateProperties(entity, tableSchema.Columns.Selected());

            if (processRelationships && !entity.Relationships.IsProcessed)
                CreateRelationships(entityContext, entity, tableSchema);

            if (processMethods && !entity.Methods.IsProcessed)
                CreateMethods(entity, tableSchema);

            entity.IsProcessed = true;
            return entity;
        }

        /// <summary>
        /// 获取数据库视图对应实体对象
        /// </summary>
        /// <param name="entityContext"></param>
        /// <param name="viewSchema"></param>
        /// <returns></returns>
        private void GetEntity(EntityContext entityContext, ViewSchema viewSchema)
        {
            string key = viewSchema.FullName;

            Entity entity = entityContext.Entities.ByTable(key)
              ?? CreateEntity(entityContext, viewSchema);

            if (!entity.Properties.IsProcessed)
                CreateProperties(entity, viewSchema.Columns);

            entity.IsProcessed = true;
        }

        /// <summary>
        /// 创建数据库表或者视图对应的实体
        /// </summary>
        /// <param name="entityContext"></param>
        /// <param name="tableSchema"></param>
        /// <returns></returns>
        private Entity CreateEntity(EntityContext entityContext, TabularObjectBase tableSchema)
        {
            var entity = new Entity
            {
                FullName = tableSchema.FullName,
                TableName = tableSchema.Name,
                TableSchema = tableSchema.Owner
            };

            string className = ToClassName(tableSchema.Name);
            className = _namer.UniqueClassName(className);

            string mappingName = className + "Map";
            mappingName = _namer.UniqueClassName(mappingName);

            string contextName = Settings.ContextName(className);
            contextName = ToPropertyName(entityContext.ClassName, contextName);
            contextName = _namer.UniqueContextName(contextName);

            entity.ClassName = className;
            entity.ContextName = contextName;
            entity.MappingName = mappingName;

            entityContext.Entities.Add(entity);

            return entity;
        }

        /// <summary>
        /// 创建数据库表字段或者视图字段对应实体属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="columnSchemaCollection">字段集合</param>
        private void CreateProperties(Entity entity, IEnumerable<DataObjectBase> columnSchemaCollection)
        {
            foreach (DataObjectBase dataObjectBase in columnSchemaCollection)
            {
                // 过滤掉数据库自定义类型
                if (dataObjectBase.NativeType.Equals("hierarchyid", StringComparison.OrdinalIgnoreCase)
                  || dataObjectBase.NativeType.Equals("sql_variant", StringComparison.OrdinalIgnoreCase))
                {
                    Debug.WriteLine("跳过字段 '{0}'，因为目前不支持自定义类型'{1}'的处理.", dataObjectBase.Name, dataObjectBase.NativeType);
                    continue;
                }

                Property property = entity.Properties.ByColumn(dataObjectBase.Name);

                if (property == null)
                {
                    property = new Property { ColumnName = dataObjectBase.Name };
                    entity.Properties.Add(property);
                }

                string propertyName = ToPropertyName(entity.ClassName, dataObjectBase.Name);
                propertyName = _namer.UniqueName(entity.ClassName, propertyName);

                property.PropertyName = propertyName;

                property.DataType = dataObjectBase.DataType;
                property.SystemType = dataObjectBase.SystemType;
                property.NativeType = dataObjectBase.NativeType;

                property.IsNullable = dataObjectBase.AllowDBNull;

                property.IsIdentity = IsIdentity(dataObjectBase);
                property.IsRowVersion = IsRowVersion(dataObjectBase);
                property.IsAutoGenerated = IsDbGenerated(dataObjectBase);

                property.Default = GetDefaultValue(dataObjectBase);

                property.Explain = dataObjectBase.Description ?? dataObjectBase.FullName;

                if (property.SystemType == typeof(string)
                  || property.SystemType == typeof(byte[]))
                {
                    property.MaxLength = dataObjectBase.Size;
                }

                if (property.SystemType == typeof(float)
                  || property.SystemType == typeof(double)
                  || property.SystemType == typeof(decimal))
                {
                    property.Precision = dataObjectBase.Precision;
                    property.Scale = dataObjectBase.Scale;
                }

                var columnSchema = dataObjectBase as ColumnSchema;
                if (columnSchema != null)
                {
                    property.IsPrimaryKey = columnSchema.IsPrimaryKeyMember;
                    property.IsForeignKey = columnSchema.IsForeignKeyMember;
                    if (columnSchema.IsUnique)
                        property.IsUnique = true;
                }

                property.IsProcessed = true;
            }

            entity.Properties.IsProcessed = true;
        }

        /// <summary>
        /// 获取数据库表中外键字段对应EF映射
        /// </summary>
        /// <param name="entityContext"></param>
        /// <param name="entity"></param>
        /// <param name="tableSchema"></param>
        private void CreateRelationships(EntityContext entityContext, Entity entity, TableSchema tableSchema)
        {
            foreach (TableKeySchema tableKey in tableSchema.ForeignKeys.Selected())
            {
                CreateRelationship(entityContext, entity, tableKey);
            }

            entity.Relationships.IsProcessed = true;
        }

        /// <summary>
        /// 创建数据库关系对应EF的Mapper
        /// </summary>
        /// <param name="entityContext">数据库上下文</param>
        /// <param name="foreignEntity">外键表对应实体</param>
        /// <param name="tableKeySchema">主键表</param>
        private void CreateRelationship(EntityContext entityContext, Entity foreignEntity, TableKeySchema tableKeySchema)
        {
            //获取主键表对应实体
            Entity primaryEntity = GetEntity(entityContext, tableKeySchema.PrimaryKeyTable, false, false);
            //主表类名
            string primaryName = primaryEntity.ClassName;
            //外键表名
            string foreignName = foreignEntity.ClassName;
            //映射名称
            string relationshipName = tableKeySchema.Name;
            //获取唯一映射名称
            relationshipName = _namer.UniqueRelationshipName(relationshipName);
            //判断是否级联删除
            bool isCascadeDelete = IsCascadeDelete(tableKeySchema);
            bool foreignMembersRequired;
            bool primaryMembersRequired;
            //获取外键表所有键属性名称
            List<string> foreignMembers = GetKeyMembers(foreignEntity, tableKeySchema.ForeignKeyMemberColumns, tableKeySchema.Name, out foreignMembersRequired);
            //获取主表中所有键的成员属性名称
            List<string> primaryMembers = GetKeyMembers(primaryEntity, tableKeySchema.PrimaryKeyMemberColumns, tableKeySchema.Name, out primaryMembersRequired);
            // 过滤没有外键主键的表处理
            if (foreignMembers == null || primaryMembers == null)
                return;
            Relationship foreignRelationship = foreignEntity.Relationships
              .FirstOrDefault(r => r.RelationshipName == relationshipName && r.IsForeignKey);

            if (foreignRelationship == null)
            {
                foreignRelationship = new Relationship { RelationshipName = relationshipName };
                foreignEntity.Relationships.Add(foreignRelationship);
            }
            foreignRelationship.IsMapped = true;
            foreignRelationship.IsForeignKey = true;
            foreignRelationship.ThisCardinality = foreignMembersRequired ? Cardinality.One : Cardinality.ZeroOrOne;
            foreignRelationship.ThisEntity = foreignName;
            foreignRelationship.ThisProperties = new List<string>(foreignMembers);
            foreignRelationship.OtherEntity = primaryName;
            foreignRelationship.OtherProperties = new List<string>(primaryMembers);
            foreignRelationship.CascadeDelete = isCascadeDelete;

            string prefix = GetMemberPrefix(foreignRelationship, primaryName, foreignName);

            string foreignPropertyName = ToPropertyName(foreignEntity.ClassName, prefix + primaryName);
            foreignPropertyName = _namer.UniqueName(foreignEntity.ClassName, foreignPropertyName);
            foreignRelationship.ThisPropertyName = foreignPropertyName;

            // add reverse
            Relationship primaryRelationship = primaryEntity.Relationships
              .FirstOrDefault(r => r.RelationshipName == relationshipName && r.IsForeignKey == false);

            if (primaryRelationship == null)
            {
                primaryRelationship = new Relationship { RelationshipName = relationshipName };
                primaryEntity.Relationships.Add(primaryRelationship);
            }

            primaryRelationship.IsMapped = false;
            primaryRelationship.IsForeignKey = false;
            primaryRelationship.ThisEntity = primaryName;
            primaryRelationship.ThisProperties = new List<string>(primaryMembers);
            primaryRelationship.OtherEntity = foreignName;
            primaryRelationship.OtherProperties = new List<string>(foreignMembers);
            primaryRelationship.CascadeDelete = isCascadeDelete;

            bool isOneToOne = IsOneToOne(tableKeySchema, foreignRelationship);
            if (isOneToOne)
                primaryRelationship.ThisCardinality = primaryMembersRequired ? Cardinality.One : Cardinality.ZeroOrOne;
            else
                primaryRelationship.ThisCardinality = Cardinality.Many;

            string primaryPropertyName = prefix + foreignName;
            if (!isOneToOne)
                primaryPropertyName = Settings.RelationshipName(primaryPropertyName);

            primaryPropertyName = ToPropertyName(primaryEntity.ClassName, primaryPropertyName);
            primaryPropertyName = _namer.UniqueName(primaryEntity.ClassName, primaryPropertyName);

            primaryRelationship.ThisPropertyName = primaryPropertyName;

            foreignRelationship.OtherPropertyName = primaryRelationship.ThisPropertyName;
            foreignRelationship.OtherCardinality = primaryRelationship.ThisCardinality;

            primaryRelationship.OtherPropertyName = foreignRelationship.ThisPropertyName;
            primaryRelationship.OtherCardinality = foreignRelationship.ThisCardinality;

            foreignRelationship.IsProcessed = true;
            primaryRelationship.IsProcessed = true;
        }

        /// <summary>
        /// 获取多对多的映射关系
        /// </summary>
        /// <param name="entityContext"></param>
        /// <param name="joinTable"></param>
        private void CreateManyToMany(EntityContext entityContext, TableSchema joinTable)
        {
            if (joinTable.ForeignKeys.Count != 2)
                return;

            var joinTableName = joinTable.Name;
            var joinSchemaName = joinTable.Owner;

            // first fkey is always left, second fkey is right
            var leftForeignKey = joinTable.ForeignKeys[0];
            var leftTable = leftForeignKey.PrimaryKeyTable;
            var joinLeftColumn = leftForeignKey.ForeignKeyMemberColumns.Select(c => c.Name).ToList();
            var leftEntity = GetEntity(entityContext, leftTable, false, false);

            var rightForeignKey = joinTable.ForeignKeys[1];
            var rightTable = rightForeignKey.PrimaryKeyTable;
            var joinRightColumn = rightForeignKey.ForeignKeyMemberColumns.Select(c => c.Name).ToList();
            var rightEntity = GetEntity(entityContext, rightTable, false, false);

            string leftPropertyName = Settings.RelationshipName(rightEntity.ClassName);
            leftPropertyName = _namer.UniqueName(leftEntity.ClassName, leftPropertyName);

            string rightPropertyName = Settings.RelationshipName(leftEntity.ClassName);
            rightPropertyName = _namer.UniqueName(rightEntity.ClassName, rightPropertyName);

            string relationshipName = string.Format("{0}|{1}",
              leftForeignKey.Name,
              rightForeignKey.Name);

            relationshipName = _namer.UniqueRelationshipName(relationshipName);

            var left = new Relationship
            {
                RelationshipName = relationshipName,
                IsForeignKey = false,
                IsMapped = true,
                ThisCardinality = Cardinality.Many,
                ThisEntity = leftEntity.ClassName,
                ThisPropertyName = leftPropertyName,
                OtherCardinality = Cardinality.Many,
                OtherEntity = rightEntity.ClassName,
                OtherPropertyName = rightPropertyName,
                JoinTable = joinTableName,
                JoinSchema = joinSchemaName,
                JoinThisColumn = new List<string>(joinLeftColumn),
                JoinOtherColumn = new List<string>(joinRightColumn)
            };

            leftEntity.Relationships.Add(left);

            var right = new Relationship
            {
                RelationshipName = relationshipName,
                IsForeignKey = false,
                IsMapped = false,
                ThisCardinality = Cardinality.Many,
                ThisEntity = rightEntity.ClassName,
                ThisPropertyName = rightPropertyName,
                OtherCardinality = Cardinality.Many,
                OtherEntity = leftEntity.ClassName,
                OtherPropertyName = leftPropertyName,
                JoinTable = joinTableName,
                JoinSchema = joinSchemaName,
                JoinThisColumn = new List<string>(joinRightColumn),
                JoinOtherColumn = new List<string>(joinLeftColumn)
            };

            rightEntity.Relationships.Add(right);
        }

        /// <summary>
        /// 创建数据库表中外键与主键对应扩展查询方法
        /// </summary>
        /// <param name="entity">EF实体</param>
        /// <param name="tableSchema">数据库表</param>
        private void CreateMethods(Entity entity, TableSchema tableSchema)
        {
            if (tableSchema.HasPrimaryKey)
            {
                var method = GetMethodFromColumns(entity, tableSchema.PrimaryKey.MemberColumns);
                if (method != null)
                {
                    method.IsKey = true;
                    method.SourceName = tableSchema.PrimaryKey.FullName;

                    if (entity.Methods.All(m => m.NameSuffix != method.NameSuffix))
                        entity.Methods.Add(method);
                }
            }

            GetIndexMethods(entity, tableSchema);
            GetForeignKeyMethods(entity, tableSchema);

            entity.Methods.IsProcessed = true;
        }

        private static void GetForeignKeyMethods(Entity entity, TableSchema table)
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
        private static void GetIndexMethods(Entity entity, TableSchema table)
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
        private static Method GetMethodFromColumns(Entity entity, IEnumerable<ColumnSchema> columns)
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

        /// <summary>
        /// 获取数据库表中外键与主键所有的属性名称
        /// </summary>
        /// <param name="entity">外键表或主表实体</param>
        /// <param name="members">主键字段或者外键字段</param>
        /// <param name="name">主表表名</param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        private static List<string> GetKeyMembers(Entity entity, IEnumerable<MemberColumnSchema> members, string name, out bool isRequired)
        {
            var keyMembers = new List<string>();
            isRequired = false;

            foreach (var member in members)
            {
                var property = entity.Properties.ByColumn(member.Name);

                if (property == null)
                {
                    Debug.WriteLine("在'{1}'表没有发现'{0}'字段的关系.", member.Name, name);
                    return null;
                }

                if (!isRequired)
                    isRequired = property.IsRequired;

                keyMembers.Add(property.PropertyName);
            }

            return keyMembers;
        }

        private static string GetMemberPrefix(Relationship relationship, string primaryClass, string foreignClass)
        {
            string thisKey = relationship.ThisProperties.FirstOrDefault() ?? string.Empty;
            string otherKey = relationship.OtherProperties.FirstOrDefault() ?? string.Empty;

            bool isSameName = thisKey.Equals(otherKey, StringComparison.OrdinalIgnoreCase);
            isSameName = (isSameName || thisKey.Equals(primaryClass + otherKey, StringComparison.OrdinalIgnoreCase));

            string prefix = string.Empty;
            if (isSameName)
                return prefix;

            prefix = thisKey.Replace(otherKey, "");
            prefix = prefix.Replace(primaryClass, "");
            prefix = prefix.Replace(foreignClass, "");
            prefix = Regex.Replace(prefix, @"(_ID|_id|_Id|\.ID|\.id|\.Id|ID|Id)$", "");
            prefix = Regex.Replace(prefix, @"^\d", "");

            return prefix;
        }

        /// <summary>
        /// 判断一对一的关系
        /// </summary>
        /// <param name="tableKeySchema"></param>
        /// <param name="foreignRelationship"></param>
        /// <returns></returns>
        private static bool IsOneToOne(TableKeySchema tableKeySchema, Relationship foreignRelationship)
        {
            bool isFkeyPkey = tableKeySchema.ForeignKeyTable.HasPrimaryKey
                              && tableKeySchema.ForeignKeyTable.PrimaryKey != null
                              && tableKeySchema.ForeignKeyTable.PrimaryKey.MemberColumns.Count == 1
                              && tableKeySchema.ForeignKeyTable.PrimaryKey.MemberColumns.Contains(
                                foreignRelationship.ThisProperties.FirstOrDefault());

            if (isFkeyPkey)
                return true;

            return false;

            // if f.key is unique
            //return tableKeySchema.ForeignKeyMemberColumns.All(column => column.IsUnique);
        }

        /// <summary>
        /// 判断是否只有2个外键的表
        /// </summary>
        /// <param name="tableSchema"></param>
        /// <returns></returns>
        private static bool IsManyToMany(TableSchema tableSchema)
        {
            // 1) 表必须有两个 ForeignKeys。
            // 2) 所有成员必须满足以下条件
            //    a) 成员为外键.
            //    b) 数据生成

            if (tableSchema.Columns.Count < 2)
                return false;

            if (tableSchema.ForeignKeyColumns.Count != 2)
                return false;

            // 所有列都是外键
            if (tableSchema.Columns.Count == 2 &&
              tableSchema.ForeignKeyColumns.Count == 2)
                return true;

            // 数据库表字段包含 自动生成列或者有默认值 的列
            return tableSchema.NonForeignKeyColumns.All(c =>
                IsDbGenerated(c) || HasDefaultValue(c));
        }

        #region 数据库表与字段对应EF相关类名称的处理

        /// <summary>
        /// 获取数据表对应实体名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string ToClassName(string name)
        {
            name = Settings.EntityName(name);
            string legalName = ToLegalName(name);

            return legalName;
        }

        /// <summary>
        /// 获取数据表字段对应实体属性名称
        /// </summary>
        /// <param name="className"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string ToPropertyName(string className, string name)
        {
            string propertyName = ToLegalName(name);
            if (className.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                propertyName += "Member";

            return propertyName;
        }

        /// <summary>
        /// 获取数据库字段名，进行字段名称匹配，再进行帕斯卡方式命名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string ToLegalName(string name)
        {
            string legalName = Settings.CleanName(name);
            legalName = StringUtil.ToPascalCase(legalName);

            return legalName;
        }
        #endregion

        #region 获取数据库字段列的相关属性

        /// <summary>
        /// 数据库字段是否级联删除
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static bool IsCascadeDelete(SchemaObjectBase column)
        {
            bool cascadeDelete = false;
            try
            {
                if (column.ExtendedProperties.Contains(ExtendedPropertyNames.CascadeDelete))
                {
                    string value = column.ExtendedProperties[ExtendedPropertyNames.CascadeDelete].Value.ToString();
                    bool.TryParse(value, out cascadeDelete);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("异常: " + ex.Message);
            }

            return cascadeDelete;
        }

        /// <summary>
        /// 判断数据库是否为 timestamp 或者 rowversion 类型
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static bool IsRowVersion(DataObjectBase column)
        {
            bool isTimeStamp = column.NativeType.Equals(
                "timestamp", StringComparison.OrdinalIgnoreCase);
            bool isRowVersion = column.NativeType.Equals(
                "rowversion", StringComparison.OrdinalIgnoreCase);

            return (isTimeStamp || isRowVersion);
        }

        /// <summary>
        /// 判断数据库字段值是否是数据库自动生成
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static bool IsDbGenerated(DataObjectBase column)
        {
            if (IsRowVersion(column))
                return true;

            if (IsIdentity(column))
                return true;

            bool isComputed = false;
            try
            {
                if (column.ExtendedProperties.Contains(ExtendedPropertyNames.IsComputed))
                {
                    string value = column.ExtendedProperties[ExtendedPropertyNames.IsComputed].Value.ToString();
                    bool.TryParse(value, out isComputed);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("异常: " + ex.Message);
            }

            return isComputed;
        }

        /// <summary>
        /// 判断表是否是自增列
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static bool IsIdentity(DataObjectBase column)
        {
            bool isIdentity = false;
            try
            {
                if (column.ExtendedProperties.Contains(ExtendedPropertyNames.IsIdentity))
                {
                    string temp = column.ExtendedProperties[ExtendedPropertyNames.IsIdentity].Value.ToString();
                    bool.TryParse(temp, out isIdentity);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("异常: " + ex.Message);
            }

            return isIdentity;
        }

        /// <summary>
        /// 判断数据库字段是否有默认值
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static bool HasDefaultValue(DataObjectBase column)
        {
            try
            {
                if (!column.ExtendedProperties.Contains(ExtendedPropertyNames.DefaultValue))
                    return false;

                string value = column.ExtendedProperties[ExtendedPropertyNames.DefaultValue].Value.ToString();
                return !string.IsNullOrEmpty(value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("异常: " + ex.Message);
            }

            return false;
        }

        /// <summary>
        /// 获取数据库字段默认值值
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static string GetDefaultValue(DataObjectBase column)
        {
            try
            {
                if (!column.ExtendedProperties.Contains(ExtendedPropertyNames.DefaultValue))
                    return null;

                string value = column.ExtendedProperties[ExtendedPropertyNames.DefaultValue].Value.ToString();
                return value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("异常: " + ex.Message);
            }

            return null;
        }

        #endregion
    }
}
