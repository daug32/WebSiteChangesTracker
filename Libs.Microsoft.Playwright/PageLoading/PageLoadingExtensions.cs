using Libs.Microsoft.Playwright.PageLoading.Implementation;
using Microsoft.Playwright;

namespace Libs.Microsoft.Playwright.PageLoading
{
    public static class PageLoadingExtensions
    {
        public static async Task WaitForLoadingAsync(
            this IPage pageHandler,
            PageLoadingOptions? options = null )
        {
            var tasks = new List<Task>();
            options ??= new PageLoadingOptions();

            if ( options.WaitUntilScriptsAreLoaded )
            {
                tasks.Add( WaitForScriptsLoadAsync(
                    pageHandler,
                    options.TimeoutInMilliseconds ) );
            }

            if ( !string.IsNullOrWhiteSpace( options.IframeCssSelector ) )
            {
                tasks.Add( WaitForIFrameLoadAsync( 
                    pageHandler,
                    options.IframeCssSelector,
                    options.TimeoutInMilliseconds ) );
            }

            if ( options.WaitForTransitionEndAtElements.Any() )
            {
                tasks.Add( WaitForTransitionEndAsync( 
                    pageHandler, 
                    options.WaitForTransitionEndAtElements, 
                    options.TimeoutInMilliseconds ) );
            }

            if ( options.AdditionalLoadTime.TotalMilliseconds > 0 )
            {
                tasks.Add( Wait( pageHandler, options ) );
            }

            await Task.WhenAll( tasks );
        }

        private static Task<IJSHandle> WaitForScriptsLoadAsync( IPage page, int timeout )
        {
            string script = JavaScriptLibs.IsDocumentReady();
            var waitForScriptLoadOptions = new PageWaitForFunctionOptions
            {
                Timeout = timeout
            };
            
            return page.WaitForFunctionAsync(
                script,
                null,
                waitForScriptLoadOptions );
        }

        private static Task WaitForIFrameLoadAsync( IPage page, string iFrame, int timeout )
        {
            var waitForIframe = new LocatorWaitForOptions
            {
                Timeout = timeout
            };
            
            return page
                .Locator( iFrame )
                .WaitForAsync( waitForIframe );
        }

        private static Task<IJSHandle> WaitForTransitionEndAsync(
            IPage page,
            IEnumerable<string> elements,
            int timeout )
        {
            string script = JavaScriptLibs.WaitForTransitionEndAtElement( elements );
            var waitForTransitionEndOptions = new PageWaitForFunctionOptions
            {
                Timeout = timeout
            };

            return page.WaitForFunctionAsync(
                script,
                null,
                waitForTransitionEndOptions );
        }

        private static Task Wait( IPage pageHandler, PageLoadingOptions options )
        {
            return pageHandler.WaitForTimeoutAsync( ( float )options.AdditionalLoadTime.TotalMilliseconds );
        }
    }
}