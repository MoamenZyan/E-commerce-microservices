using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Entities;
using System.Text;
using System.Net.Http.Headers;
using Shared.DTOs;

namespace UserService.Infrastructure.Services
{
    public class ExternalHttpService
    {
        private readonly HttpClient _httpClient;
        public ExternalHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Product>?> GetUserProducts(Guid userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5190/api/product/all/{userId}");

            string content = await response.Content.ReadAsStringAsync();

            List<Product>? products = JsonConvert.DeserializeObject<List<Product>>(content);
            return products;
        }

        public async Task<Cart?> GetUserCart(Guid userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5069/api/cart?userId={userId}");
            
            string content = await response.Content.ReadAsStringAsync();
            Cart? cart = JsonConvert.DeserializeObject<Cart>(content);
            return cart;
        }

        public async Task<List<dynamic>?> GetProductsInfo(List<Guid> ProductsIds)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(ProductsIds),
                Encoding.UTF8,
                "application/json"
            );
            foreach ( var product in ProductsIds )
            {
                Console.WriteLine(product);
            }

            HttpResponseMessage response = await _httpClient.PostAsync($"http://localhost:5190/api/product/info", content);
            string responseData = await response.Content.ReadAsStringAsync();
            List<dynamic>? products = JsonConvert.DeserializeObject<List<dynamic>>(responseData);
            return products;
        }
    }
}
