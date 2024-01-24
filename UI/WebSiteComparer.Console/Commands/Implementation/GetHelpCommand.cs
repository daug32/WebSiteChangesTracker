using WebSiteComparer.Console.Utils;
using WebSiteComparer.Core;
using WebSiteComparer.Core.Configurations;

namespace WebSiteComparer.Console.Commands.Implementation;

internal class GetHelpCommand : ICommand
{
    private readonly Task _completedTask = Task.CompletedTask;
    
    public CommandType CommandType => CommandType.NeedHelp;

    public Task ExecuteAsync( List<WebsiteConfiguration> websiteConfigurations )
    {
        System.Console.WriteLine( ArgumentsHandler.GetHelp() );
        return _completedTask;
    }
}