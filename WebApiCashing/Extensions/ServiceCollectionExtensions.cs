using Cachier.net.Data;
using Cachier.net.Services;
using Cachier.net.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace Cachier.net.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Redis
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisConfiguration = configuration.GetConnectionString("RedisCacheUrl");
            return ConnectionMultiplexer.Connect(redisConfiguration);
        });
        services.AddScoped<ICacheService, CacheService>();
        // Add controllers
        services.AddControllers();
        // Configure Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "RedisCaching",
                Version = "v1"
            });
        });

        // Configure Entity Framework
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        // Configure Redis caching
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisCacheUrl");
            options.InstanceName = "SampleInstance";
        });

        // Register services
    }

    public static void UseApplicationConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RedisCaching v1"));
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}