using System.Threading.Tasks;
using AutoMapper;
using carbon.persistence.interfaces;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace carbon.api.Controllers
{
    [ApiController]
    [Route("api/app/")]
    public class AppController : CarbonAuthenticatedController
    {
        private readonly IUserStore<IdentityUser> _users;
        private readonly IReadWriteRepository _readWriteRepository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public AppController(IUserStore<IdentityUser> users, 
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
        [Route("externalProfile")]
        public async Task<IActionResult> ExternalProfile()
        {
            var vm = await GetUserProfile();

            return Ok(vm);
        }
        
    }
}