using WebSiteComparer.Console.Commands.Implementation;
using WebSiteComparer.UseCases;
using UpdateScreenshotsCommand = WebSiteComparer.Console.Commands.Implementation.UpdateScreenshotsCommand;

namespace WebSiteComparer.Console.Commands;

public class CommandBuilder
{
    private readonly UpdateScreenshotsCommandHandler _updateScreenshotsCommandHandler;

    public CommandBuilder( UpdateScreenshotsCommandHandler updateScreenshotsCommandHandler )
    {
        _updateScreenshotsCommandHandler = updateScreenshotsCommandHandler;
    }

    public ICommand Build( CommandType commandType )
    {
        return commandType switch
        {
            CommandType.NeedHelp => new GetHelpCommand(),
            CommandType.UpdateScreenshots => new UpdateScreenshotsCommand( _updateScreenshotsCommandHandler ),
            CommandType.CheckForChanges => new CheckForChanges(),
            _ => throw new ArgumentOutOfRangeException( nameof( commandType ), commandType, null )
        };
    }
}