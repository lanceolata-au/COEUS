using System;
using carbon.core.Features;

namespace carbon.core.domain.model
{
    public class Test : Entity<Guid>
    {
        public static Test Create()
        {
            var obj  = new Test();


            return obj;
        }
    }
}