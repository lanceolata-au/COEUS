using System;
using carbon.core.dtos.account;
using carbon.core.Features;

namespace carbon.core.domain.model.account
{
    public class CoreUser : Entity<Guid>
    {
        public AccessEnum Access { get; private set; }
        public string Picture { get; private set; }

        protected CoreUser() {}

        public static CoreUser Create(Guid userId)
        {
            //TODO Guards
            
            var obj = new CoreUser();

            obj.Id = userId;

            obj.Access = AccessEnum.Standard;
            
            obj.Picture = null;
            
            return obj;
        }

        public void Update(CoreUserDto updateObj)
        {
            Access = updateObj.Access;
            if (updateObj.Picture != null)
            {
                Picture = updateObj.Picture;
            }
        }
        
    }
}