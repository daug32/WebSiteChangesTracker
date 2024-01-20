using WebSiteComparer.Core;
using WebSiteComparer.Core.ChangesTracking;

namespace WebSiteComparer.Console.Commands.Implementation;

public class CheckForChanges : ICommand
{
    private readonly IChangesTracker _changesTracker;

    public CheckForChanges( IChangesTracker changesTracker )
    {
        _changesTracker = changesTracker;
    }

    public CommandType CommandType => CommandType.CheckForChanges;

    public Task ExecuteAsync( List<WebsiteConfiguration> websiteConfigurations )
    {
        return _changesTracker.FindChanges( websiteConfigurations );
    }
}