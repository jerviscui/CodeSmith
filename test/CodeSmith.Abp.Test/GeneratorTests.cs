using System.Collections.Generic;
using System.Linq;
using CodeSmith.Abp.Extensions;
using CodeSmith.Abp.Generator;
using CodeSmith.Abp.Model;
using CodeSmith.Engine;
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
            //IDbSchemaProvider dbSchemaProvider = new SqlSchemaProvider();
            //const string connectionString = "Data Source=.;Initial Catalog=YT_QD_YTSF;User ID=sa;Password=sa";
            //DatabaseSchema databaseSchema = new DatabaseSchema(dbSchemaProvider, connectionString);
            //Generator.Generator generator = new Generator.Generator();

            //generator.Settings.CleanExpressions.Add("MigrationHistory");
            //generator.Settings.TableNaming = TableNaming.Plural;
            //EntityContext entityContext = generator.Generate(databaseSchema);
            //foreach (Entity entity in entityContext.Entities)
            //{
            //    Console.WriteLine(entity.ClassName + "-----------------");
            //    foreach (Property property in entity.Properties)
            //    {
            //        Console.WriteLine(property.PropertyName);
            //    }
            //}

        }

        [Test]
        public void GetTablePks()
        {
            IDbSchemaProvider dbSchemaProvider = new SqlSchemaProvider();
            const string connectionString = "Data Source=.;Initial Catalog=YT_QD_YTSF;User ID=sa;Password=sa";
            DatabaseSchema databaseSchema = new DatabaseSchema(dbSchemaProvider, connectionString);
            foreach (TableSchema tableSchema in databaseSchema.Tables)
            {
                ColumnSchema columnSchema = tableSchema.PrimaryKey.MemberColumns.FirstOrDefault();
                Console.WriteLine(columnSchema.Name + "----------" + columnSchema.SystemType.ToType());
            }
        }

        [Test]
        public void Comc()
        {
         

            Dictionary<string, int> columndDictionary = new Dictionary<string, int>
            {
                {"int",  25},
                {"LastModificationTime", 3},
                {"LastModifierUserId", 5},
                {"CreationTime", 7},
                {"CreatorUserId", 9},
                {"IsDeleted", 11},
                {"DeleterUserId", 13},
                {"DeletionTime", 15},
                {"IsActive", 17},
                {"TenantId", 19},
                {"TenantId?", 21},
                 {"Id", 23}
            };


            Console.WriteLine("IAudited :" + (7 * 9 * 3 * 5));       
            Console.WriteLine("ICreationAudited :" + (9 * 7));
            Console.WriteLine("IDeletionAudited :" + (11 * 12 * 15));
            Console.WriteLine("IFullAudited :" + (7 * 9 * 13 * 15));
            Console.WriteLine("IHasCreationTime :" + (7));
            Console.WriteLine("IModificationAudited :" + (3 * 5));
            Console.WriteLine("IEntity:" + (25 * 23));
            Console.WriteLine("IMayHaveTenant  :" + 21);
            Console.WriteLine("IMustHaveTenant  :" + 19);
            Console.WriteLine("IPassivable  :" + 17);
            Console.WriteLine("ISoftDelete  :" + 11);
        }
    }
}
