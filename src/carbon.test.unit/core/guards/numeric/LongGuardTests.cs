using carbon.core.guards;
using NUnit.Framework;

namespace carbon.test.unit.core.guards.numeric
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
        
        [Test]
        public void LongNotZeroGuardThrow()
        {    
            long test = 100;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.NotZero);
            });

        }
        
        [Test]
        public void LongNotZeroGuardNoThrow()
        {
            long test = 0;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.NotZero);
            });

        }
        
        [Test]
        public void LongDefaultGuardThrow()
        {    
            long test = default;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.Default);
            });

        }
        
        [Test]
        public void LongDefaultGuardNoThrow()
        {
            long test = 10;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.Default);
            });

        }
    }
}