using Microsoft.Extensions.Logging;
using WebSiteComparer.Core;
using WebSiteComparer.Core.ChangesTracking;

namespace WebSiteComparer.Console.Commands.Implementation;

public class CheckForChanges : ICommand
{
    private readonly ILogger _logger;
    private readonly IChangesTracker _changesTracker;
    private readonly WebSiteComparerConfiguration _webSiteComparerConfiguration;

    public CheckForChanges(
        IChangesTracker changesTracker, 
        ILogger<CheckForChanges> logger, 
        WebSiteComparerConfiguration webSiteComparerConfiguration )
    {
        _changesTracker = changesTracker;
        _logger = logger;
        _webSiteComparerConfiguration = webSiteComparerConfiguration;
    }

    public CommandType CommandType => CommandType.CheckForChanges;

    public async Task ExecuteAsync( List<WebsiteConfiguration> websiteConfigurations )
    {
        await _changesTracker.FindChanges( websiteConfigurations );
        _logger.Log( LogLevel.Information, $"Changes are saved into {_webSiteComparerConfiguration.ChangesTrackingOutputDirectory}" );
    }
}