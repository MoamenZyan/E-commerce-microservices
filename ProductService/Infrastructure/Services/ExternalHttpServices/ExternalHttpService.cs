using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ProductService.Infrastructure.Services.ExternalHttpServices
{
    public class ExternalHttpService
    {
        private readonly HttpClient _httpClient;
        public ExternalHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<List<string>?> GetUserRoles(string userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"http://localhost:8080/api/auth/roles?userId={userId}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<string>>(content);
        }
    }
}
