using System;
using carbon.core.dtos.model.registration;
using carbon.core.Features;

namespace carbon.core.domain.model.registration
{
    public class Application : Entity<int>
    {

        public Guid UserId { get; private set; }
        public StatusEnum Status { get; private set; }
        
        public static Application Create(Guid userId)
        {
            //TODO Guards
                
            var obj = new Application();

            obj.UserId = userId;

            return obj;
        }

        public void Update(ApplicationDto dto)
        {
            Status = dto.Status;
        }
        
    }

}