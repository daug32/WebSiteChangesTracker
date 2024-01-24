using Microsoft.Playwright;
using WebSiteComparer.Core.Configurations;

namespace WebSiteComparer.Core.Implementation.Extensions;

internal static class PageLoadingConfigurationExtensions
{
    public static BrowserNewPageOptions ToPlaywrightPageOptions(
        this PageLoadingConfiguration configuration ) => new()
    {
        JavaScriptEnabled = !configuration.DisableJavaScript
    };
}