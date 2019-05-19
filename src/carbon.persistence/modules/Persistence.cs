using Autofac;
using carbon.persistence.features;

namespace carbon.persistence.modules
{
    public class Persistence : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ReadOnlyRepository>().InstancePerLifetimeScope();

        }
    }
}