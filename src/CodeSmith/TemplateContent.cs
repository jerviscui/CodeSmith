using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using CodeSmith.Engine;
using SchemaExplorer;

namespace CodeSmith
{
    public class TemplateContent
    {
        public readonly GeneratorSettings Settings;

        public readonly UniqueNamer UniqueNamer;

        public TemplateContent(GeneratorSettings settings, UniqueNamer uniqueNamer)
        {
            Settings = settings;
            UniqueNamer = uniqueNamer;
        }
  
        /// <summary>
        /// 获取数据库表中外键与主键所有的属性名称
        /// </summary>
        /// <param name="entity">外键表或主表实体</param>
        /// <param name="members">主键字段或者外键字段</param>
        /// <param name="name">主表表名</param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public List<string> GetKeyMembers<TProperty>(Entity<TProperty> entity, IEnumerable<MemberColumnSchema> members, string name, out bool isRequired) where TProperty : Property
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

        public string GetMemberPrefix(Relationship relationship, string primaryClass, string foreignClass)
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
        public bool IsOneToOne(TableKeySchema tableKeySchema, Relationship foreignRelationship)
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
        public bool IsManyToMany(TableSchema tableSchema)
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
        public string ToClassName(string name)
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
        public string ToPropertyName(string className, string name)
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
        public string ToLegalName(string name)
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
        public bool IsCascadeDelete(SchemaObjectBase column)
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
        public bool IsRowVersion(DataObjectBase column)
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
        public bool IsDbGenerated(DataObjectBase column)
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
        public bool IsIdentity(DataObjectBase column)
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
        public bool HasDefaultValue(DataObjectBase column)
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
        public  string GetDefaultValue(DataObjectBase column)
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
