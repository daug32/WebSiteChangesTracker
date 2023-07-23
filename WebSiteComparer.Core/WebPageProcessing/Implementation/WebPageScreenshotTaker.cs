using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using WebSiteComparer.Core.ImageProcessing;
using WebSiteComparer.Core.ImageProcessing.Implementation.Utils;
using WebSiteComparer.Core.ImageProcessing.Models;
using WebSiteComparer.Core.Screenshots;
using WebSiteComparer.Core.WebPageProcessing.Implementation.Extensions;
using WebSiteComparer.Core.WebPageProcessing.Implementation.Utils;
using WebSiteComparer.Core.WebPageProcessing.Models;

namespace WebSiteComparer.Core.WebPageProcessing.Implementation
{
    internal class WebPageScreenshotTaker : IWebPageScreenshotTaker
    {
        private readonly IScreenshotRepository _screenshotRepository;
        private readonly ILogService _logService;

        public WebPageScreenshotTaker(
            IScreenshotRepository screenshotRepository,
            ILogService logService )
        {
            _screenshotRepository = screenshotRepository;
            _logService = logService;
        }

        #region Public members

        public async Task TakeScreenshotsAndSaveAsync(
            IEnumerable<string> urls,
            int screenshotsWidth,
            PageLoadConfiguration loadConfiguration )
        {
            DateTime date = DateTime.UtcNow;

            // TODO: Move onto Parallel.ForEach
            List<Func<IPage, Task>> functions = urls
                .Select(
                    url =>
                    {
                        Task Func( IPage page )
                        {
                            return TakeScreenshotAndSaveInternalAsync(
                                url,
                                screenshotsWidth,
                                loadConfiguration,
                                page,
                                date );
                        }

                        return ( Func<IPage, Task> )Func;
                    } )
                .ToList();

            await PageProcessor.Process( functions );
        }

        public async Task<CashedBitmap> TakeScreenshotAsync(
            string url,
            int screenshotWidth,
            PageLoadConfiguration loadConfiguration )
        {
            IBrowser browser = await BrowserFactory.GetBrowserAsync();

            return await TakeScreenshotInternal(
                url,
                await browser.NewPageAsync(),
                screenshotWidth,
                loadConfiguration );
        }

        #endregion

        private async Task TakeScreenshotAndSaveInternalAsync(
            string url,
            int screenshotWidth,
            PageLoadConfiguration loadConfiguration,
            IPage page,
            DateTime date )
        {
            CashedBitmap bitmap = await TakeScreenshotInternal(
                url,
                page,
                screenshotWidth,
                loadConfiguration );

            _screenshotRepository.Save( url, bitmap, date );
        }

        private async Task<CashedBitmap> TakeScreenshotInternal(
            string url,
            IPage page,
            int screenshotWidth,
            PageLoadConfiguration loadConfiguration )
        {
            _logService?.Message( $"Loading page for {url}" );

            await page.GotoAsync( url );
            await page.SetPageWidth( screenshotWidth );

            try
            {
                await PageLoadWaiter
                    .WaitAsync( page, loadConfiguration )
                    .ConfigureAwait( false );
            }
            catch ( Exception ex )
            {
                var message = $"An error occured while trying to wait for page to load. Url: {url}\n{ex}";
                _logService?.Error( message );
            }

            byte[] buffer = await page
                .ScreenshotAsync(
                    new PageScreenshotOptions
                    {
                        FullPage = true,
                        Type = ScreenshotType.Png
                    } );

            await page.CloseAsync();

            var bitmap = await CashedBitmap.CreateAsync( BitmapBuilder.CreateFromByteArray( buffer ) );

            return bitmap;
        }
    }
}