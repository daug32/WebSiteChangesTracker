using Microsoft.Playwright;

namespace UiTesting.Playwright.Factories;

public class BrowserFactory : IAsyncDisposable
{
    private IBrowser? _browser;

    public async Task<IBrowser> GetBrowserAsync()
    {
        if ( _browser != null )
        {
            return _browser;
        }

        IPlaywright playwright = await global::Microsoft.Playwright.Playwright.CreateAsync();

        _browser = await playwright.Chromium.LaunchAsync( new BrowserTypeLaunchOptions
        {
            Headless = true
        } );

        return _browser;
    }

    public async ValueTask DisposeAsync()
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