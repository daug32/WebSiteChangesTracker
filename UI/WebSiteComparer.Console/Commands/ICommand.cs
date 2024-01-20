using WebSiteComparer.Core;

namespace WebSiteComparer.Console.Commands;

public interface ICommand
{
    public CommandType CommandType { get; }

    public Task ExecuteAsync( List<WebsiteConfiguration> websiteConfigurations );
}