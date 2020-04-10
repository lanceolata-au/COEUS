using carbon.core.guards;
using NUnit.Framework;

namespace carbon.test.unit.core.guards.numeric
{
    [TestFixture]
    public class DecimalGuardTests
    {
        [Test]
        public void DecimalZeroGuardThrow()
        {    
            decimal test = 0;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.Zero);
            });

        }

        [Test]
        public void DecimalZeroGuardNoThrow()
        {
            decimal test = 110;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.Zero);
            });

        }

        [Test]
        public void DecimalNotZeroGuardThrow()
        {    
            decimal test = 100;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.NotZero);
            });

        }

        [Test]
        public void DecimalNotZeroGuardNoThrow()
        {
            decimal test = 0;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.NotZero);
            });

        }

        [Test]
        public void DecimalDefaultGuardThrow()
        {    
            decimal test = default;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.Default);
            });

        }

        [Test]
        public void DecimalDefaultGuardNoThrow()
        {
            decimal test = 10;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.Default);
            });

        }
    }
}