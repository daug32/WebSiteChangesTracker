using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebSiteComparer.Console.Commands;
using WebSiteComparer.Core;
using WebSiteComparer.Core.Configurations;
using WebSiteComparer.UseCases;

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

        AddWebSiteComparer( services, configuration );

        return services;
    }

    private static void AddWebSiteComparer( IServiceCollection services, IConfiguration configuration )
    {
        var websiteComparerConfig = configuration
            .GetSection( "WebSiteComparerConfiguration" )
            .Get<WebSiteComparerConfiguration>();

        if ( websiteComparerConfig is null )
        {
            throw new ArgumentException( $"Couldn't get configuration: {nameof( WebSiteComparerConfiguration )}" );
        }

        websiteComparerConfig.Validate();

        services.AddWebSiteComparer( websiteComparerConfig );
        services.AddWebSiteComparerUseCases();
    }
}