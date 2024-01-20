using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Libs.ImageProcessing;
using Libs.ImageProcessing.Extensions;
using Libs.ImageProcessing.Models;
using Microsoft.Extensions.Logging;
using WebSiteComparer.Core.ChangesTracking;
using WebSiteComparer.Core.Implementation.Extensions;
using WebSiteComparer.Core.Screenshots;

namespace WebSiteComparer.Core.Implementation.ChangesTracking;

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

        Dictionary<Uri, CashedBitmap> currentStates = await _screenshotTaker.TakeScreenshotAsync( screenshotOptions );

        await Parallel.ForEachAsync(
            currentStates,
            async ( screenshotData, _ ) => await CompareToOldVersionAsync( screenshotData.Key, screenshotData.Value ) );
    }

    private async Task CompareToOldVersionAsync( Uri uri, CashedBitmap newState )
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
        return $"{_comparingOutputDirectory}/{serializedChangesPercent}_{uri.ToFilePath()}";
    }
}