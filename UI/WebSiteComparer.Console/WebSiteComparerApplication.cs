using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebSiteComparer.Console.Commands;
using WebSiteComparer.Console.Utils;
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
        _websiteConfigurations = GetWebsiteConfigurations( configuration );
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
            _logger.LogCritical( ex, null );
        }

        _logger.Log( LogLevel.Information, "Completed" );
    }

    private CommandType ParseActionType( string[] args )
    {
        try
        {
            return ArgumentsHandler.Parse( args );
        }
        catch ( ArgumentOutOfRangeException )
        {
            _logger.Log( LogLevel.Debug, $"Couldn't parse command type. {CommandType.NeedHelp} is used instead" );
            return CommandType.NeedHelp;
        }
    }

    private static List<WebsiteConfiguration> GetWebsiteConfigurations( IConfiguration configuration )
    {
        List<WebsiteConfiguration>? websiteConfigurations = configuration
            .GetSection( "WebSites" )
            .Get<List<WebsiteConfiguration>>();

        if ( websiteConfigurations is null || !websiteConfigurations.Any() )
        {
            throw new ArgumentException(
                "Value must not be null or empty",
                nameof( websiteConfigurations ) );
        }

        foreach ( WebsiteConfiguration websiteConfiguration in websiteConfigurations )
        {
            WebsiteConfiguration.ValidateOrThrow( websiteConfiguration );
        }

        return websiteConfigurations;
    }
}