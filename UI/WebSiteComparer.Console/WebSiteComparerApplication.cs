using Microsoft.Extensions.Configuration;
using WebSiteComparer.Console.Models;
using WebSiteComparer.Console.Utils;
using WebSiteComparer.Core;
using WebSiteComparer.Core.WebPageProcessing;
using WebSiteComparer.Core.WebPageProcessing.Models;

namespace WebSiteComparer.Console;

public class WebSiteComparerApplication
{
    private readonly List<WebsiteConfiguration> _websiteConfigurations;
    
    private readonly ILogService _logService;
    private readonly IWebPageScreenshotTaker _screenshotTaker;
    private readonly IWebSitesViewChangesTracker _webSitesViewChangesTracker; 

    public WebSiteComparerApplication(
        IConfiguration configuration,
        IWebPageScreenshotTaker screenshotTaker,
        IWebSitesViewChangesTracker webSitesViewChangesTracker,
        ILogService logService )
    {
        _screenshotTaker = screenshotTaker;
        _webSitesViewChangesTracker = webSitesViewChangesTracker;
        _logService = logService;

        _websiteConfigurations = configuration
                 .GetSection( "WebSites" )
                 .Get<List<WebsiteConfiguration>>()
             ?? throw new ArgumentException( "Configuration for Websites not found" );
    }

    public async Task StartAsync( string[] args )
    {
        _logService.Message( "Started" );

        // ActionType actionType = ArgumentsHandler.Parse( args );
        ActionType actionType = ActionType.GetScreenshots;

        try
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if ( actionType == ActionType.NeedHelp )
            {
                System.Console.WriteLine( ArgumentsHandler.GetHelp() );
                return;
            }

            if ( actionType == ActionType.CheckForUpdates )
            {
                await _webSitesViewChangesTracker.CheckForViewChanges( _websiteConfigurations );
                return;
            }

            if ( actionType == ActionType.GetScreenshots )
            {
                foreach ( WebsiteConfiguration configuration in _websiteConfigurations )
                {
                    await _screenshotTaker.TakeScreenshotsAndSaveAsync(
                        configuration.Urls,
                        configuration.ScreenshotWidth,
                        configuration.LoadConfiguration );
                }
            }
        }
        catch ( Exception ex )
        {
            _logService.Error( ex.Message );
        }

        _logService.Message( "Completed" );
    }
}