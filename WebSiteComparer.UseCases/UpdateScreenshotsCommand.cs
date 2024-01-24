using WebSiteComparer.Core;
using WebSiteComparer.Core.Configurations;

namespace WebSiteComparer.UseCases;

public class UpdateScreenshotsCommand
{
    public List<WebsiteConfiguration> Configurations { get; }

    public UpdateScreenshotsCommand( List<WebsiteConfiguration> configurations )
    {
        Configurations = configurations.Count < 1
            ? throw new ArgumentException( nameof( configurations ) )
            : configurations;
    }
}