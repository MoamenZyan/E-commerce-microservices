using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.EntityFrameworkCore;
using ReviewService.Infrastructure.Data;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.SigningKeys;
using System.Security.Cryptography;
using MediatR;
using ReviewService.Controllers;

namespace ReviewService.Infrastructure.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddSerivces(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
                .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddMediatR(typeof(ReviewController));

        // Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            RSA rsa = RSA.Create();
            rsa.ImportFromPem(SigningKeys.GetPublicKey());
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                SaveSigninToken = true,
                ValidIssuer = "Backend",
                ValidAudience = "Frontend",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa)
            };
        });

        // Serilog configuration
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                IndexFormat = "logs-{0:yyyy.MM.dd}",
                AutoRegisterTemplate = true,
                NumberOfShards = 2,
                NumberOfReplicas = 1
            })
            .CreateLogger();

        return services;
    }
}
