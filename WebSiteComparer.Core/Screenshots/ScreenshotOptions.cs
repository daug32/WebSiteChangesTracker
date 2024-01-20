using System;

namespace WebSiteComparer.Core.Screenshots;

public class ScreenshotOptions
{
    public Uri Uri { get; }
    public int Width { get; }

    public ScreenshotOptions( string uri, int width )
    {
        Uri = new Uri( uri );
        Width = width < 1
            ? throw new ArgumentException( nameof( width ) )
            : width;
    }
}