using carbon.core.domain.model;
using NUnit.Framework;

namespace carbon.test.unit.domain
{
    public class StandardTests
    {
        [Test]
        public void Test1()
        {
            var obj = Test.Create();
            
            Assert.IsNotNull(obj.Name);
        }

        [Test]
        public void Test2()
        {
            var obj = Test.Create();
            
            Assert.IsNotNull(obj.Value);
        }
        
    }
}