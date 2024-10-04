using Newtonsoft.Json;
using Shared.DTOs;

namespace NotificationService.Infrastructure.Services.ExternalHttpServices
{
    public class ExternalHttpService
    {
        private readonly HttpClient _httpClient;
        public ExternalHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto?> GetProduct(string productId)
        {
            var url = $"http://localhost:5190/api/product?productId={productId}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            ProductDto? content = JsonConvert.DeserializeObject<ProductDto>(await response.Content.ReadAsStringAsync());
            return content;
        }
    }
}
