using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Libs.ImageProcessing;
using Libs.ImageProcessing.Creators;
using Libs.ImageProcessing.Models;
using Libs.Microsoft.Playwright.Factories;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using WebSiteComparer.Core.ChangesTracking;
using WebSiteComparer.Core.Implementation.Extensions;
using WebSiteComparer.Core.Screenshots;

namespace WebSiteComparer.Core.Implementation.ChangesDetecting;

internal class ChangesDetector : IChangesDetector
{
    private readonly IScreenshotTaker _screenshotTaker;
    private readonly IScreenshotRepository _screenshotRepository;
    private readonly IImageComparer _imageComparer;
    private readonly ILogger _logger;
    
    private readonly string _comparingOutputDirectory;

    public ChangesDetector( 
        IImageComparer imageComparer,
        ILogger<ChangesDetector> logger,
        IScreenshotTaker screenshotTaker,
        IScreenshotRepository screenshotRepository,
        WebSiteComparerConfiguration configuration )
    {
        _logger = logger;
        _screenshotTaker = screenshotTaker;
        _screenshotRepository = screenshotRepository;
        _comparingOutputDirectory = configuration.ChangesTrackingOutputDirectory;
        _imageComparer = imageComparer;
    }

    public Task FindChangesAsync( IEnumerable<WebsiteConfiguration> configurations )
    {
        var screenshotOptions = configurations
            .SelectMany( configuration => configuration.Urls
                .Select( url => new ScreenshotOptions( url, configuration.ScreenshotWidth ) ) )
            .ToList();

        return FindChangesInternalAsync( screenshotOptions );
    }

    public Task FindChangesAsync( WebsiteConfiguration configuration )
    {
        var screenshotOptions = configuration.Urls
            .Select( url => new ScreenshotOptions( url, configuration.ScreenshotWidth ) )
            .ToList();

        return FindChangesInternalAsync( screenshotOptions );
    }

    private async Task FindChangesInternalAsync( List<ScreenshotOptions> screenshotOptions )
    {
        var directoryInfo = new DirectoryInfo( _comparingOutputDirectory );
        if ( directoryInfo.Exists )
        {
            directoryInfo.ClearDirectory();
        }

        IBrowser browser = await BrowserFactory.GetBrowserAsync();

        await Parallel.ForEachAsync(
            screenshotOptions,
            async ( screenshotOption, _ ) =>
            {
                IPage page = await browser.NewPageAsync();
                CashedBitmap screenshot = await _screenshotTaker.TakeScreenshotAsync( page, screenshotOption );
                
                await Task.WhenAll(
                    CompareToOldVersionAsync( screenshotOption.Uri, screenshot ),
                    page.CloseAsync() );
            } );
    }

    private async Task CompareToOldVersionAsync( Uri uri, CashedBitmap newState )
    {
        string? imagePath = _screenshotRepository.Get( uri );

        _logger.Log( LogLevel.Information, $"Loading old screenshot\nUrl: {uri}" );
        CashedBitmap oldState = imagePath is null
            ? CashedBitmapCreator.CreateEmpty( newState.Size.Width, newState.Size.Height )
            : await CashedBitmapCreator.CreateAsync( imagePath );

        _logger.Log( LogLevel.Information, $"Comparing images\nUrl: {uri}" );
        ImageComparingResult result = await _imageComparer.CompareAsync( oldState, newState );

        string path = BuildFilePath( result.PercentOfChanges, uri );
        _logger.Log( LogLevel.Information, $"Comparing images\nPath: {path}\nUrl: {uri}" );
        result.Bitmap.Save( path );
    }

    private string BuildFilePath( float changesPercent, Uri uri )
    {
        float serializedChangesPercent = MathF.Ceiling( changesPercent );
        return $"{_comparingOutputDirectory}/{serializedChangesPercent}_{uri.ToFilePath()}";
    }
}