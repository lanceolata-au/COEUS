// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using carbon.api.Models.Account;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    public class AccountController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IUserStore<IdentityUser> _users;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IUserStore<IdentityUser> users)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _users = users;
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

            if (vm.IsExternalLoginOnly)
                return RedirectToAction("Challenge", "External", new {provider = vm.ExternalLoginScheme, returnUrl});

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


        /// <summary>
        ///     Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var logoutId = User.GetSubjectId();
            
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            await Logout(vm);

            return View();
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
        public async Task<IActionResult> Register()
        {
            if (User?.IsAuthenticated() != null || (User?.IsAuthenticated() == true))
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
                
                return new ProfileViewModel()
                {
                    UserName = identityUser.UserName
                };
                
            }
            else
            {
                return null;
            }
        }
        
    }
}