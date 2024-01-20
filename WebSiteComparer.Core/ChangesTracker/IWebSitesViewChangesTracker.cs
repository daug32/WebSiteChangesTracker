using System.Threading;
using System.Threading.Tasks;

namespace WebSiteComparer.Core;

public interface IWebSitesViewChangesTracker
{
    Task CheckForViewChanges( 
        WebsiteConfiguration configuration,
        CancellationToken token = default );
}