using Newtonsoft.Json;
using Shared.Entities;

namespace NotificationService.Infrastructure.Services.ExternalHttpServices
{
    public class ExternalHttpService
    {
        private readonly HttpClient _httpClient;
        public ExternalHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Product?> GetProduct(string productId)
        {
            var url = $"http://localhost:5190/api/product?productId={productId}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Product? content = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
            return content;
        }
    }
}
