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
        public const string docStackAppApi = "doc-stack-app-api";
        public const string docStoreApi = "doc-store";

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(docStackAppApi, docStackAppApi),
                new ApiResource(docStoreApi, docStoreApi)
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
            var docStackAppApiHost = config["Identity:DocStackAppApiHost"];
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
                        docStackAppApi,
                        docStoreApi
                    }
                },
                new Client
                {
                    ClientId = "doc-stack-app-api-swagger",
                    ClientName = "doc-stack-app-api-swagger",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    

                    RedirectUris =           { $"{docStackAppApiHost}/swagger/o2c.html" },
                    PostLogoutRedirectUris = { $"{docStackAppApiHost}/swagger/index.html" },
                    AllowedCorsOrigins =     { $"{docStackAppApiHost}" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        docStackAppApi,
                        docStoreApi
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
                        new Claim("name", "admin"),
                        new Claim("client", "1337")
                    }
                }
            };
        }
    }
}
