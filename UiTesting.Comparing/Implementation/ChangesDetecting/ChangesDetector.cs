﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessing;
using ImageProcessing.Creators;
using ImageProcessing.Models;
using UiTesting.Playwright.Factories;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using UiTesting.Comparing.Implementation.Extensions;
using WebSiteComparer.Core.ChangesTracking;
using WebSiteComparer.Core.Configurations;
using WebSiteComparer.Core.Screenshots;

namespace UiTesting.Comparing.Implementation.ChangesDetecting;

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
                .Select( url => new ScreenshotOptions(
                    new Uri( url ),
                    configuration.ScreenshotWidth,
                    configuration.PageLoadingConfiguration ) ) )
            .ToList();

        return FindChangesInternalAsync( screenshotOptions );
    }

    public Task FindChangesAsync( WebsiteConfiguration configuration )
    {
        var screenshotOptions = configuration.Urls
            .Select( url => new ScreenshotOptions(
                new Uri( url ),
                configuration.ScreenshotWidth,
                configuration.PageLoadingConfiguration ) )
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

        await using var browserFactory = new BrowserFactory();
        IBrowser browser = await browserFactory.GetBrowserAsync();

        await Parallel.ForEachAsync(
            screenshotOptions,
            async ( screenshotOption, _ ) =>
            {
                IPage page = await browser.NewPageAsync(
                    screenshotOption
                        .PageLoadingConfiguration
                        .ToPlaywrightPageOptions() );

                CashedBitmap screenshot = await _screenshotTaker.TakeScreenshotAsync( page, screenshotOption );

                await Task.WhenAll(
                    CompareToOldVersionAsync( screenshotOption.Uri, screenshot ),
                    page.CloseAsync() );
            } );
    }

    private async Task CompareToOldVersionAsync( Uri uri, CashedBitmap newState )
    {
        string? imagePath = _screenshotRepository.Get( uri );

        _logger.Log( LogLevel.Debug, $"Loading old screenshot\nUrl: {uri}" );
        CashedBitmap oldState = imagePath is null
            ? CashedBitmapCreator.CreateEmpty( newState.Size.Width, newState.Size.Height )
            : await CashedBitmapCreator.CreateAsync( imagePath );

        _logger.Log( LogLevel.Debug, $"Comparing images\nUrl: {uri}" );
        ImageComparingResult result = await _imageComparer.CompareAsync( oldState, newState );

        string path = BuildFilePath( result.PercentOfChanges, uri );
        _logger.Log( LogLevel.Debug, $"Comparing images\nPath: {path}\nUrl: {uri}" );
        result.Bitmap.Save( path );
    }

    private string BuildFilePath( float changesPercent, Uri uri )
    {
        float serializedChangesPercent = MathF.Ceiling( changesPercent );
        return $"{_comparingOutputDirectory}/{serializedChangesPercent}_{uri.ToFilePath()}";
    }
}