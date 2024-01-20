using System.Threading;
using System.Threading.Tasks;
using Libs.ImageProcessing;
using Libs.Microsoft.Playwright.PageLoading;
using Microsoft.Extensions.Logging;

namespace WebSiteComparer.Core.Implementation
{
    internal class WebSitesViewChangesTracker : IWebSitesViewChangesTracker
    {
        private readonly IImageComparer _imageComparer;
        private readonly ILogger _logger;

        public WebSitesViewChangesTracker( 
            IImageComparer imageComparer,
            ILogger<WebSitesViewChangesTracker> logger )
        {
            _logger = logger;
            _imageComparer = imageComparer;
        }

        public Task CheckForViewChanges(
            WebsiteConfiguration configuration,
            CancellationToken token )
        {
            var parallelOptions = new ParallelOptions();

            if ( token != default )
            {
                parallelOptions.CancellationToken = token;
            }

            return Parallel.ForEachAsync(
                configuration.Urls,
                parallelOptions,
                async ( url, _ ) => await CheckForViewChanges(
                    url,
                    configuration.ScreenshotWidth,
                    configuration.LoadConfiguration ) );
        }

        private async Task CheckForViewChanges(
            string url,
            int screenshotWidth,
            PageLoadingOptions? loadingOptions = null )
        {
            // _logger.Log( LogLevel.Information, $"Checking for changes: {url}" );
            // 
            // Task<CashedBitmap> takeSiteScreenshotTask;
            // try
            // {
            //     _logger.Log( LogLevel.Information, $"Taking a screenshot: {url}" );
            //     .TakeScreenshotAsync(
            //         url,
            //         screenshotWidth,
            //         loadingOptions );
            // }
            // catch ( Exception ex )
            // {
            //     var message = $"An error occured while trying to process site. Url: {url}.\n{ex}";
            //     _logger?.Error( message );
            //     return;
            // }
            // 
            // Task<CashedBitmap> getPreviousSiteScreenshotTask;
            // try
            // {
            //     _logger.Log( LogLevel.Information, $"Loading prev state: {url}" );
            //     getPreviousSiteScreenshotTask = _screenshotRepository.GetPreviousVersion( url );
            // }
            // catch ( Exception ex )
            // {
            //     var message = $"An error occured while trying to get older version of site. Url: {url}\n{ex}";
            //     _logService?.Error( message );
            //     return;
            // }
            //
            // _logger.Log( LogLevel.Information, $"Comparing: {url}" );
            // ImageComparingResult compareResult = await _imageComparer.CompareAsync(
            //     await takeSiteScreenshotTask,
            //     await getPreviousSiteScreenshotTask );
        }
    }
}