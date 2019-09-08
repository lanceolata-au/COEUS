// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Alterations by Owen Holloway for the Carbon project 

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using carbon.api.Models.Account;
using carbon.core.domain.model.account;
using carbon.core.dtos.account;
using carbon.persistence.interfaces;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace carbon.api.Controllers.Account
{
    /// <summary>
    ///     This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    ///     The login service encapsulates the interactions with the user data store. This data store is in-memory only and
    ///     cannot be used for production!
    ///     The interaction service provides a way for the UI to communicate with identityserver for validation and context
    ///     retrieval
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : CarbonController
    {
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IUserStore<IdentityUser> _users;
        private readonly IReadWriteRepository _readWriteRepository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IUserStore<IdentityUser> users,
            IReadWriteRepository readWriteRepository,
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _users = users;
            _readWriteRepository = readWriteRepository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        /// <summary>
        ///     Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User?.IsAuthenticated() != null && (bool)User?.IsAuthenticated())
            {
                return Redirect("/Account/Details");
            }
            
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            return View(vm);
        }

        /// <summary>
        ///     Handle post back from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button.Equals("cancel"))
            {
                if (context == null) return Redirect("~/");
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (await _clientStore.IsPkceClientAsync(context.ClientId))
                    return View("Redirect", new RedirectViewModel {RedirectUrl = model.ReturnUrl});

                return Redirect(model.ReturnUrl);

                // since we don't have a valid context, then we just go back to the home page
            }
            
            if (ModelState.IsValid)
            {
                
                var user = _users.FindByNameAsync(model.Email, new CancellationToken(false)).Result;
                if (user != null)
                {
                    var checkedPassword = new PasswordHasher<IdentityUser>().VerifyHashedPassword(user, user.PasswordHash, model.Password);
                    // validate username/password against store
                    if (checkedPassword == PasswordVerificationResult.Success || checkedPassword == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        
                        await _events.RaiseAsync(new UserLoginSuccessEvent(user.Email, user.Id, user.UserName));
                        var props = new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                            };
    
                        // issue authentication cookie with subject ID and username
                        await HttpContext.SignInAsync(user.Id, user.UserName, props);
    
                        if (context != null)
                        {
                            if (await _clientStore.IsPkceClientAsync(context.ClientId))
                                return View("Redirect", new RedirectViewModel {RedirectUrl = model.ReturnUrl});
    
                            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                            return Redirect(model.ReturnUrl);
                        }
    
                        // request for a local page
                        if (Url.IsLocalUrl(model.ReturnUrl))
                            return Redirect(model.ReturnUrl);
                        if (string.IsNullOrEmpty(model.ReturnUrl))
                            return Redirect("~/");
                        throw new Exception("invalid return URL");
                    }
                }


                await _events.RaiseAsync(new UserLoginFailureEvent(model.Email, "invalid credentials"));
                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> LoginToken([FromBody] LoginInputModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid login model");
            
            var user = _users.FindByNameAsync(model.Email, new CancellationToken(false)).Result;
            
            if (user == null) return BadRequest("Invalid login model");
            
            var checkedPassword = new PasswordHasher<IdentityUser>().VerifyHashedPassword(user, user.PasswordHash, model.Password);
            // validate username/password against store
            if (checkedPassword == PasswordVerificationResult.Success ||
                checkedPassword == PasswordVerificationResult.SuccessRehashNeeded)
            {
                
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7qxSC8eWmhuN7UQ5owt9O29ci5AGRCy7")); //TODO change this to be generated and stored in db
                
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Role, "NAA")
                };
                
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5443",
                    audience: "https://localhost:6443",
                    claims: claims,
                    expires: DateTime.Now.AddDays(15),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                
                return Ok(new { Token = tokenString });
                
            }
            else
            {
                return Unauthorized();
            }
            
        }

        /// <summary>
        ///     Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var referer = Request.Headers["Referer"].ToString();
            var logoutId = User.GetSubjectId();
            
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            await Logout(vm);

            return Redirect(referer);
        }

        /// <summary>
        ///     Handle logout page post back
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

            }
            else
            {
                return Redirect("/");
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                var url = Url.Action("Logout", new {logoutId = vm.LogoutId});

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties {RedirectUri = url}, vm.ExternalAuthenticationScheme);
            }
            else
            {
                return Redirect("/");
            }

        }

        [HttpGet]
        [AllowAnonymous]
#pragma warning disable 1998
        public async Task<IActionResult> Register()
#pragma warning restore 1998
        {
            if (User?.IsAuthenticated() != null && (User?.IsAuthenticated() == true))
            {
                return Redirect("/Account/Profile");
            }
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterInputModel inputModel)
        {
            var user = _users.FindByNameAsync(inputModel.Email, new CancellationToken(false)).Result;
            
            if (user == null && ModelState.IsValid)
            {
                var newUser = new IdentityUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = inputModel.Email,
                    UserName = inputModel.Email,
                    NormalizedUserName = inputModel.Email,
                    EmailConfirmed = true
                };

                newUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(newUser, inputModel.Password);
                
                await _users.CreateAsync(newUser, default(CancellationToken));
                
                var newCoreUser = CoreUser.Create(Guid.Parse(newUser.Id));
                newCoreUser.Update(new CoreUserDto()
                {
                    Access = AccessEnum.Standard,
                    Picture = @"data:image/png;base64,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"
                });
                
                _readWriteRepository.Create<CoreUser,Guid>(newCoreUser);

                return Redirect("/Account/Login");
            }
            
            ModelState.AddModelError("",AccountOptions.UserAlreadyExistsErrorMessage);
            return Redirect("/Account/Register");
        }
        
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (User?.IsAuthenticated() == true)
            {
                var vm = await BuildProfileViewModelAsync();
            
                return View(vm);
            }
            else
            {
                return Redirect("/");
            }
            
        }
        
        [HttpGet]
        public async Task<IActionResult> Password()
        {
            if (User?.IsAuthenticated() == true)
            {
                var vm = await BuildPasswordUpdateViewModelAsync();
            
                return View(vm);
            }
            else
            {
                return Redirect("/");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password(PasswordInputModel inputModel)
        {
            if (User?.IsAuthenticated() == true)
            {
                var identityUser = await _users.FindByIdAsync(User.Identity.GetSubjectId(), new CancellationToken());
                
                var vm = await BuildPasswordUpdateViewModelAsync();

                var checkedPassword = new PasswordHasher<IdentityUser>().VerifyHashedPassword(identityUser, identityUser.PasswordHash, inputModel.CurrentPassword);

                if (checkedPassword == PasswordVerificationResult.Success ||
                    checkedPassword == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    if (inputModel.NewPassword.Equals(inputModel.NewPasswordVerify))
                    {
                        identityUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(identityUser, inputModel.NewPassword);
                        await _users.UpdateAsync(identityUser, new CancellationToken());
                        return Redirect("/Account/Profile");
                    }

                    
                    ModelState.AddModelError("", "Password verify does not match new password.");
                    return View(vm);
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(inputModel.UserName, "Password update failure. Incorrect password."));
                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);
                return View(vm);

            }

            return Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> Application()
        {
            if (User?.IsAuthenticated() == true)
            {
                var vm = await BuildPasswordUpdateViewModelAsync();
            
                return View(vm);
            }
            else
            {
                return Redirect("/");
            }
        }
        
        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
                return new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Email = context.LoginHint,
                    ExternalProviders = new[] {new ExternalProvider {AuthenticationScheme = context.IdP}}
                };

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null ||
                            x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName,
                                StringComparison.OrdinalIgnoreCase)
                )
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                        providers = providers.Where(provider =>
                            client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                }
            }

            return new LoginViewModel
            {
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Email = model.Email;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel {LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt};

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null) vm.LogoutId = await _interaction.CreateLogoutContextAsync();

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }

            }

            return vm;
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
        
        private async Task<PasswordInputModel> BuildPasswordUpdateViewModelAsync()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                var identityUser = await _users.FindByIdAsync(User.Identity.GetSubjectId(), new CancellationToken());
                
                var coreUser = _readOnlyRepository.GetById<CoreUser, Guid>(Guid.Parse(identityUser.Id));
                
                return new PasswordInputModel()
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