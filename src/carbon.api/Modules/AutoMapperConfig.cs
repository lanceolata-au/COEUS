using Autofac;
using AutoMapper;
using carbon.core.domain.model.account;
using carbon.core.domain.model.registration;
using carbon.core.dtos.account;
using carbon.core.dtos.model.registration;

namespace carbon.api.Modules
{
    public class AutoMapperConfig : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(mp =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    /*
                     * Used to specify mapping configs
                     *
                     * Add further domain to dto configs in here
                     */
                    
                    //User Details
                    cfg.CreateMap<CoreUser,CoreUserDto>();
                    
                    //Application
                    cfg.CreateMap<Application, ApplicationDto>();
                    cfg.CreateMap<Country, CountryDto>();
                    cfg.CreateMap<State, StateDto>();

                });

                return config.CreateMapper();

            }).As<IMapper>().SingleInstance();
            
        }
    }
}