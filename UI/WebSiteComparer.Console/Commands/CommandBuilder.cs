using WebSiteComparer.Console.Commands.Implementation;
using WebSiteComparer.Core.ChangesTracking;
using WebSiteComparer.UseCases;
using UpdateScreenshotsCommand = WebSiteComparer.Console.Commands.Implementation.UpdateScreenshotsCommand;

namespace WebSiteComparer.Console.Commands;

public class CommandBuilder
{
    private readonly UpdateScreenshotsCommandHandler _updateScreenshotsCommandHandler;
    private readonly IChangesTracker _changesTracker;

    public CommandBuilder( UpdateScreenshotsCommandHandler updateScreenshotsCommandHandler, IChangesTracker changesTracker )
    {
        _updateScreenshotsCommandHandler = updateScreenshotsCommandHandler;
        _changesTracker = changesTracker;
    }

    public ICommand Build( CommandType commandType )
    {
        return commandType switch
        {
            CommandType.NeedHelp => new GetHelpCommand(),
            CommandType.UpdateScreenshots => new UpdateScreenshotsCommand( _updateScreenshotsCommandHandler ),
            CommandType.CheckForChanges => new CheckForChanges( _changesTracker ),
            _ => throw new ArgumentOutOfRangeException( nameof( commandType ), commandType, null )
        };
    }
}