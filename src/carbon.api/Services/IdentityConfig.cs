using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

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

        public static IEnumerable<ApiResource> GetApiResources()
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

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "carbon.app", 
                    ClientName = "Carbon Angular APP",
                    
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 330,
                    IdentityTokenLifetime = 30,
                    
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {
                        "https://localhost:6443/callback"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:6443/"
                    },
                    AllowedCorsOrigins =
                    {
                        "https://localhost:6443"
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