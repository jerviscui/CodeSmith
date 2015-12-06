using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SchemaExplorer;

namespace CodeSmith.Test.EntityFramework
{
    [TestFixture]
    public class ContentTemplateTests
    {
        [Test]
        public void GetAndCreateTest()
        {
            GeneratorSettings generatorSettings = new GeneratorSettings();
            UniqueNamer uniqueNamer = new UniqueNamer();
            TemplateContent templateContent = new TemplateContent(generatorSettings, uniqueNamer);

            AbpDbContentTemplate abpDbContentTemplate = new AbpDbContentTemplate(templateContent);
            MySQLSchemaProvider mySqlSchemaProvider = new MySQLSchemaProvider();
            const string mysqlConn = "Server=127.0.0.1;Database=esms;Uid=root;Pwd=123qwe!@#;";
            SchemaSelector schemaSelector = new SchemaSelector(mySqlSchemaProvider, mysqlConn);

            foreach (TableSchema tableSchema in schemaSelector.Tables)
            {
                foreach (DataObjectBase columnSchema in tableSchema.Columns)
                {
                    Console.WriteLine(columnSchema.FullName);
                }
            }

            //EntityContext<AbpEntity, AbpEntityProperty> entityContext = abpDbContentTemplate.GetAndCreate(schemaSelector);
            //Entity<AbpEntityProperty> entity = entityContext.Entities.First(t => t.ClassName == "SysUser");

            //foreach (Relationship relationship in entity.Relationships.Where(t=>t.ThisEntity.Contains("SysUser")&&t.ThisPropertyName=="LastRole"))
            //{
            //    Console.WriteLine(relationship.JoinTable);
            //}

        }

        public string GetGridAllColumn(AbpEntity entity)
        {
            string html = "";
            List<AbpEntityProperty> properties = entity.Properties.ToList();
            string pkProperty = entity.PrimaryKey == null
                ? "Id"
                : entity.PrimaryKey.PropertyName.ToSafeName();
            properties.RemoveAll(
                property => property.PropertyName == pkProperty ||
                            property.PropertyName.ToSafeName() == "CreatorUserId" ||
                            property.PropertyName.ToSafeName() == "CreationTime" ||
                            property.PropertyName.ToSafeName() == "LastModificationTime" ||
                            property.PropertyName.ToSafeName() == "LastModifierUserId" ||
                            property.PropertyName.ToSafeName() == "DeleterUserId" ||
                            property.PropertyName.ToSafeName() == "DeletionTime" ||
                            property.PropertyName.ToSafeName() == "TenantId" ||
                            property.PropertyName.ToSafeName() == "IsDeleted" ||
                            property.PropertyName.ToSafeName() == "Sort" ||
                            property.IsForeignKey.HasValue && property.IsForeignKey.Value);
            for (int i = 0; i < properties.Count(); i++)
            {
                AbpEntityProperty property = properties[i];
                switch (property.SystemType.ToNullableType())
                {
                    case "System.DateTime":
                        html += "{" +
                          string.Format(
                              " field: '{0}', title: '{1}', width: '{2}%', align: 'center', sortable: true , formatter: $.girdFormatter.date ",
                              property.PropertyName.ToSafeName(),
                              property.Explain,
                              (properties.Any() ? 100 / properties.Count() : 99)) +
                          "}" + (i == properties.Count() ? "" : ",") + "\r\t\t\t\t\t\t\t";
                        break;
                    case "bool":
                        html += "{" +
                              string.Format(
                                  " field: '{0}', title: '{1}', width: '{2}%', align: 'center', sortable: true , formatter: $.girdFormatter.bool ",
                                  property.PropertyName.ToSafeName(),
                                  property.Explain,
                                  (properties.Any() ? 100 / properties.Count() : 99)) +
                              "}" + (i == properties.Count() ? "" : ",") + "\r\t\t\t\t\t\t\t";
                        break;
                    default:
                        html += "{" +
                                string.Format(
                                    " field: '{0}', title: '{1}', width: '{2}%', align: 'center', sortable: true ",
                                    property.PropertyName.ToSafeName(),
                                    property.Explain,
                                    (properties.Any() ? 100 / properties.Count() : 99)) +
                                "}" + (i == properties.Count() ? "" : ",") + "\r\t\t\t\t\t\t\t";
                        break;
                }
            }
            return html;
        }
    }
}
