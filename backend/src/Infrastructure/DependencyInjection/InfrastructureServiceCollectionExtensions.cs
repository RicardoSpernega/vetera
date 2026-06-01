using Application.Games.Contracts;
using Infrastructure.Caching;
using Infrastructure.Configuration;
using Infrastructure.Rawg;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RawgOptions>(configuration.GetSection(RawgOptions.SectionName));
        services.AddMemoryCache();

        services.AddHttpClient<RawgGateway>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<RawgOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(20);
        });

        services.AddScoped<IRawgGateway, RawgGateway>();
        services.AddSingleton<IGameSearchCache, GameSearchMemoryCache>();

        return services;
    }
}
