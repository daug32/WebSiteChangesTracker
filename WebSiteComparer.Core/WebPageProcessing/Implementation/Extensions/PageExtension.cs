using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using WebSiteComparer.Core.WebPageProcessing.Implementation.Exceptions;

namespace WebSiteComparer.Core.WebPageProcessing.Implementation.Extensions
{
    public static class PageExtension
    {
        public static Task SetPageWidth( this IPage page, int screenshotWidth )
        {
            if ( screenshotWidth <= 0 )
            {
                return Task.CompletedTask;
            }

            ValidateViewportSizeOrThrow( page );

            Task task = page.SetViewportSizeAsync(
                screenshotWidth,
                page.ViewportSize.Height );
            task.ConfigureAwait( false );

            return task;
        }

        private static void ValidateViewportSizeOrThrow( IPage page )
        {
            if ( page.ViewportSize == null )
            {
                throw new BrowserException( $"{nameof( page.ViewportSize )} expected to be not null, but got null" );
            }
        }
    }
}