using System;
using System.Collections.Generic;
using System.Linq;
using CodeSmith.Abp.Model;

namespace CodeSmith.Abp.Extensions
{
    public static class AbpExtension
    {
        /// <summary>
        /// 获取实体继承标识
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string ToEntityInterface(this Entity entity)
        {
            string output = " : Entity<" + entity.Properties.First(t => t.ColumnName == "Id").SystemType.ToNullableType() + ">";

            List<string> columnNames = entity.Properties.Select(t => t.PropertyName).ToList();
            //foreach (var columnName in columnNames)
            //{
            //    output += columnName + ",";
            //}
            if (columnNames.Any(t => t == "CreatorUserId")
                && columnNames.Any(t => t == "CreationTime")
                && columnNames.Any(t => t == "LastModificationTime")
                && columnNames.Any(t => t == "LastModifierUserId"))
                output += ",IAudited";

            if (columnNames.Any(t => t == "CreatorUserId")
                && columnNames.Any(t => t == "CreationTime"))
                output += ",ICreationAudited";

            if (columnNames.Any(t => t == "DeleterUserId")
                && columnNames.Any(t => t == "DeletionTime")
                && columnNames.Any(t => t == "IsDeleted"))
                output += ",IDeletionAudited";

            if (columnNames.Any(t => t == "CreatorUserId")
                && columnNames.Any(t => t == "CreationTime")
                && columnNames.Any(t => t == "LastModificationTime")
                && columnNames.Any(t => t == "LastModifierUserId")
                && columnNames.Any(t => t == "DeleterUserId")
                && columnNames.Any(t => t == "DeletionTime"))
                output += ",IFullAudited";

            if (columnNames.Any(t => t == "CreationTime"))
                output += ",IHasCreationTime";

            if (columnNames.Any(t => t == "LastModificationTime")
                && columnNames.Any(t => t == "LastModifierUserId"))
                output += ",IModificationAudited";
     
            if (columnNames.Any(t => t == "TenantId"))
            {
                if (!entity.Properties.First(t => t.PropertyName == "TenantId").IsRequired)
                    output += ",IMayHaveTenant";
            }

            if (columnNames.Any(t => t == "TenantId"))
                output += ",IMustHaveTenant";

            if (columnNames.Any(t => t == "IsActive"))
                output += ",IPassivable";

            if (columnNames.Any(t => t == "IsDeleted"))
                output += ",ISoftDelete";

            return output;
        }


        /// <summary>
        /// 判断是否有枚举类型
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool IsHaveEnum(this List<EnumItem> list, string entityName, string propertyName)
        {
            string tableName = entityName;
            string columnName = propertyName;
            return list.FirstOrDefault(t => t.TableName.Trim() == tableName && t.ColumnName.Trim() == columnName) != null;
        }

        /// <summary>
        /// 获取枚举类型
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityName"></param>
        /// <param name="propertyName"></param>
        /// <param name="isNull"></param>
        /// <returns></returns>
        public static string GetEnumType(this List<EnumItem> list, string entityName, string propertyName, bool isNull = false)
        {
            string tableName = entityName;
            string columnName = propertyName;
            EnumItem enumItem =
                list.First(t => t.TableName.Trim() == tableName && t.ColumnName.Trim() == columnName);
            return enumItem.TypeName + (isNull ? "?" : "");
        }
    }
}
