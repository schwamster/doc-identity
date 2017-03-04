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

        public const string tenant = "tenant";
        public const string tenantId = "tenant_id";

        public const string tenantName = "tenant_name";

        public const string role = "role";

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
                new IdentityResource(tenant, "Tenant Information", new List<string>{tenantId, tenantName}),
                new IdentityResource(role, "Role", new List<string>{"role"})
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
                    ClientId = "service_to_service",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret(config["Identity:ServiceToServiceSecret"].Sha256())
                    },
                    AllowedScopes = 
                    { 
                        docStackAppApi,
                        docStoreApi
                    }
                },
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
                        tenant,
                        role,

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
                        tenant,
                        role,

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
                        new Claim("role", "admin"),
                        new Claim("tenant_id", "1337"),
                        new Claim("tenant_name", "internal")
                    }
                }
            };
        }
    }
}
