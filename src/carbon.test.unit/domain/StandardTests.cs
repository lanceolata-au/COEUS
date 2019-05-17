using carbon.core.domain.model;
using NUnit.Framework;

namespace Tests.domain
{
    public class StandardTests
    {
        [Test]
        public void Test1()
        {
            var obj = Test.Create();
            
            Assert.IsNotNull(obj.Name);
        }
    }
}