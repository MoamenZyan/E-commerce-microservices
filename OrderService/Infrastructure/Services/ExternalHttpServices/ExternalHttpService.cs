using Newtonsoft.Json;
using OrderService.Domain;
using Shared.Entities;
using System.Net.Http.Headers;
using System.Text;

namespace OrderService.Infrastructure.Services.ExternalHttpServices
{
    public class ExternalHttpService
    {
        private readonly HttpClient _httpClient;
        public ExternalHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Cart?> GetUserCart(Guid userId, string token)
        {
            var url = $"http://localhost:5069/api/cart?userId={userId}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Cart>(content);
        }

        public async Task<List<Product>?> GetProductsInfo(List<Guid> ProductsIds)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(ProductsIds),
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage response = await _httpClient.PostAsync($"http://localhost:5190/api/product/info", content);
            string responseData = await response.Content.ReadAsStringAsync();
            List<Product>? products = JsonConvert.DeserializeObject<List<Product>>(responseData);
            return products;
        }


        public async Task<Checkout> CreateCheckout(object obj)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(obj),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", "Very Very Secret API KEY :)");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5127/api/payment")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            var jsonContent = await response.Content.ReadAsStringAsync();

            var dynamicObj = JsonConvert.DeserializeObject<dynamic>(jsonContent)!;
            return JsonConvert.DeserializeObject<Checkout>(JsonConvert.SerializeObject(dynamicObj));
        }
    }
}
