using System;
using carbon.api.Features;
using carbon.core.domain.model.account;
using carbon.core.dtos.account;
using carbon.persistence.features;
using carbon.persistence.interfaces;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace carbon.api.Services
{
    internal static class IdentitySetup
    {
        internal static void InitializeDatabase(IApplicationBuilder app, IConfiguration configuration)
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

                    
                    var writeRepo = new ReadWriteRepository(GetDbContext(configuration.GetConnectionString("ApplicationDatabase")));

                    if (!writeRepo.Table<CoreUser,Guid>().Any())
                    {
                        var adminUserCore = CoreUser.Create(Guid.Parse(adminUser.Id));
                        adminUserCore.Update(new CoreUserDto()
                        {
                            Access = AccessEnum.SuperAdmin,
                            Picture = @"data:image/png;base64,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"
                        });

                        
                        writeRepo.Create<CoreUser,Guid>(adminUserCore);
                        
                        writeRepo.Commit();
                        
                        writeRepo.Dispose();
                    }
                    
                    adminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(adminUser, "Password1@");
                    
                    identityContext.Users.Add(adminUser);
                    identityContext.SaveChanges(); 
                }

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    
                    foreach (var client in IdentityConfig.GetClients(configuration))
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
                    foreach (var resource in IdentityConfig.GetApiResources(configuration))
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
        
        private static DbContext GetDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            var options = optionsBuilder.UseMySql(connectionString);
            return new CoreDbContext(options.Options);
        }
        
    }
}