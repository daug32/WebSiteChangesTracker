using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Libs.ImageProcessing.Models;
using Microsoft.Playwright;

namespace WebSiteComparer.Core.Screenshots;

public interface IScreenshotTaker
{
    public Task<Dictionary<Uri, CashedBitmap>> TakeScreenshotAsync( IEnumerable<ScreenshotOptions> allScreenshotOptions );

    public Task<CashedBitmap> TakeScreenshotAsync( ScreenshotOptions options );

    public Task<CashedBitmap> TakeScreenshotAsync( IPage page, ScreenshotOptions options );
}