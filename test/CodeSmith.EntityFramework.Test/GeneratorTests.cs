using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSmith.EntityFramework;
using NUnit.Framework;
using SchemaExplorer;

namespace CodeSmith.EntityFramework.Test
{
    [TestFixture()]
    public class GeneratorTests
    {
        [Test()]
        public void GeneratorTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GenerateTest()
        {
            GeneratorSettings generatorSettings = new GeneratorSettings();
            MySQLSchemaProvider mySqlSchemaProvider = new MySQLSchemaProvider();
            const string mysqlConn = "Server=127.0.0.1;Database=yt_qd_ytsf;Uid=root;Pwd=123qwe!@#;";
            SchemaSelector schemaSelector = new SchemaSelector(mySqlSchemaProvider, mysqlConn);
            Generator generator = new Generator();
            generator.Settings.TableNaming = generatorSettings.TableNaming;
            generator.Settings.EntityNaming = generatorSettings.EntityNaming;
            generator.Settings.RelationshipNaming = generatorSettings.RelationshipNaming;
            generator.Settings.ContextNaming = generatorSettings.ContextNaming;
            EntityContext entityContext = generator.Generate(schemaSelector);
            foreach (Entity entity in entityContext.Entities)
            {
                Property property =
                    entity.Properties.FirstOrDefault(
                        t => (t.IsPrimaryKey.HasValue && t.IsPrimaryKey.Value) && (t.IsIdentity.HasValue && t.IsIdentity.Value));
                Console.WriteLine(entity.ClassName + " --- " + (property != null ? property.PropertyName : ""));

            }
        }
    }
}
