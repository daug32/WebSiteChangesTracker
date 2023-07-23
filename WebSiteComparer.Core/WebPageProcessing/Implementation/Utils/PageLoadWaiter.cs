using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using WebSiteComparer.Core.WebPageProcessing.Implementation.Dictionaries;
using WebSiteComparer.Core.WebPageProcessing.Models;

namespace WebSiteComparer.Core.WebPageProcessing.Implementation.Utils
{
    public static class PageLoadWaiter
    {
        private static readonly int _defaultTimeoutInMilliseconds = 10 * 1000;
        
        public static async Task WaitAsync(
            IPage page,
            PageLoadConfiguration loadConfiguration )
        {
            var tasks = new List<Task>();

            if ( loadConfiguration.WaitUntilScriptsAreLoaded )
            {
                tasks.Add( WaitForScriptsLoadAsync( page ) );
            }

            if ( !string.IsNullOrWhiteSpace( loadConfiguration.IframeCssSelector ) )
            {
                tasks.Add( WaitForIFrameLoadAsync( page, loadConfiguration.IframeCssSelector ) );
            }

            if ( loadConfiguration.WaitForTransitionEndAtElements.Any() )
            {
                tasks.Add( WaitForTransitionEndAsync( page, loadConfiguration.WaitForTransitionEndAtElements ) );
            }

            if ( loadConfiguration.AdditionalLoadTime.TotalMilliseconds > 0 )
            {
                tasks.Add( WaitForTimeoutAsync( page, loadConfiguration.AdditionalLoadTime.TotalMilliseconds ) );
            }

            await Task.WhenAll( tasks );
        }

        private static Task<IJSHandle> WaitForScriptsLoadAsync( IPage page )
        {
            string script = JavaScriptLibs.IsDocumentReady();
            var waitForScriptLoadOptions = new PageWaitForFunctionOptions
            {
                Timeout = _defaultTimeoutInMilliseconds
            };
            
            return page.WaitForFunctionAsync(
                script,
                null,
                waitForScriptLoadOptions );
        }

        private static Task WaitForIFrameLoadAsync( IPage page, string iFrame )
        {
            var waitForIframe = new LocatorWaitForOptions
            {
                Timeout = _defaultTimeoutInMilliseconds
            };
            
            return page
                .Locator( iFrame )
                .WaitForAsync( waitForIframe );
        }

        private static Task WaitForTimeoutAsync( 
            IPage page,
            double timeoutInMilliseconds )
        {
            return page.WaitForTimeoutAsync( ( float )timeoutInMilliseconds );
        }

        private static Task<IJSHandle> WaitForTransitionEndAsync(
            IPage page,
            IEnumerable<string> elements )
        {
            string script = JavaScriptLibs.WaitForTransitionEndAtElement( elements );
            var waitForTransitionEndOptions = new PageWaitForFunctionOptions
            {
                Timeout = _defaultTimeoutInMilliseconds
            };

            Task<IJSHandle> waitForTransitionEndTask = page.WaitForFunctionAsync(
                script,
                null,
                waitForTransitionEndOptions );
            return waitForTransitionEndTask;
        }
    }
}