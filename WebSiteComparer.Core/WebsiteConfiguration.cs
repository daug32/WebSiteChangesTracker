using System.Collections.Generic;
using Libs.Microsoft.Playwright.PageLoading;

namespace WebSiteComparer.Core
{
    public class WebsiteConfiguration
    {
        public List<string> Urls { get; set; } = new();
        
        public int ScreenshotWidth { get; set; } = 1280;

        public PageLoadingOptions LoadConfiguration { get; set; } = new();
    }
}