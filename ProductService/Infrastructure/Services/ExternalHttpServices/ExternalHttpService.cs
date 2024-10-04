using Newtonsoft.Json;
using Shared.Entities;
using System.Net.Http.Headers;
using System.Text;

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


        public async Task<List<Review>> GetProductReviews(Guid productId)
        {
            var url = $"http://localhost:5012/api/review/{productId}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Review>>(content)!;
        }
        public async Task<Dictionary<Guid, List<Review>>> GetProductsReviews(List<Guid> productIds)
        {
            var url = "http://localhost:5012/api/review/all";
            var content = new StringContent(
                JsonConvert.SerializeObject(new {ProductsIds = productIds}),
                Encoding.UTF8,
                "application/json"
            );

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            Console.WriteLine(response.StatusCode);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<Guid, List<Review>>>(responseContent)!;
        }
    }
}
