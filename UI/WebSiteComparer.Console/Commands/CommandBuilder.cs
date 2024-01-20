using WebSiteComparer.Console.Commands.Implementation;
using WebSiteComparer.Core.Screenshots;

namespace WebSiteComparer.Console.Commands;

public class CommandBuilder
{
    private readonly IScreenshotTaker _screenshotTaker;

    public CommandBuilder( IScreenshotTaker screenshotTaker )
    {
        _screenshotTaker = screenshotTaker;
    }

    public ICommand Build( CommandType commandType )
    {
        return commandType switch
        {
            CommandType.NeedHelp => new GetHelpCommand(),
            CommandType.UpdateScreenshots => new UpdateScreenshots( _screenshotTaker ),
            CommandType.CheckForChanges => new CheckForChanges(),
            _ => throw new ArgumentOutOfRangeException( nameof( commandType ), commandType, null )
        };
    }
}