using System;
using System.Threading;
using System.Threading.Tasks;
using carbon.core.domain.model.account;
using carbon.core.dtos.account;
using carbon.persistence.interfaces;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace carbon.api.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IUserStore<IdentityUser> _users;
        private readonly IReadWriteRepository _readWriteRepository;
        private readonly IReadOnlyRepository _readOnlyRepository;

        public AdminController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IUserStore<IdentityUser> users,
            IReadWriteRepository readWriteRepository,
            IReadOnlyRepository readOnlyRepository)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _users = users;
            _readWriteRepository = readWriteRepository;
            _readOnlyRepository = readOnlyRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> Main()
        {
            if (HasAccessLevel(AccessEnum.SuperAdmin))
            {
                return View();
            }
            else
            {
                return Redirect("/");
            }
            
        }
        
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            if (HasAccessLevel(AccessEnum.SuperAdmin))
            {
                return View();
            }
            else
            {
                return Redirect("/");
            }
            
        }
        
        /*****************************************/
        /* helper APIs for the Admin Controller  */
        /*****************************************/
        
        private bool HasAccessLevel(AccessEnum accessLevel)
        {
            if (User?.IsAuthenticated() == null || !(bool) User?.IsAuthenticated()) return false;
            
            var identityUser = _users.FindByIdAsync(User.Identity.GetSubjectId(), new CancellationToken()).Result;

            var coreUser = _readOnlyRepository.GetById<CoreUser, Guid>(Guid.Parse(identityUser.Id));

            return coreUser.Access >= accessLevel;

        }
        
    }    
}