using carbon.core.guards;
using NUnit.Framework;

namespace carbon.test.unit.core.guards
{
    [TestFixture]
    public class DoubleGuardTests
    {
        [Test]
        public void DoubleZeroGuardThrow()
        {    
            double test = 0;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.Zero);
            });

        }
        
        [Test]
        public void DoubleZeroGuardNoThrow()
        {
            double test = 110;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.Zero);
            });

        }
    }
}