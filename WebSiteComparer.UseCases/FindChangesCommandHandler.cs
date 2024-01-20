using Microsoft.Extensions.Logging;
using WebSiteComparer.Core;
using WebSiteComparer.Core.ChangesTracking;

namespace WebSiteComparer.UseCases;

public class FindChangesCommandHandler
{
    private readonly IChangesDetector _changesDetector;
    private readonly ILogger _logger;
    private readonly string _changesFindingOutputDirectory;

    public FindChangesCommandHandler( 
        IChangesDetector changesDetector, 
        ILogger<FindChangesCommandHandler> logger,
        WebSiteComparerConfiguration webSiteComparerConfiguration )
    {
        _changesDetector = changesDetector;
        _logger = logger;
        _changesFindingOutputDirectory = webSiteComparerConfiguration.ChangesTrackingOutputDirectory;
    }

    public async Task Handle( FindChangesCommand command )
    {
        await _changesDetector.FindChangesAsync( command.Configurations );
        _logger.Log( LogLevel.Information, $"Changes are saved into {_changesFindingOutputDirectory}" );
    } 
}