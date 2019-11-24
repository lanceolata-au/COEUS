using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using carbon.core.domain.model.registration;
using carbon.core.dtos.model.registration;
using carbon.persistence.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        
        [HttpGet]
        public async Task<IActionResult> GetStatus()
        {
            var profile = await GetUserProfile();

            if (!_readOnlyRepository.Table<Application, int>().Any(a => a.UserId.Equals(profile.CoreUserDto.UserId)))
                return Ok(StatusEnum.NotStarted);

            var application = _readOnlyRepository.Table<Application, int>().First(a => a.UserId.Equals(profile.CoreUserDto.UserId));
            
            return Ok(application.Status);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetApplication()
        {
            var profile = await GetUserProfile();

            Application application;
            
            if (_readOnlyRepository.Table<Application,int>().Any(a => a.UserId.Equals(profile.CoreUserDto.UserId)))
            {
                application = _readOnlyRepository.Table<Application, int>()
                    .First(a => a.UserId.Equals(profile.CoreUserDto.UserId));
            }
            else
            {
                application = Application.Create(profile.CoreUserDto.UserId);
            
                _readWriteRepository.Create<Application, int>(application);
            }
            
            var dto = _mapper.Map<ApplicationDto>(application);

            return Ok(dto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> NewPreliminaryApplication([FromBody] PreliminaryApplicationDto applicationDto)
        {
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetBlankPreliminaryApplication()
        {
            var provider = CultureInfo.InvariantCulture;  
            
            var applicationDto = new PreliminaryApplicationDto
            {
                Status = StatusEnum.Preliminary,
                DateOfBirth = DateTime.ParseExact("26/12/2004","dd/mm/yyyy", provider)
            };

            return Ok(applicationDto);
        }
        
    }
}