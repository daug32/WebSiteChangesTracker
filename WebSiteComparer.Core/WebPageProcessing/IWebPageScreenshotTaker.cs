using System.Collections.Generic;
using System.Threading.Tasks;
using WebSiteComparer.Core.ImageProcessing.Models;
using WebSiteComparer.Core.WebPageProcessing.Models;

namespace WebSiteComparer.Core.WebPageProcessing
{
    public interface IWebPageScreenshotTaker
    {
        // TODO: Add realization for multiple urls 
        Task<CashedBitmap> TakeScreenshotAsync(
            string url,
            int screenshotWidth,
            PageLoadConfiguration loadConfiguration );

        // TODO: Add realization for a single url
        Task TakeScreenshotsAndSaveAsync(
            IEnumerable<string> urls,
            int screenshotsWidth,
            PageLoadConfiguration loadConfiguration );
    }
}