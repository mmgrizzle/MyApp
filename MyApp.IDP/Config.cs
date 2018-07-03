using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyApp.IDP
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "mare",
                    Password = "mare",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Mare"),
                        new Claim("family_name", "Grizzle"),
                        new Claim("address", "1 Main Street"),
                        new Claim("role", "Admin")
                    }
                },

                new TestUser
                {
                    SubjectId = "2",
                    Username = "david",
                    Password = "david",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "David"),
                        new Claim("family_name", "Henley"),
                        new Claim("address", "2 West Road"),
                        new Claim("role", "Sales")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string> {"role"})
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("myapi", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName= "My App",
                    ClientId = "myappclient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44371/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44371/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "myapi"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }

                }
            };
        }
    }
}
