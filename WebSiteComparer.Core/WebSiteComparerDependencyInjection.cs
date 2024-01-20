using Microsoft.Extensions.DependencyInjection;
using Libs.ImageProcessing;
using Libs.ImageProcessing.Implementation.Comparing;
using WebSiteComparer.Core.Implementation;
using WebSiteComparer.Core.Screenshots;

namespace WebSiteComparer.Core
{
    public static class WebSiteComparerDependencyInjection
    {
        public static IServiceCollection AddWebSiteComparer( this IServiceCollection services )
        {
            services.AddScoped<IImageComparer, CashedBitmapComparer>();
            services.AddScoped<IScreenshotTaker, ScreenshotTaker>();
            services.AddScoped<IWebSitesViewChangesTracker, WebSitesViewChangesTracker>();

            return services;
        }
    }
}