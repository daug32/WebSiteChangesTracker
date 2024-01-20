using WebSiteComparer.Core;
using WebSiteComparer.Core.Screenshots;

namespace WebSiteComparer.Console.Commands.Implementation;

public class UpdateScreenshots : ICommand
{
    private readonly IScreenshotTaker _screenshotTaker;

    public UpdateScreenshots( IScreenshotTaker screenshotTaker )
    {
        _screenshotTaker = screenshotTaker;
    }

    public CommandType CommandType => CommandType.UpdateScreenshots;

    public async Task ExecuteAsync( List<WebsiteConfiguration> websiteConfigurations )
    {
        var screenshotOptions = websiteConfigurations
            .SelectMany( config => config.Urls.Select( url => new ScreenshotOptions( url, config.ScreenshotWidth ) ) )
            .ToList();

        await _screenshotTaker.TakeScreenshotAsync( screenshotOptions );
    }
}