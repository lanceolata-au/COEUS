using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using carbon.api.Models.Account;
using carbon.core.domain.model.account;
using carbon.core.dtos.account;
using carbon.persistence.interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace carbon.api.Controllers
{
    [SecurityHeaders]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CarbonAuthenticatedController : Controller
    {
        private readonly IUserStore<IdentityUser> _users;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        
        public CarbonAuthenticatedController(IUserStore<IdentityUser> users,
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _users = users;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }
        
        protected async Task<ProfileViewModel> GetUserProfile()
        {
            if (User?.Identity.IsAuthenticated != true) return null;
            
            var claims = User.Identities.FirstOrDefault()?.Claims;
            
            var userIdentity = claims.First(c =>
                c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;
            
            var identityUser = await _users.FindByIdAsync(userIdentity, new CancellationToken());
            
            var coreUser = _readOnlyRepository.GetById<CoreUser, Guid>(Guid.Parse(identityUser.Id));
            
            return new ProfileViewModel()
            {
                UserName = identityUser.UserName,
                CoreUserDto = _mapper.Map<CoreUserDto>(coreUser)
            };
            
        }
    }
}    