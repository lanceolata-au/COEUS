using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using carbon.core.domain.model.account;
using carbon.core.domain.model.registration;
using carbon.core.dtos.account;
using carbon.core.dtos.filter;
using carbon.core.dtos.model.registration;
using carbon.persistence.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace carbon.api.Controllers.ScoutEvent
{
    [ApiController]
    [Route("api/profile/")]
    public class ProfileController : CarbonAuthenticatedController
    {
        private readonly IUserStore<IdentityUser> _users;
        private readonly IReadWriteRepository _readWriteRepository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public ProfileController(IUserStore<IdentityUser> users, 
            IReadWriteRepository readWriteRepository, 
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper)
            :base(users, readOnlyRepository, mapper)
        {
            _users = users;
            _readWriteRepository = readWriteRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        [Route("my-full")]
        public async Task<IActionResult> GetApplicationById()
        {
            var profile = await GetUserProfile();

            if (profile.CoreUserDto.Access < AccessEnum.Standard) return Unauthorized();

            var application = FindApplicationById(profile.CoreUserDto.UserId, 1);
            
            var dto = _mapper.Map<ApplicationDto>(application);

            return Ok(dto);
        }

        //Helpers
        
        private Application FindApplicationById(Guid id, int eventId)
        {
            Application application;
            
            if (_readOnlyRepository.Table<Application,int>().Any(a => a.UserId.Equals(id)))
            {
                application = _readOnlyRepository.Table<Application, int>()
                    .First(a => a.UserId.Equals(id));
            }
            else
            {
                application = Application.Create(id, eventId);
            
                _readWriteRepository.Create<Application, int>(application);
            }

            return application;
        }
        
    }
}