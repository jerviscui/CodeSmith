using System;
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
            const string mysqlConn = "Server=127.0.0.1;Database=yt_qd_ytsf;Uid=root;Pwd=123qwe!@#;";
            SchemaSelector schemaSelector = new SchemaSelector(mySqlSchemaProvider, mysqlConn);
            EntityContext<AbpEntity, AbpEntityProperty> entityContext = abpDbContentTemplate.GetAndCreate(schemaSelector);
            foreach (AbpEntity entity in entityContext.Entities)
            {
                Console.WriteLine(entity.ClassName);
                Console.WriteLine(entity.PrimaryKey != null ? entity.PrimaryKey.SystemType.ToType() : "");
                Console.WriteLine(entity.Inherited);
                Console.WriteLine(entity.Description);
            }
        }
    }
}
