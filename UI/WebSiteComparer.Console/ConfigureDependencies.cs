using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebSiteComparer.Console.Services;
using WebSiteComparer.Core;
using WebSiteComparer.Core.WebPageProcessing;

namespace WebSiteComparer.Console;

public static class ConfigureDependencies
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        services.AddScoped( _ => configuration );
        services.AddScoped<WebSiteComparerApplication>();
        services.AddScoped<ILogService, ConsoleLogService>();
        
        string screenshotsDirectory = configuration.GetSection( "ScreenshotsDirectory" ).Value!;
        services.AddWebSiteComparer( screenshotsDirectory );
        
        return services;
    }
}