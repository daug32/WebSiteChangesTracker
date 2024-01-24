using System;
using WebSiteComparer.Core.Configurations;

namespace WebSiteComparer.Core.Screenshots;

public class ScreenshotOptions
{
    public Uri Uri { get; }
    public int Width { get; }
    public PageLoadingConfiguration PageLoadingConfiguration { get; }

    public ScreenshotOptions( 
        Uri uri,
        int width,
        PageLoadingConfiguration? loadingConfiguration )
    {
        Uri = uri;
        Width = width < 1
            ? throw new ArgumentException( nameof( width ) )
            : width;
        PageLoadingConfiguration = loadingConfiguration ?? new PageLoadingConfiguration();
    }
}