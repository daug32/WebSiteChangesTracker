using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace WebSiteComparer.Core.WebPageProcessing.Implementation.Utils
{
    public static class PageProcessor
    {
        public static async Task Process( List<Func<IPage, Task>> functions )
        {
            var limit = 10;
            
            for ( var i = 0; i < functions.Count; i += limit )
            {
                IBrowser browser = await BrowserFactory.GetBrowserAsync();
                
                var tasks = new List<Task>();
                foreach ( Func<IPage,Task> func in functions.Skip( i ).Take( limit ) )
                {
                    IPage page = await browser.NewPageAsync();
                    tasks.Add( func( page ) );
                }
                await Task.WhenAll( tasks );
                
                await BrowserFactory.DisposeAsync();
            }
        }
    }
}