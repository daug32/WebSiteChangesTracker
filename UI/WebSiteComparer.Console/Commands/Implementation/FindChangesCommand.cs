using WebSiteComparer.Core;
using WebSiteComparer.Core.ChangesTracking;

namespace WebSiteComparer.Console.Commands.Implementation;

public class FindChangesCommand : ICommand
{
    private readonly IChangesDetector _changesDetector;

    public FindChangesCommand( IChangesDetector changesDetector )
    {
        _changesDetector = changesDetector;
    }

    public CommandType CommandType => CommandType.CheckForChanges;

    public async Task ExecuteAsync( List<WebsiteConfiguration> websiteConfigurations )
    {
        await _changesDetector.FindChangesAsync( websiteConfigurations );
    }
}