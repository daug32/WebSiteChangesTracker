using System.Text;
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
            if ( image is null )
            {
                _logger.Log( LogLevel.Warning, $"Couldn't save a screenshot because it doesn't exist\nUrl: {url}" );
                continue;
            }

            _screenshotRepository.Save( image, UrlToFilePath( url ) );
        }
    }

    private static string UrlToFilePath( Uri uri )
    {
        StringBuilder result = new StringBuilder( uri.Host + uri.PathAndQuery + uri.Fragment )
            .Replace( '/', '_' )
            .Replace( '.', '_' )
            .Replace( '-', '_' );

        if ( result[^1] == '_' )
        {
            result = result.Remove( result.Length - 1, 1 );
        }

        return result.ToString();
    }
}