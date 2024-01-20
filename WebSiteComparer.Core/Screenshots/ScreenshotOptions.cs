using System;

namespace WebSiteComparer.Core.Screenshots;

public class ScreenshotOptions
{
    public Uri Uri { get; }
    public int Width { get; }

    public ScreenshotOptions( string url, int width )
    {
        Uri = new Uri( url );
        Width = width < 1
            ? throw new ArgumentException( nameof( width ) )
            : width;
    }
}