using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebSiteComparer.Core;

// ReSharper disable once InconsistentNaming
public static class IWebSitesViewChangesTrackerExtensions
{
    public static async Task CheckForViewChanges(
        this IWebSitesViewChangesTracker changesTracker,
        IEnumerable<WebsiteConfiguration> configurations,
        CancellationToken token = default )
    {
        foreach ( WebsiteConfiguration configuration in configurations )
        {
            await changesTracker.CheckForViewChanges( configuration, token );
        }
    }
}