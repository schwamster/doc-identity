using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace doc_identity
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("doc-stack-app-api", "doc-stack-app-api")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration config)
        {
            var docStackAppHost = config["Identity:DocStackAppHost"];
            return new List<Client>
            {
                new Client
                {
                    ClientId = "doc-stack-app",
                    ClientName = "doc-stack-app",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    

                    RedirectUris =           { $"{docStackAppHost}/callback" },
                    PostLogoutRedirectUris = { $"{docStackAppHost}/home" },
                    AllowedCorsOrigins =     { $"{docStackAppHost}" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "doc-stack-app-api"
                    }
                }
            };
        }

        public static List<InMemoryUser> GetUsers(IConfiguration config)
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "1",
                    Username = "admin",
                    Password = config["Identity:AdminPassword"],

                    Claims = new []
                    {
                        new Claim("name", "admin")
                    }
                }
            };
        }
    }
}
