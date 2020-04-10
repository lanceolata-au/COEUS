using carbon.core.guards;
using NUnit.Framework;

namespace carbon.test.unit.core.guards.numeric
{
    [TestFixture]
    public class ByteGuardTests
    {
        [Test]
        public void ByteZeroGuardThrow()
        {    
            byte test = 0;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.Zero);
            });

        }
        
        [Test]
        public void ByteZeroGuardNoThrow()
        {
            byte test = 110;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.Zero);
            });

        }
        
        [Test]
        public void ByteNotZeroGuardThrow()
        {    
            byte test = 100;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.NotZero);
            });

        }
        
        [Test]
        public void ByteNotZeroGuardNoThrow()
        {
            byte test = 0;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.NotZero);
            });

        }
        
        [Test]
        public void ByteDefaultGuardThrow()
        {    
            byte test = default;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.NullOrDefault);
            });

        }
        
        [Test]
        public void ByteDefaultGuardNoThrow()
        {
            byte test = 10;

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.NullOrDefault);
            });

        }
    }
}