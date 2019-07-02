using System;
using carbon.core.domain.model.account;
using carbon.core.dtos.account;
using carbon.persistence.interfaces;

namespace carbon.api.Features
{
    public class CoreUserRunner
    {
        private IReadOnlyRepository _readOnlyRepository;
        
        public CoreUserRunner(IReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }

        public CoreUserDto GetUser(Guid id)
        {
            var user = _readOnlyRepository.GetById<CoreUser, Guid>(id);
            
            return new CoreUserDto()
            {
                Picture = user.Picture,
                Access = user.Access
            };
        }
        
    }
}