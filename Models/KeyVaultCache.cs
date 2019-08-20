using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace KeyVaultTest.Models
{
    public static class KeyVaultCache
    {
        private static KeyVaultClient _KeyVaultClient = null;
        public static KeyVaultClient KeyVaultClient
        {
            get
            {
                if (_KeyVaultClient is null)
                    _KeyVaultClient = new KeyVaultClient(AuthenticateVault);
                return _KeyVaultClient;
            }
        }

        private static Dictionary<string, string> SecretsCache = new Dictionary<string, string>();
        public async static Task<string> GetCachedSecret(string secretName)
        {
            if (!SecretsCache.ContainsKey(secretName))
            {
                var secretBundle = await KeyVaultClient
                                        .GetSecretAsync(secretName).ConfigureAwait(false);
                SecretsCache.Add(secretName, secretBundle.Value);
            }
            return SecretsCache.ContainsKey(secretName) ? SecretsCache[secretName] : string.Empty;
        }
        public async static Task<string> AuthenticateVault(string authority, string resource, string scope)
        {
            var aplicationId = ConfigurationManager.AppSettings["AplicationId"];
            var aplicationKey = ConfigurationManager.AppSettings["AplicationKey"];

            var clientCredentials = new ClientCredential(
                aplicationId,
                aplicationKey);

            var authenticationContext = new AuthenticationContext(authority);
            var result = await authenticationContext.AcquireTokenAsync(resource, clientCredentials).ConfigureAwait(false);

            return result.AccessToken;
        }
    }
}