using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using carbon.core.domain.model.registration;
using carbon.core.dtos.account;
using carbon.core.dtos.model.registration;
using carbon.persistence.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace carbon.api.Controllers.ScoutEvent
{
    [SecurityHeaders]
    [Authorize]
    public class ApplicationController : CarbonAuthenticatedController
    {
        private readonly IUserStore<IdentityUser> _users;
        private readonly IReadWriteRepository _readWriteRepository;
        private readonly IReadOnlyRepository _readOnlyRepository;    
        private readonly IMapper _mapper;

        public ApplicationController(IUserStore<IdentityUser> users,
            IReadWriteRepository readWriteRepository,
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper)
            : base(users, readOnlyRepository, mapper)
        {
            _users = users;
            _readWriteRepository = readWriteRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        [HttpGet, Route("{id}/status")]
        public async Task<IActionResult> GetStatus(Guid id)
        {
            var profile = await GetUserProfile();

            if (profile.CoreUserDto.Access < AccessEnum.Admin)
            {
                if (!profile.CoreUserDto.UserId.Equals(id)) return Unauthorized(
                    "You cannot access another users application");
            }

            if (!_readOnlyRepository.Table<Application, int>().Any(a => a.UserId.Equals(id)))
                return Ok(StatusEnum.NotStarted);

            var application = _readOnlyRepository.Table<Application, int>().First(a => a.UserId.Equals(id));
            
            return Ok(application.Status);
        }

        [HttpGet, Route("{id}/new")]
        public async Task<IActionResult> GetNewApplication(Guid id)
        {
            var profile = await GetUserProfile();

            if (profile.CoreUserDto.Access < AccessEnum.Admin)
            {
                if (!profile.CoreUserDto.UserId.Equals(id)) return Unauthorized(
                    "You cannot access another users application");
            }
            
            var application = Application.Create(id);
            
            _readWriteRepository.Create<Application, int>(application);

            var dto = _mapper.Map<ApplicationDto>(application);

            return Ok(dto);
        }
        
    }
}