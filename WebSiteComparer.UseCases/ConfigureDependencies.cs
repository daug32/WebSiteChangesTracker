using Microsoft.Extensions.DependencyInjection;

namespace WebSiteComparer.UseCases;

public static class ConfigureDependencies
{
    public static IServiceCollection AddWebSiteComparerUseCases( this IServiceCollection services )
    {
        services.AddScoped<CheckForChangesCommandHandler>();
        services.AddScoped<UpdateScreenshotsCommandHandler>();

        return services;
    }
}