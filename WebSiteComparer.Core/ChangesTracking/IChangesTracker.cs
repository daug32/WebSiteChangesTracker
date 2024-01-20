using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSiteComparer.Core.ChangesTracking;

public interface IChangesTracker
{
    Task FindChanges( IEnumerable<WebsiteConfiguration> configuration );
}