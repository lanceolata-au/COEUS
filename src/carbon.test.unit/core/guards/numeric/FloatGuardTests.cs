using carbon.core.guards;
using NUnit.Framework;

namespace carbon.test.unit.core.guards.numeric
{
    [TestFixture]
    public class FloatGuardTests
    {
        [Test]
        public void FloatZeroGuardThrow()
        {    
            float test = 0;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.Zero);
            });

        }

        [Test]
        public void FloatZeroGuardNoThrow()
        {
            float test = 110;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.Zero);
            });

        }

        [Test]
        public void FloatNotZeroGuardThrow()
        {    
            float test = 100;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.NotZero);
            });

        }

        [Test]
        public void FloatNotZeroGuardNoThrow()
        {
            float test = 0;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.NotZero);
            });

        }

        [Test]
        public void FloatDefaultGuardThrow()
        {    
            float test = default;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.NullOrDefault);
            });

        }

        [Test]
        public void FloatDefaultGuardNoThrow()
        {
            float test = 10;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.NullOrDefault);
            });

        }
    }
}