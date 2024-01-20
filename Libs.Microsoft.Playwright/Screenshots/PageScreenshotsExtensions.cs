using Libs.Microsoft.Playwright.PageLoading;
using Microsoft.Playwright;

namespace Libs.Microsoft.Playwright.Screenshots
{
    public static class PageScreenshotsExtensions
    {
        public static async Task<byte[]> TakeScreenshotAsync( this IPage pageHandler )
        {
            var screenshotOptions = new PageScreenshotOptions
            {
                FullPage = true,
                // TODO: Set type by ScreenshotConfig
                Type = ScreenshotType.Png
            };

            byte[] buffer = await pageHandler.ScreenshotAsync( screenshotOptions );

            await pageHandler.CloseAsync();

            return buffer;
        }
    }
}