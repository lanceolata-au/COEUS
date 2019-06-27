using System.Collections.Generic;
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
                    ClientId = "infinity.client",

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
                        "infinity.read",
                        "infinity.write",
                    }
                }
            };
        }
    }
}