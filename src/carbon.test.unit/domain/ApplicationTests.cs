using System;
using carbon.core.domain.model.registration;
using carbon.core.dtos.model.registration;
using carbon.core.execeptions;
using NUnit.Framework;

namespace carbon.test.unit.domain
{
    [TestFixture]
    public class ApplicationTests
    {
        [Test]
        public void ThrowsIdMismatch()
        {
            var obj = new Application();
            obj.UserId = Guid.NewGuid();
            
            Assert.Catch<IdMismatchException>(() => obj.Update(new ApplicationDto()
            {
                UserId = Guid.NewGuid()
            }));
            
        } 
        
        [Test]
        public void UpdateLockTest()
        {
            var obj = new Application();
            obj.UserId = Guid.NewGuid();
            obj.Locked = true;
            
            Assert.Catch<CarbonDomainException>(() => obj.Update(new ApplicationDto()
            {
                UserId = obj.UserId
            }));
            
        }
    }
}