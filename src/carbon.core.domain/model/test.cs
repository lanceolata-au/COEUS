using System;
using carbon.core.Features;

namespace carbon.core.domain.model
{
    public class Test : Entity<Guid>
    {
        
        public string Name { get; private set; }
        public int Value { get; private set; }
        
        public static Test Create()
        {
            var obj  = new Test();

            obj.Name = "DEFAULT";
            obj.Value = 100;
            
            return obj;
        }

        public void Update(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}