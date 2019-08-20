using System.Configuration;
using System.Threading.Tasks;

namespace KeyVaultTest.Models
{
    public class GetSecret
    {
        public static async Task<string> SqlConnectionString() => await KeyVaultCache.GetCachedSecret(ConfigurationManager.AppSettings["ConnectionString"]);
    }
}