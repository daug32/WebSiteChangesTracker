using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Libs.ImageProcessing;
using Libs.ImageProcessing.Extensions;
using Libs.ImageProcessing.Models;
using Libs.System.IO.Extensions;
using Microsoft.Extensions.Logging;
using WebSiteComparer.Core.Extensions;
using WebSiteComparer.Core.Screenshots;

namespace WebSiteComparer.Core.ChangesTracking.Implementation;

internal class ChangesTracker : IChangesTracker
{
    private readonly WebSiteComparerConfiguration _configuration;
    private readonly IScreenshotTaker _screenshotTaker;
    private readonly IScreenshotRepository _screenshotRepository;
    private readonly IImageComparer _imageComparer;
    private readonly ILogger _logger;

    public ChangesTracker( 
        IImageComparer imageComparer,
        ILogger<ChangesTracker> logger,
        IScreenshotTaker screenshotTaker,
        IScreenshotRepository screenshotRepository,
        WebSiteComparerConfiguration configuration )
    {
        _logger = logger;
        _screenshotTaker = screenshotTaker;
        _screenshotRepository = screenshotRepository;
        _configuration = configuration;
        _imageComparer = imageComparer;
    }

    public async Task FindChanges( IEnumerable<WebsiteConfiguration> configurations )
    {
        var options = configurations
            .SelectMany( configuration => configuration.Urls
                .Select( url => new ScreenshotOptions( url, configuration.ScreenshotWidth ) ) )
            .ToList();

        var directoryInfo = new DirectoryInfo( _configuration.ChangesTrackingOutputDirectory );
        if ( directoryInfo.Exists )
        {
            directoryInfo.ClearDirectory();
        }

        Dictionary<Uri, CashedBitmap> currentStates = await _screenshotTaker.TakeScreenshotAsync( options );

        await Parallel.ForEachAsync(
            currentStates,
            async ( screenshotData, _ ) => await FindChanges( screenshotData.Key, screenshotData.Value ) );
    }

    private async Task FindChanges( Uri uri, CashedBitmap newState )
    {
        string? imagePath = _screenshotRepository.Get( uri );

        _logger.Log( LogLevel.Information, $"Loading old screenshot\nUrl: {uri}" );
        CashedBitmap oldState = imagePath is null
            ? CashedBitmap.CreateEmpty( newState.Size.Width, newState.Size.Height )
            : await BitmapBuilder
                .CreateFromFile( imagePath )
                .ToCashedBitmapAsync();

        _logger.Log( LogLevel.Information, $"Comparing images\nUrl: {uri}" );
        ImageComparingResult result = await _imageComparer.CompareAsync( oldState, newState );

        string path = BuildFilePath( result.PercentOfChanges, uri );
        _logger.Log( LogLevel.Information, $"Comparing images\nPath: {path}\nUrl: {uri}" );
        result.Bitmap.Save( path );
    }

    private string BuildFilePath( float changesPercent, Uri uri )
    {
        float serializedChangesPercent = MathF.Ceiling( changesPercent );
        return $"{_configuration.ChangesTrackingOutputDirectory}/{serializedChangesPercent}_{uri.ToFilePath()}";
    }
}