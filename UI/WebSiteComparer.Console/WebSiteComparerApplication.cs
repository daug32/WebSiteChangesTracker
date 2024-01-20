using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebSiteComparer.Console.Commands;
using WebSiteComparer.Core;

namespace WebSiteComparer.Console;

public class WebSiteComparerApplication
{
    private readonly ILogger _logger;
    private readonly CommandBuilder _commandBuilder;

    private readonly List<WebsiteConfiguration> _websiteConfigurations;

    public WebSiteComparerApplication(
        IConfiguration configuration,
        ILogger<WebSiteComparerApplication> logger,
        CommandBuilder commandBuilder )
    {
        _logger = logger;
        _commandBuilder = commandBuilder;

        _websiteConfigurations = configuration
                                     .GetSection( "WebSites" )
                                     .Get<List<WebsiteConfiguration>>()
                                 ?? throw new ArgumentException( "Configuration for Websites not found" );
    }

    public async Task StartAsync( string[] args )
    {
        _logger.Log( LogLevel.Information, "Started" );

        CommandType commandType = ParseActionType( args );
        ICommand command = _commandBuilder.Build( commandType );

        try
        {
            await command.ExecuteAsync( _websiteConfigurations );
        }
        catch ( Exception ex )
        {
            _logger.LogCritical( ex, null, null );
        }

        _logger.Log( LogLevel.Information, "Completed" );
    }

    private static CommandType ParseActionType( string[] args )
    {
        // return ArgumentsHandler.Parse( args );
        return CommandType.UpdateScreenshots;
    }
}