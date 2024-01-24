using WebSiteComparer.Core;
using WebSiteComparer.Core.Configurations;

namespace WebSiteComparer.UseCases;

public class FindChangesCommand
{
    public List<WebsiteConfiguration> Configurations { get; }

    public FindChangesCommand( List<WebsiteConfiguration> configurations )
    {
        Configurations = configurations;
    }
}