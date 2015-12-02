using System;
using System.Linq;
using CodeSmith.Engine;
using CodeSmith.EntityFramework;
using SchemaExplorer;

namespace CodeSmith.Template.Test
{
    public class EntityEditableClassTest
    {
        [NUnit.Framework.Test]
        public void Generator()
        {
            //Generator generator = new Generator();
            MySQLSchemaProvider mySqlSchemaProvider = new MySQLSchemaProvider();
            string mysqlConn = "Server=127.0.0.1;Database=yt_qd_ytsf;Uid=root;Pwd=123qwe!@#;";
            //string mssqlConn = "Server=127.0.0.1;Database=YT_QD_YTSF;Uid=sa;Pwd=sa;";
            //SqlSchemaProvider sqlSchemaProvider =new SqlSchemaProvider();
            ////EntityContext entityContext = generator.Generate(new SchemaSelector(mySqlSchemaProvider, "Server=127.0.0.1;Database=yt_qd_ytsf;Uid=root;Pwd=123qwe!@#;"));
            //EntityContext entityContext = generator.Generate(new SchemaSelector(sqlSchemaProvider, mssqlConn));
            ////const string path = @"E:\Oneself\CodeSmith\src\CodeSmith.Template\Entity.Generated.cst";
            ////CodeTemplateCompiler codeTemplateCompiler = new CodeTemplateCompiler(path);
            ////codeTemplateCompiler.Compile();        
            ////CodeTemplate codeTemplate = codeTemplateCompiler.CreateInstance();
            ////codeTemplate.SetProperty("Entity", entityContext.Entities.First());
            ////codeTemplate.SetProperty("EntityNamespace", "123");
            ////Console.WriteLine(codeTemplate.RenderToString()); 
            //Entity entity = entityContext.Entities.Take(2).First();
            //foreach (Property property in entity.Properties)
            //{
            //    Console.WriteLine(property.DataType);
            //    Console.WriteLine(property.NativeType);
            //    Console.WriteLine(property.SystemType);
            //    Console.WriteLine("    ");
            //}
            SchemaSelector schemaSelector = new SchemaSelector(mySqlSchemaProvider, mysqlConn);
            foreach (TableSchema tableSchema in schemaSelector.Database.Tables)
            {
                Console.WriteLine(tableSchema.Name);
                foreach (ColumnSchema columnSchema in tableSchema.Columns)
                {
                    Console.WriteLine("--" + columnSchema.Name + "--" + columnSchema.SystemType);
                }
            }

            Console.WriteLine(CodeSmith.Engine.StringUtil.ToPascalCase("bs_user"));
        }
    }
}
