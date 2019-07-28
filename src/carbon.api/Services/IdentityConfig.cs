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
                    UserClaims = new List<string> {"user","admin","master"}
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "infinity",
                    DisplayName = "Infinity Paper",
                    Description = "Infinity Paper, a base API",
                    UserClaims = new List<string> {"user","admin","master"},
                    ApiSecrets = new List<Secret> {new Secret("internalRuntimeSecret".Sha256())},
                    Scopes = new List<Scope>
                    {
                        new Scope("infinity.read"),
                        new Scope("infinity.write")
                            
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
                    ClientId = "carbon.internal",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256()) //TODO this should save in the DB, also the word secret isn't very
                    },

                    // scopes that client has access to
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "carbon.read",
                        "carbon.write",
                    }
                },
                new Client
                {
                    ClientId = "carbon.app", 
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris =           { "http://localhost:6443/callback" },
                    PostLogoutRedirectUris = { "http://localhost:6443/" },
                    AllowedCorsOrigins =     { "http://localhost:6443" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }
    }
}