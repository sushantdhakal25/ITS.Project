using IdentityServer4;
using IdentityServer4.Models;

namespace ITS.Application.IdentityServer
{
    public class Configuration
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("itsApi", "ITS Identity Server Api"){  ApiSecrets = { new Secret("securePassword".Sha256()) }}
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
             {
                 new ApiScope(name: "itsApi","ITS Identity Server Api")
             };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "itsWebClient",
                    ClientName = "ITS Web Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireConsent = false,
                    AccessTokenLifetime = 43200,
                    ClientSecrets =
                    {
                        new Secret("securePassword".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "itsApi"}  ,
                    AllowOfflineAccess =true
                }
                  
             };
        }
    }
}
