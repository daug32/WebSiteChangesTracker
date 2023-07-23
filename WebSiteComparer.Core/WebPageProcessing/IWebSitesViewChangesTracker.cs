using System.Collections.Generic;
using System.Threading.Tasks;
using WebSiteComparer.Core.WebPageProcessing.Models;

namespace WebSiteComparer.Core.WebPageProcessing
{
    public interface IWebSitesViewChangesTracker
    {
        // ReSharper disable once UnusedMember.Global
        Task CheckForViewChanges( WebsiteConfiguration websiteConfiguration );
        
        Task CheckForViewChanges( IEnumerable<WebsiteConfiguration> websiteConfigurations );
    }
}