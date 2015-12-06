using System;
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
            EntityContext<AbpEntity, AbpEntityProperty> entityContext = abpDbContentTemplate.GetAndCreate(schemaSelector);
            Entity<AbpEntityProperty> entity = entityContext.Entities.First(t => t.ClassName == "SysUser");
            
            foreach (Relationship relationship in entity.Relationships.Where(t=>t.ThisEntity.Contains("SysUser")&&t.ThisPropertyName=="LastRole"))
            {
                Console.WriteLine(relationship.JoinTable);
            }

        }



        public string GetInputHtml<TProperty>(Entity<TProperty> entity, Property property) where TProperty : Property
        {
            string html = "";
            switch (property.SystemType.ToNullableType())
            {
                case "System.DateTime":
                    if (property.IsNullable.HasValue && property.IsNullable.Value)
                    {
                        html =
                            string.Format(
                                "<input name=\"{0}\" class=\"easyui-datebox\" data-options=\"required:{1},width:350\" value=\"@(Model.{0}.HasValue?Model.{0}.Value.ToString(\"yyyy-MM-dd\"):\"\")\"/>",
                                property.PropertyName.ToSafeName(),
                                "true");
                    }
                    else
                    {
                        html =
                            string.Format(
                                "<input name=\"{0}\" class=\"easyui-datebox\" data-options=\"required:{1},width:350\" value=\"@(Model.{2})\"/>",
                                property.PropertyName.ToSafeName(),
                                "false",
                                property.PropertyName.ToSafeName() + ".ToString(\"yyyy-MM-dd\")");
                    }
                    break;
                case "bool":
                    html =
                        string.Format(
                            "<select name=\"{0}\" class=\"easyui-combobox\" data-options=\"required:{1},panelHeight:50,width:80,editable:false\" >" +
                            "\r\t\t\t\t\t\t\t<option value=\"true\" @(Model.{0} ? \"selected\" : \"\")>是</option>" +
                            "\r\t\t\t\t\t\t\t<option value=\"false\" @(!Model.{0} ? \"selected\" : \"\")>否</option>" +
                            "\r</select>",
                            property.PropertyName.ToSafeName(),
                            (property.IsNullable.HasValue && property.IsNullable.Value ? "false" : "true"));
                    break;
                case "int":
                    if (property.IsForeignKey.HasValue && property.IsForeignKey.Value)
                    {
                        Relationship relationship =
                            entity.Relationships.First(t => t.ThisProperties.Any(o => o.Contains(property.PropertyName)));
                        string fkClassName = relationship.OtherEntity;
                        html =
                       string.Format(
                           "<select name=\"{0}\" class=\"easyui-combobox\" data-options=\"url:{2},required:{1},panelHeight:50,width:350,editable:false\"  value=\"{3}\"></select>",
                           property.PropertyName.ToSafeName(),
                           (property.IsNullable.HasValue && property.IsNullable.Value ? "false" : "true"),
                           "/Service/" + fkClassName + "/GetCombo",
                           (property.IsNullable.HasValue && property.IsNullable.Value
                                ? "@(Model." + property.PropertyName + ".HasValue?Model." + property.PropertyName + ".Value:\"\")"
                                : "@(Model." + property.PropertyName + "==0?\"\":\"@Model." + property.PropertyName + "\")"));
                    }
                    else
                    {
                        html =
                        string.Format(
                            "<input name=\"{0}\" class=\"easyui-numberbox\" data-options=\"required:{1},width:350\" value=\"{2}\"/>",
                            property.PropertyName.ToSafeName(),
                            (property.IsNullable.HasValue && property.IsNullable.Value ? "false" : "true"),
                            (property.IsNullable.HasValue && property.IsNullable.Value
                                ? "@(Model." + property.PropertyName + ".HasValue?Model." + property.PropertyName + ".Value:\"\")"
                                : "@(Model." + property.PropertyName + "==0?\"\":\"@Model." + property.PropertyName + "\")"));
                    }
                    break;
                default:
                    html =
                        string.Format(
                            "<input name=\"{0}\" class=\"{1}\" data-options=\"required:{2},width:350\" value=\"@Model.{0}\"/>",
                            property.PropertyName.ToSafeName(),
                            "easyui-textbox",
                            (property.IsNullable.HasValue && property.IsNullable.Value ? "false" : "true"));
                    break;
            }
            return html;
        }
    }
}
