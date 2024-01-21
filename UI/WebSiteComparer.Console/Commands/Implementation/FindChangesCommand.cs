using WebSiteComparer.Core;
using WebSiteComparer.UseCases;

namespace WebSiteComparer.Console.Commands.Implementation;

internal class FindChangesCommand : ICommand
{
    private readonly FindChangesCommandHandler _handler;

    public FindChangesCommand( FindChangesCommandHandler handler )
    {
        _handler = handler;
    }

    public CommandType CommandType => CommandType.CheckForChanges;

    public async Task ExecuteAsync( List<WebsiteConfiguration> websiteConfigurations )
    {
        await _handler.Handle( new UseCases.FindChangesCommand( websiteConfigurations ) );
    }
}