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
    public class AdminController : CarbonAuthenticatedController
    {
        private readonly IUserStore<IdentityUser> _users;
        private readonly IReadWriteRepository _readWriteRepository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public AdminController(IUserStore<IdentityUser> users, 
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
        public async Task<IActionResult> GetUsers()
        {
            var user = await GetUserProfile();
            if (user.CoreUserDto.Access < AccessEnum.Admin) return Unauthorized();

            var coreUsers = _readOnlyRepository.Table<CoreUser, Guid>().ToList();

            var users = new List<UserDto>();
            
            var lookUps = new List<Task<UserDto>>();
            
            foreach (var coreUser in coreUsers)
            {
                lookUps.Add(GetUserDto(coreUser));
            }

            await Task.WhenAll(lookUps);

            foreach (var lookUp in lookUps)
            {
                users.Add(lookUp.Result);
            }
            
            return Ok(users);    

        }

        [HttpPost]
        public async Task<IActionResult> GetApplicationsPackage([FromBody] ApplicationFilterDto filter)
        {
            var user = await GetUserProfile();
            if (user.CoreUserDto.Access < AccessEnum.Admin) return Unauthorized();

            var applications = _readOnlyRepository.Table<Application, int>().ToList();
            
            var applicationDtos = new List<ApplicationDto>();
            var countryDtos = new List<CountryDto>();
            var stateDtos = new List<StateDto>();

            bool filtered;
            
            applications.ForEach(application =>
            {
                
                if (countryDtos.All(dto => dto.Id != application.Country))
                {
                    //Add applications country to countryDtos as it does not yet exist
                    var country = _readOnlyRepository.GetById<Country, int>(application.Country);

                    var countryDto = _mapper.Map<CountryDto>(country);
                    
                    countryDtos.Add(countryDto);

                }

                if (application.State != 0 && stateDtos.All(dto => dto.Id != application.State))
                {
                    //Add applications state to stateDtos as it does not yet exist
                    var state = _readOnlyRepository.GetById<State, int>(application.State);

                    var stateDto = _mapper.Map<StateDto>(state);
                    
                    stateDtos.Add(stateDto);

                }

                filtered = false;

                if (filter.Countries != null)
                {
                    if (filter.Countries.All(f => f != application.Country))
                    {
                        filtered = true;
                    }
                }

                if (!filtered && filter.States != null)
                {
                    if (filter.States.All(f => f != application.State))
                    {
                        filtered = true;
                    }
                }

                if (!filtered && filter.AgeDate != default && filter.MaximumAge != 0)
                {
                    //TODO age max filter
                }
            
                if (!filtered && filter.AgeDate != default && filter.MinimumAge != 0)
                {
                    //TODO age min filter
                }

                if (!filtered)
                {
                    applicationDtos.Add(_mapper.Map<ApplicationDto>(application));
                }

            });

            
            

            //Sort all lists in order of ID
            var applicationCount = applicationDtos.Count;
            applicationDtos = applicationDtos.OrderBy(a => a.Id).ToList();
            countryDtos = countryDtos.OrderBy(c => c.Id).ToList();
            stateDtos = stateDtos.OrderBy(s => s.Id).ToList();

            applicationDtos = applicationDtos.Skip((filter.Page - 1) * filter.ResultsPerPage).ToList();
            applicationDtos = applicationDtos.Take(filter.ResultsPerPage).ToList();
            
            var package = new ApplicationsPackageDto
            {
                ApplicationCount = applicationCount,
                Applications = applicationDtos,
                ApplicationCountries = countryDtos,
                ApplicationStates = stateDtos
            };
            
            return Ok(package);
        }

        [HttpGet]
        public async Task<IActionResult> GetDefaultFilter()
        {
            var filter = new ApplicationFilterDto();
            
            return Ok(filter);
        }
        
        private async Task<UserDto> GetUserDto(CoreUser coreUser)
        {
            var identityUser = await _users.FindByIdAsync(coreUser.Id.ToString(), new CancellationToken());
                    
            return new UserDto()
            {
                Id = coreUser.Id,
                UserName = identityUser.UserName,
                Email = identityUser.Email,
                EmailConfirmed = identityUser.EmailConfirmed,
                TwoFactorEnabled = identityUser.EmailConfirmed,
                AccessFailedCount = identityUser.AccessFailedCount,
                CoreUser = _mapper.Map<CoreUserDto>(coreUser)
            };
        }

    }
}