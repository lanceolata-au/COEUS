using System;
using carbon.api.Features;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace carbon.api.Services
{
    internal static class IdentitySetup
    {
        internal static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var  identityContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
                identityContext.Database.Migrate();

                if (!identityContext.Users.Any())
                {
                    var adminUser = new IdentityUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "owen.holloway101+infinity@gmail.com",
                        UserName = "admin@zeryter.xyz",
                        NormalizedUserName = "admin@zeryter.xyz",
                        EmailConfirmed = true
                    };

                    adminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(adminUser, "Password1@");
                    
                    identityContext.Users.Add(adminUser);
                    identityContext.SaveChanges(); 
                }

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    
                    foreach (var client in IdentityConfig.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityConfig.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityConfig.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}