using CodeSmith.Abp.Generator;
using CodeSmith.Abp.Model;
using NUnit.Framework;
using SchemaExplorer;
using System;

namespace CodeSmith.Abp.Test
{
    [TestFixture]
    public class GeneratorTests
    {
        [Test]
        public void GeneratorTest()
        {
            IDbSchemaProvider dbSchemaProvider = new SqlSchemaProvider();
            const string connectionString = "Data Source=.;Initial Catalog=YT_QD_YTSF;User ID=sa;Password=sa";
            DatabaseSchema databaseSchema = new DatabaseSchema(dbSchemaProvider, connectionString);
            Generator.Generator generator = new Generator.Generator();
           
            generator.Settings.CleanExpressions.Add("MigrationHistory");
             generator.Settings.TableNaming= TableNaming.Plural;
            EntityContext entityContext = generator.Generate(databaseSchema);
            foreach (Entity entity in entityContext.Entities)
            {
                Console.WriteLine(entity.ClassName+"-----------------");
                foreach (Property property in entity.Properties)
                {
                    Console.WriteLine(property.PropertyName);
                }
            }

        }
    }
}
