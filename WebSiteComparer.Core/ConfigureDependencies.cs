using Microsoft.Extensions.DependencyInjection;
using Libs.ImageProcessing;
using Libs.ImageProcessing.Implementation.Comparing;
using WebSiteComparer.Core.ChangesTracker.Implementation;
using WebSiteComparer.Core.Screenshots;
using WebSiteComparer.Core.Screenshots.Implementation;

namespace WebSiteComparer.Core;

public static class ConfigureDependencies
{
    public static IServiceCollection AddWebSiteComparer(
        this IServiceCollection services,
        WebSiteComparerConfiguration configuration )
    {
        services.AddSingleton<WebSiteComparerConfiguration>( configuration );

        services.AddScoped<IScreenshotTaker, ScreenshotTaker>();
        services.AddScoped<IScreenshotRepository, ScreenshotsRepository>();
        services.AddScoped<IWebSitesViewChangesTracker, WebSitesViewChangesTracker>();
            
        services.AddScoped<IImageComparer, CashedBitmapComparer>();

        return services;
    }
}