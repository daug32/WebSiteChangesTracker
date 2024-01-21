using System.Collections.Generic;

namespace WebSiteComparer.Core;

public class WebsiteConfiguration
{
    public List<string> Urls { get; set; } = new();
        
    public int ScreenshotWidth { get; set; } = 1280;
}