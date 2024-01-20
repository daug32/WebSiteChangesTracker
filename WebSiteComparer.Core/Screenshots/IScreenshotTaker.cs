using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSiteComparer.Core.Screenshots;

public interface IScreenshotTaker
{
    public Task TakeScreenshotAsync( ScreenshotOptions options );
    Task TakeScreenshotAsync( IEnumerable<ScreenshotOptions> allScreenshotOptions );
}