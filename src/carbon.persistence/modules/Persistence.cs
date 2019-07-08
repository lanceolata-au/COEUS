using Autofac;
using carbon.persistence.features;
using carbon.persistence.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace carbon.persistence.modules
{
    public class Persistence : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ReadOnlyRepository>().As<IReadOnlyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ReadWriteRepository>().As<IReadWriteRepository>().InstancePerLifetimeScope();
            
            //Db Context Options
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();
                var optionsBuilder = new DbContextOptionsBuilder();
                optionsBuilder.UseMySql(config.GetConnectionString("ApplicationDatabase"));
                return optionsBuilder.Options;
            }).As<DbContextOptions>().SingleInstance();


            builder.RegisterType<CoreDbContext>().As<DbContext>().InstancePerLifetimeScope();

        }
    }
}