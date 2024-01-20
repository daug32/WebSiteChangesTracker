using Microsoft.Playwright;

namespace Libs.Microsoft.Playwright.Screenshots;

public static class PageScreenshotsExtensions
{
    public static async Task<byte[]> TakeScreenshotAsync( 
        this IPage pageHandler, 
        PageScreenshotOptions? screenshotOptions = null )
    {
        screenshotOptions ??= new PageScreenshotOptions
        {
            FullPage = true,
            Type = ScreenshotType.Png
        };

        byte[] buffer = await pageHandler.ScreenshotAsync( screenshotOptions );

        await pageHandler.CloseAsync();

        return buffer;
    }
}