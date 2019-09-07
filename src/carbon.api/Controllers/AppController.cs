using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using carbon.api.Models.Account;
using carbon.core.domain.model.account;
using carbon.core.dtos.account;
using carbon.persistence.interfaces;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace carbon.api.Controllers
{
    [SecurityHeaders]
    [Authorize]
    public class AppController : CarbonController
    {
        private readonly IUserStore<IdentityUser> _users;
        private readonly IReadWriteRepository _readWriteRepository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public AppController(IUserStore<IdentityUser> users, 
            IReadWriteRepository readWriteRepository, 
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _users = users;
            _readWriteRepository = readWriteRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> ExternalProfile()
        {
            if (User?.IsAuthenticated() != true) return Unauthorized();
            var vm = await BuildProfileViewModelAsync();

            return Ok(vm);

        }
        
        
        private async Task<ProfileViewModel> BuildProfileViewModelAsync()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                var identityUser = await _users.FindByIdAsync(User.Identity.GetSubjectId(), new CancellationToken());
                
                var coreUser = _readOnlyRepository.GetById<CoreUser, Guid>(Guid.Parse(identityUser.Id));
                
                return new ProfileViewModel()
                {
                    UserName = identityUser.UserName,
                    CoreUserDto = _mapper.Map<CoreUserDto>(coreUser)
                };
                
            }
            else
            {
                return null;
            }
        }
    }
}