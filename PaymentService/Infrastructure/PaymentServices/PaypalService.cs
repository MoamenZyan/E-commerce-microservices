using PaymentService.Application.Features.Commands.CreatePaymentOrder;
using PaymentService.Application.Interfaces;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PaymentService.Infrastructure.Data;
using Shared.Entities;
using PaymentService.Domain;
using PaymentService.Application.Responses;
using Shared.Enums;

namespace PaymentService.Infrastructure.PaymentServices
{
    public class PaypalService : IPayment
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public PaypalService(HttpClient httpClient, IConfiguration configuration, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }
        public async Task<dynamic> Pay(CreatePaymentOrderCommand command)
        {
            var token = await GetAccessToken();
            return await CreatePaypalPaymentOrder(token, command);
        }
        public async Task<PaypalOrder> CreatePaypalPaymentOrder(string token, CreatePaymentOrderCommand command)
        {
            var url = "https://api.sandbox.paypal.com/v2/checkout/orders/";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var itemTotal = command.Products.Sum(x => x.Price * x.Quantity);

            var items = command.Products.Select(x => new
            {
                name = x.Name,
                description = x.Description,
                quantity = x.Quantity.ToString(),
                unit_amount = new 
                {
                    currency_code = "USD",
                    value = x.Price.ToString(),
                }
            });

            var body = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = "USD",
                            value = itemTotal.ToString("F2"),
                            breakdown = new
                            {
                                item_total = new
                                {
                                    currency_code = "USD",
                                    value = itemTotal.ToString("F2"),
                                }
                            }
                        },
                        items = items
                    }
                },
            };


            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };
            
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            PaypalOrder paypalOrder = JsonConvert.DeserializeObject<PaypalOrder>(content)!;
            return paypalOrder;
        }
        private async Task<string> GetAccessToken()
        {
            var token = _context.PaypalTokens.FirstOrDefault();
            if (token == null)
            {
                var newToken = await GetNewAccessToken();
                if (newToken == null)
                    throw new ArgumentNullException(nameof(newToken));

                PaypalToken paypalToken = new PaypalToken()
                {
                    Id = Guid.NewGuid(),
                    Token = newToken,
                    ExpireDate = DateTime.Now.AddHours(3)
                };
                await _context.PaypalTokens.AddAsync(paypalToken);
                token = paypalToken;
            }
            else
            {
                if (token.ExpireDate <= DateTime.Now)
                {
                    var newToken = await GetNewAccessToken();
                    if (newToken == null)
                        throw new ArgumentNullException(nameof(newToken));

                    token.Token = newToken;
                    token.ExpireDate = DateTime.Now.AddHours(3);
                    _context.PaypalTokens.Update(token);
                }
            }
            await _context.SaveChangesAsync();
            return token.Token;
        }
        private async Task<string?> GetNewAccessToken()
        {
            var url = "https://api-m.sandbox.paypal.com/v1/oauth2/token";
            var clientId = _configuration["Paypal:ClientId"];
            var secretId = _configuration["Paypal:SecretId"];

            if (clientId == null || secretId == null)
                throw new ArgumentNullException("Paypal ClientId or SecretId is null");

            var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{secretId}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var requestBody = new Dictionary<string, string>()
            {
                {"grant_type" , "client_credentials"}
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(requestBody)
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                return obj!.access_token;
            }

            return null;
        }

        public async Task<OrderCheckResponse?> CheckOrder(string orderId)
        {
            var url = $"https://api.sandbox.paypal.com/v2/checkout/orders/{orderId}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<OrderCheckResponse>(content);
                if (order!.Status == Convert.ToString(OrderStatus.APPROVED))
                {
                    url = $"https://api.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture";
                    response = await _httpClient.PostAsync(url, new StringContent("{}", Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<OrderCheckResponse>(content);
                    }
                    else
                    {
                        return null;
                    }
                }

                return order;
            }
            return null;
        }
    }
}
