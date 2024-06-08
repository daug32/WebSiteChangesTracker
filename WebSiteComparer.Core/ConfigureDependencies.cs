using Microsoft.Extensions.DependencyInjection;
using ImageProcessing;
using ImageProcessing.Implementation.Comparing;
using WebSiteComparer.Core.ChangesTracking;
using WebSiteComparer.Core.Configurations;
using WebSiteComparer.Core.Implementation.ChangesDetecting;
using WebSiteComparer.Core.Implementation.Screenshots;
using WebSiteComparer.Core.Screenshots;

namespace WebSiteComparer.Core;

public static class ConfigureDependencies
{
    public static IServiceCollection AddWebSiteComparer(
        this IServiceCollection services,
        WebSiteComparerConfiguration configuration )
    {
        services.AddSingleton( configuration );

        services.AddScoped<IScreenshotTaker, ScreenshotTaker>();
        services.AddScoped<IScreenshotRepository, ScreenshotsRepository>();
        services.AddScoped<IChangesDetector, ChangesDetector>();
            
        services.AddScoped<IImageComparer, CashedBitmapComparer>();

        return services;
    }
}