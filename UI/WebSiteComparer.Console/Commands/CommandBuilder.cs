using Microsoft.Extensions.DependencyInjection;
using WebSiteComparer.Console.Commands.Implementation;
using WebSiteComparer.Core.ChangesTracking;
using WebSiteComparer.UseCases;
using FindChangesCommand = WebSiteComparer.Console.Commands.Implementation.FindChangesCommand;
using UpdateScreenshotsCommand = WebSiteComparer.Console.Commands.Implementation.UpdateScreenshotsCommand;

namespace WebSiteComparer.Console.Commands;

public class CommandBuilder
{
    private readonly IServiceProvider _serviceProvider;

    public CommandBuilder( IServiceProvider serviceProvider )
    {
        _serviceProvider = serviceProvider;
    }

    public ICommand Build( CommandType commandType )
    {
        return commandType switch
        {
            CommandType.NeedHelp => new GetHelpCommand(),
            CommandType.UpdateScreenshots => new UpdateScreenshotsCommand( GetService<UpdateScreenshotsCommandHandler>() ),
            CommandType.CheckForChanges => new FindChangesCommand( GetService<IChangesDetector>() ),
            _ => throw new ArgumentOutOfRangeException( nameof( commandType ), commandType, null )
        };
    }

    private T GetService<T>()
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}