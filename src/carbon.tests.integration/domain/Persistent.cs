using carbon.persistence.transforms;
using carbon.core.domain.model;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Test = carbon.core.domain.model.Test;

namespace carbon.tests.integration.domain
{
    [TestFixture]
    public class Persistent
    {

        [SetUp]
        public void TestDatabaseUp()
        {
            var obj = new Runner(@"server=zeryter.xyz;user=carbonTest;password=the_game", false, false, dbName: "carbonTest");
        }
        
        [Test]
        public void IsPersisting()
        {
            var obj = Test.Create();
            
            
            Assert.Fail(obj.Name);
            
        }
    }
}