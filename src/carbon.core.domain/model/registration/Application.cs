using System;
using carbon.core.Features;

namespace carbon.core.domain.model.registration
{
    public class Application : Entity<int>
    {
        public Guid UserId { get; private set; }
        
        
        public static Application Create(Guid userId)
        {
            //TODO Guards
                
            var obj = new Application();

            obj.UserId = userId;

            return obj;
        }
        
    }

}