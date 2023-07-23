using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSiteComparer.Core.ImageProcessing;
using WebSiteComparer.Core.ImageProcessing.Models;
using WebSiteComparer.Core.Screenshots;
using WebSiteComparer.Core.WebPageProcessing.Models;

namespace WebSiteComparer.Core.WebPageProcessing.Implementation
{
    internal class WebSitesViewChangesTracker : IWebSitesViewChangesTracker
    {
        private readonly IScreenshotRepository _screenshotRepository;
        private readonly IWebPageScreenshotTaker _screenshotTaker;
        private readonly IImageComparer _imageComparer;
        private readonly ILogService _logService;

        public WebSitesViewChangesTracker(
            IWebPageScreenshotTaker screenshotTaker,
            IImageComparer imageComparer,
            IScreenshotRepository screenshotRepository,
            ILogService logService = null )
        {
            _screenshotTaker = screenshotTaker;
            _imageComparer = imageComparer;
            _screenshotRepository = screenshotRepository;
            _logService = logService;
        }

        public async Task CheckForViewChanges( IEnumerable<WebsiteConfiguration> websiteConfigurations )
        {
            IEnumerable<Task> tasks =
                from configuration in websiteConfigurations
                from url in configuration.Urls
                select CheckPageForViewChanges(
                    url,
                    configuration.ScreenshotWidth,
                    configuration.LoadConfiguration );

            await Task.WhenAll( tasks );
        }

        public async Task CheckForViewChanges( WebsiteConfiguration websiteConfiguration )
        {
            IEnumerable<Task> tasks = 
                from url in websiteConfiguration.Urls
                select CheckPageForViewChanges(
                    url,
                    websiteConfiguration.ScreenshotWidth,
                    websiteConfiguration.LoadConfiguration );

            await Task.WhenAll( tasks );
        }

        private async Task CheckPageForViewChanges(
            string url,
            int screenshotWidth,
            PageLoadConfiguration loadConfiguration )
        {
            _logService?.Message( $"Checking for changes: {url}" );
            
            Task<CashedBitmap> takeSiteScreenshotTask;
            try
            {
                takeSiteScreenshotTask = _screenshotTaker.TakeScreenshotAsync(
                    url,
                    screenshotWidth,
                    loadConfiguration );
            }
            catch ( Exception ex )
            {
                var message = $"An error occured while trying to process site. Url: {url}.\n{ex}";
                _logService?.Error( message );
                return;
            }

            Task<CashedBitmap> getPreviousSiteScreenshotTask;
            try
            {
                getPreviousSiteScreenshotTask = _screenshotRepository.GetPreviousVersion( url );
            }
            catch ( Exception ex )
            {
                var message = $"An error occured while trying to get older version of site. Url: {url}\n{ex}";
                _logService?.Error( message );
                return;
            }

            ImageComparingResult compareResult = await _imageComparer.CompareAsync(
                await takeSiteScreenshotTask,
                await getPreviousSiteScreenshotTask );

            _screenshotRepository.Save( url, compareResult.Bitmap, "Result" );
        }
    }
}