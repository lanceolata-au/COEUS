using carbon.core.guards;
using NUnit.Framework;

namespace carbon.test.unit.core.guards
{
    [TestFixture]
    public class LongGuardTests
    {
        [Test]
        public void LongZeroGuardThrow()
        {    
            long test = 0;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.Zero);
            });

        }
        
        [Test]
        public void LongZeroGuardNoThrow()
        {
            long test = 110;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.Zero);
            });

        }
    }
}