using WebSiteComparer.Core;
using WebSiteComparer.UseCases;

namespace WebSiteComparer.Console.Commands.Implementation;

public class UpdateScreenshotsCommand : ICommand
{
    private readonly UpdateScreenshotsCommandHandler _handler;

    public UpdateScreenshotsCommand( UpdateScreenshotsCommandHandler handler )
    {
        _handler = handler;
    }

    public CommandType CommandType => CommandType.UpdateScreenshots;

    public async Task ExecuteAsync( List<WebsiteConfiguration> websiteConfigurations )
    {
        await _handler.Handle( new UseCases.UpdateScreenshotsCommand( websiteConfigurations ) );
    }
}