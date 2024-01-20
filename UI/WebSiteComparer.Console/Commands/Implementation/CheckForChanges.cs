using WebSiteComparer.Core;

namespace WebSiteComparer.Console.Commands.Implementation;

public class CheckForChanges : ICommand
{
    public CommandType CommandType => CommandType.CheckForChanges;

    public Task ExecuteAsync( List<WebsiteConfiguration> websiteConfigurations )
    {
        throw new NotImplementedException();
    }
}