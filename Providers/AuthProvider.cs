using AppwriteWithBlazor.Helpers;
using AppwriteWithBlazor.Models;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace AppwriteWithBlazor.Providers
{
    public interface IAuthProvider
    {
        Task Login(UserCreateModel loginRequest);
        Task Register(UserCreateModel registerRequest);
        Task Logout();
        Task<CurrentUser> CurrentUserInfo();
    }

    public class AuthProvider : IAuthProvider
    {
        private readonly HttpClient _client;
        private readonly IAppStates _states;

        public AuthProvider(IHttpClientFactory clientFactory, IAppStates states)
        {
            _client = clientFactory.CreateClient("APPWRITE");
            _states = states;
        }

        public async Task<CurrentUser> CurrentUserInfo()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/v1/account/sessions/current");

            request.Headers.TryAddWithoutValidation("Cookie", await _states.GetToken());
            request.Headers.TryAddWithoutValidation("X-Fallback-Cookies", await _states.GetToken());

            HttpResponseMessage response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<CurrentUser>(json, ExtensionMethods.DeserializerSettings);

            return result;
        }

        public async Task Login(UserCreateModel loginRequest)
        {
            var modelItemJson = new StringContent(
                JsonSerializer.Serialize(loginRequest, ExtensionMethods.SerializerSettings),
                encoding: System.Text.Encoding.UTF8,
                Application.Json
            );

            using var httpResponseMessage = await _client.PostAsync(
                "/v1/account/sessions/email",
                modelItemJson
            );

            httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.Headers.TryGetValues("x-fallback-cookies", out IEnumerable<string> values))
            {
                foreach (var value in values)
                {
                    await _states.SetToken(value);
                }
            }
        }

        public async Task Logout()
        {
            await _states.RemoveToken();
        }

        public async Task Register(UserCreateModel registerRequest)
        {
            var modelItemJson = new StringContent(
                JsonSerializer.Serialize(registerRequest, ExtensionMethods.SerializerSettings),
                encoding: System.Text.Encoding.UTF8,
                Application.Json
            );

            using var httpResponseMessage = await _client.PostAsync(
                "/v1/account",
                modelItemJson
            );

            httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}
