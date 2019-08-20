using KeyVaultTest.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KeyVaultTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public async Task<ActionResult> Contact()
        {
            //var keyVaultClient = new KeyVaultClient(AuthenticateVault);
            //var result = await keyVaultClient
            //    .GetSecretAsync("https://devcadenasconexion.vault.azure.net/secrets/conexionarennas/1ef2941401e74ba6b0a5aedb9b3da48a");


            var resultSecret = GetSecret.SqlConnectionString().ConfigureAwait(true).GetAwaiter().GetResult();
            ViewBag.Message = $"Your contact page.{resultSecret}";

            //var resultSecretTwo = await GetSecret.SqlConnectionString();

            return View();
        }
        public async Task<string> AuthenticateVault(string authority,string resource,string scope)
        {
            var aplicationId = ConfigurationManager.AppSettings["AplicationId"];
            var aplicationKey = ConfigurationManager.AppSettings["AplicationKey"];

            var clientCredentials = new ClientCredential(
                aplicationId,
                aplicationKey);

            var authenticationContext = new AuthenticationContext(authority);
            var result = await authenticationContext.AcquireTokenAsync(resource, clientCredentials);

            return result.AccessToken;
        }
    }
}