using System;
using carbon.core.guards;
using NUnit.Framework;

namespace carbon.test.unit.core.guards.text
{
    [TestFixture]
    public class StringGuardTests
    {

        [Test]
        public void StringNullOrEmptyGuardThrow()
        {    
            string test = string.Empty;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.NullOrEmpty);
            });

        }
        
        [Test]
        public void StringNullOrEmptyNoThrow()
        {
            string test = "test";

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.NullOrEmpty);
            });

        }
        
        [Test]
        public void StringDefaultGuardThrow()
        {    
            string test = String.Empty;

            Assert.Throws<GuardException>(() =>
            {
                Guard.Against(test, GuardType.Default);
            });

        }
        
        [Test]
        public void StringDefaultNoThrow()
        {
            string test = "test";

            Assert.DoesNotThrow(() =>
            {
                Guard.Against(test, GuardType.Default);
            });

        }
    }
}