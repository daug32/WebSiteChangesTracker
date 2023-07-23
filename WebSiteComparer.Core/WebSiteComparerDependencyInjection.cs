using Microsoft.Extensions.DependencyInjection;
using WebSiteComparer.Core.ImageProcessing;
using WebSiteComparer.Core.ImageProcessing.Implementation.Comparing;
using WebSiteComparer.Core.Screenshots;
using WebSiteComparer.Core.Screenshots.Implementation;
using WebSiteComparer.Core.WebPageProcessing;
using WebSiteComparer.Core.WebPageProcessing.Implementation;

namespace WebSiteComparer.Core
{
    public static class WebSiteComparerDependencyInjection
    {
        public static IServiceCollection AddWebSiteComparer( 
            this IServiceCollection services,
            string screenshotsDirectory )
        {
            services.AddScoped<IImageComparer, CashedBitmapComparer>();
            services.AddScoped<IWebPageScreenshotTaker, WebPageScreenshotTaker>();
            services.AddScoped<IScreenshotRepository, ScreenshotRepository>( _ => new ScreenshotRepository( screenshotsDirectory ) );
            services.AddScoped<IWebSitesViewChangesTracker, WebSitesViewChangesTracker>();

            return services;
        }
    }
}