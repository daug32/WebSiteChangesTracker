using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebSiteComparer.Console.Commands;
using WebSiteComparer.Core;

namespace WebSiteComparer.Console;

public static class ConfigureDependencies
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        services.AddScoped( _ => configuration );
        services.AddScoped<WebSiteComparerApplication>();
        services.AddScoped<CommandBuilder>();
        services.AddWebSiteComparer();
        
        return services;
    }
}