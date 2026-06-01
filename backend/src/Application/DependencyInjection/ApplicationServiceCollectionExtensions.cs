using Application.Games.Contracts;
using Application.Games.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGameSearchService, GameSearchService>();
        return services;
    }
}
