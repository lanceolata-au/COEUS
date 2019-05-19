using System;
using System.Linq;
using carbon.core.domain.model;
using carbon.persistence.features;
using carbon.runner.database.transforms;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace carbon.tests.integration.domain
{
    [TestFixture]
    public class Persistent
    {
        public static string ConnectionString { get; } =
            "server=zeryter.xyz;database=carbonTest;user=carbonTest;password=the_game";

        [SetUp]
        public void TestDatabaseUp()
        {
            var obj = new Runner(@"server=zeryter.xyz;user=carbonTest;password=the_game", 
                false, true, dbName: "carbonTest");
        }
        
        [Test]
        public void IsPersisting()
        {

            var optionsBuilder = new DbContextOptionsBuilder();

            optionsBuilder.UseMySql(ConnectionString);

            var dbOptions = optionsBuilder.Options;

            var dbContext = new CoreDbContext(dbOptions);

            var readRepo = new ReadOnlyRepository(dbContext);

            var testObj = readRepo.Table<Test, Guid>().First();
            
            Assert.IsNotNull(testObj.Name);
            Assert.IsNotNull(testObj.Value);
            
            Assert.GreaterOrEqual(testObj.Value,100);

        }
    }
}