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
            Generator generator = new Generator();
            IDbSchemaProvider dbSchemaProvider = new MySQLSchemaProvider();
            generator.Generate(new SchemaSelector(dbSchemaProvider, "Server=127.0.0.1;Database=world;Uid=root;Pwd=123qwe!@#;"));
            Assert.Fail();
        }
    }
}
