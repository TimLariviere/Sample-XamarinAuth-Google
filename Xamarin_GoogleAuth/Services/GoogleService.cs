using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Xamarin_GoogleAuth.Services
{
    public class GoogleService
    {
        public async Task<string> GetEmailAsync(string tokenType, string accessToken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
            var json = await httpClient.GetStringAsync("https://www.googleapis.com/userinfo/email?alt=json");
            var email = JsonConvert.DeserializeObject<GoogleEmail>(json);
            return email.Data.Email;
        }
    }
}
