using System;
using System.Linq;
using carbon.core.domain.model;
using carbon.persistence.features;
using carbon.runner.database.transforms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace carbon.tests.integration.domain
{
    [TestFixture]
    public class Persistent
    {
        private string ConnectionString { get; } =
            "server=zeryter.xyz;database=carbonTest;user=carbonTest;password=the_game";

        private DbContextOptionsBuilder _optionsBuilder = new DbContextOptionsBuilder();
        private DbContextOptions _contextOptions;
        private DbContext _dbContext;


        [SetUp]
        public void Setup()
        {
            var obj = new Runner(@"server=zeryter.xyz;user=carbonTest;password=the_game", 
                false, true, dbName: "carbonTest");

            _optionsBuilder.UseMySql(ConnectionString);
            _contextOptions = _optionsBuilder.Options;
            _dbContext = new CoreDbContext(_contextOptions);
        }
        
        [Test]
        public void IsReading()
        {

            var readRepo = new ReadOnlyRepository(_dbContext);

            var testObj = readRepo.Table<Test, Guid>().First();
            
            Assert.IsNotNull(testObj.Name);
            Assert.IsNotNull(testObj.Value);
            
            Assert.GreaterOrEqual(testObj.Value,100);

        }

        [Test]    
        public void IsWritingAdd()
        {
            var obj = Test.Create();
            
            obj.Update("Test_Is_Write",11000011);

            var objId = obj.Id;
            
            var writeRepo = new ReadWriteRepository(_dbContext);
            
            writeRepo.Create<Test,Guid>(obj);
            
            //THIS IS NOT DONE IN PRODUCTION
            writeRepo.Commit();
            
            var readRepo = new ReadOnlyRepository(_dbContext);

            var testObj = readRepo.GetById<Test, Guid>(objId);
            
            Assert.AreEqual(obj.Name,testObj.Name);
            Assert.AreEqual(obj.Value,testObj.Value);

        }
        
        [Test]
        public void IsWritingUpdate()
        {
            var writeRepo = new ReadWriteRepository(_dbContext);
            
            var obj = writeRepo.Table<Test, Guid>().First();

            var testId = obj.Id;
            var valueTest = obj.Value + 1;
            
            obj.Update(obj.Name, valueTest);
            
            writeRepo.Commit();
            
            var readRepo = new ReadOnlyRepository(_dbContext);

            var testObj = readRepo.GetById<Test, Guid>(testId);
            
            Assert.AreEqual(valueTest,testObj.Value);
            
        }
        
    }
}