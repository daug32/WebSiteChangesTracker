using Microsoft.Playwright;

namespace Libs.Microsoft.Playwright.Factories;

public static class BrowserFactory
{
    private static IBrowser? _browser;

    public static async Task<IBrowser> GetBrowserAsync()
    {
        if ( _browser != null )
        {
            return _browser;
        }

        var browserLaunchOptions = new BrowserTypeLaunchOptions
        {
            Headless = true
        };

        IPlaywright playwright = await global::Microsoft.Playwright.Playwright.CreateAsync();
        IBrowser browser = await playwright.Chromium.LaunchAsync( browserLaunchOptions );
        _browser = browser;

        return _browser;
    }

    public static async Task DisposeAsync()
    {
        if ( _browser == null )
        {
            return;
        }

        await _browser.CloseAsync();
        await _browser.DisposeAsync();
        _browser = null;
    }
}