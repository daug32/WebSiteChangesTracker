using Microsoft.Playwright;

namespace Libs.Microsoft.Playwright;

public static class PageExtension
{
    public static Task SetPageWidth( this IPage page, int width )
    {
        ValidateSizeOrThrow( width );
        ValidateViewportSizeOrThrow( page );

        return page.SetViewportSizeAsync(
            width,
            page.ViewportSize!.Height );
    }

    private static void ValidateSizeOrThrow( int width )
    {
        if ( width <= 0 )
        {
            throw new ArgumentException( $"Page {nameof( width )} must be greater than 0" );
        }
    }

    private static void ValidateViewportSizeOrThrow( IPage page )
    {
        if ( page.ViewportSize == null )
        {
            throw new ArgumentException( $"{nameof( page.ViewportSize )} expected to be not null, but got null" );
        }
    }
}