using Autofac;
using carbon.api.Services;
using carbon.persistence.modules;
using NUnit.Framework;

namespace carbon.test.unit.domain
{
    [TestFixture]
    public class StartupTest
    {
        [Test]
        public void AutofacRegistering()
        {
            
            Assert.DoesNotThrow(() =>
            {
                var assemblies = AppScanner.GetCarbonAssemblies();

                var builder = new ContainerBuilder();

                builder.RegisterModule(new Persistence());
            });

        }
    }
}