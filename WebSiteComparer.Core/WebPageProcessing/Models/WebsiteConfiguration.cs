using System.Collections.Generic;

namespace WebSiteComparer.Core.WebPageProcessing.Models
{
    public class WebsiteConfiguration
    {
        public List<string> Urls { get; set; } = new List<string>();

        public PageLoadConfiguration LoadConfiguration { get; set; } = new PageLoadConfiguration();
        
        public int ScreenshotWidth { get; set; } = 1280;
    }
}