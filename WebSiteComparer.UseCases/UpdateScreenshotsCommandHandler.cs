using Libs.ImageProcessing.Models;
using Microsoft.Extensions.Logging;
using WebSiteComparer.Core.Screenshots;

namespace WebSiteComparer.UseCases;

public class UpdateScreenshotsCommandHandler
{
    private readonly ILogger _logger;
    private readonly IScreenshotTaker _screenshotTaker;
    private readonly IScreenshotRepository _screenshotRepository;

    public UpdateScreenshotsCommandHandler( 
        IScreenshotTaker screenshotTaker, 
        IScreenshotRepository screenshotRepository,
        ILogger<UpdateScreenshotsCommandHandler> logger )
    {
        _screenshotTaker = screenshotTaker;
        _screenshotRepository = screenshotRepository;
        _logger = logger;
    }

    public async Task Handle( UpdateScreenshotsCommand command )
    {
        var options = command.Configurations
            .SelectMany( config => config.Urls.Select( url => new ScreenshotOptions( url, config.ScreenshotWidth ) ) )
            .ToList();

        var screenshots = await _screenshotTaker.TakeScreenshotAsync( options );

        foreach ( ( Uri url, CashedBitmap? image ) in screenshots )
        {
            _logger.Log( LogLevel.Information, $"Saving a screenshot\nUrl: {url}" );
            _screenshotRepository.Save( image, url );
        }
    }
}