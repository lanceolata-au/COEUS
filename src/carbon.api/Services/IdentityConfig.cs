using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace carbon.api.Services
{
    public static class IdentityConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string>
                    {
                        "user",
                        "admin",
                        "master"
                    }
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources(IConfiguration configuration)
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "carbon.api",
                    DisplayName = "carbon API",
                    Description = "carbon, a base API",
                    UserClaims = new List<string>
                    {
                        "user",
                        "admin",
                        "master"
                    },
                    ApiSecrets = new List<Secret> {new Secret("thisIsABadSecretWeNeedToChangeIt".Sha256())}, //TODO read the secret from db and change it
                    Scopes = new List<Scope>
                    {
                        new Scope("carbon.read"),
                        new Scope("carbon.write")
                            
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "carbon.app", 
                    ClientName = "Carbon Angular APP",
                    
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 604800,
                    IdentityTokenLifetime = 604800,
                    
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {
                        configuration.GetSection("Hosts").GetSection("APPFqdn").Value + "/callback"
                    },
                    PostLogoutRedirectUris =
                    {
                        configuration.GetSection("Hosts").GetSection("APPFqdn").Value + "/"
                    },
                    AllowedCorsOrigins =
                    {
                        configuration.GetSection("Hosts").GetSection("APPFqdn").Value
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "carbon.read",
                        "carbon.write"
                    }
                },
                new Client
                {
                    ClientId = "swagger", 
                    ClientName = "Swagger API Tool",
                    
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 604800,
                    IdentityTokenLifetime = 604800,
                    
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {
                        configuration.GetSection("Hosts").GetSection("APIFqdn").Value + "/swagger/oauth2-redirect.html"
                    },
                    PostLogoutRedirectUris =
                    {
                        configuration.GetSection("Hosts").GetSection("APIFqdn").Value + "/swagger"
                    },
                    AllowedCorsOrigins =
                    {
                        configuration.GetSection("Hosts").GetSection("APIFqdn").Value
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "carbon.read",
                        "carbon.write"
                    }
                }
            };
        }
    }
}