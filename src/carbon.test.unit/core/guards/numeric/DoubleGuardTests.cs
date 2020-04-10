using carbon.core.guards;
using NUnit.Framework;

namespace carbon.test.unit.core.guards.numeric
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
        
        [Test]
        public void DoubleNotZeroGuardThrow()
        {    
            double test = 100;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.NotZero);
            });

        }
        
        [Test]
        public void DoubleNotZeroGuardNoThrow()
        {
            double test = 0;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.NotZero);
            });

        }
        
        [Test]
        public void DoubleDefaultGuardThrow()
        {    
            double test = default;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.NullOrDefault);
            });

        }
        
        [Test]
        public void DoubleDefaultGuardNoThrow()
        {
            double test = 10;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.NullOrDefault);
            });

        }
    }
}