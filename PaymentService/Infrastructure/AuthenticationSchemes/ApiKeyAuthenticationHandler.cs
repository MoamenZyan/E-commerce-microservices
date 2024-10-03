using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace PaymentService.Infrastructure.AuthenticationSchemes
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string ApiKeyHeaderName = "X-API-KEY";
        private const string ValidApiKey = "Very Very Secret API KEY :)";
        public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(ApiKeyHeaderName))
                return Task.FromResult(AuthenticateResult.Fail("API Key not found"));
            
            var apiKey = Request.Headers[ApiKeyHeaderName];
            if (apiKey != ValidApiKey)
                return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));


            var identity = new ClaimsIdentity([], Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
        }
    }
}
