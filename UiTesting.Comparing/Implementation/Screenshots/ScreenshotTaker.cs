﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessing.Creators;
using ImageProcessing.Models;
using UiTesting.Playwright;
using UiTesting.Playwright.Factories;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using WebSiteComparer.Core.Screenshots;

namespace UiTesting.Comparing.Implementation.Screenshots;

internal class ScreenshotTaker : IScreenshotTaker
{
    private static readonly PageScreenshotOptions _pageScreenshotOptions = new()
    {
        FullPage = true,
        Type = ScreenshotType.Png,
        Animations = ScreenshotAnimations.Disabled
    };
    
    private readonly ILogger _logger;

    public ScreenshotTaker( ILogger<ScreenshotTaker> logger )
    {
        _logger = logger;
    }

    public async Task<Dictionary<Uri, CashedBitmap>> TakeScreenshotAsync( IEnumerable<ScreenshotOptions> allScreenshotOptions )
    {
        var concurrentDictionary = new ConcurrentDictionary<Uri, CashedBitmap>();
        await using ( var browserFactory = new BrowserFactory() )
        {
            IBrowser browser = await browserFactory.GetBrowserAsync();

            await Parallel.ForEachAsync(
                allScreenshotOptions,
                async ( options, _ ) =>
                {
                    IPage page = await browser.NewPageAsync();
                    concurrentDictionary.TryAdd(
                        options.Uri,
                        await TakeScreenshotAsync( page, options ) );

                    await page.CloseAsync();
                } );
        }
        
        return concurrentDictionary.ToDictionary( x => x.Key, x => x.Value );
    }

    public async Task<CashedBitmap> TakeScreenshotAsync( ScreenshotOptions options )
    {
        CashedBitmap image;
        await using ( var browserFactory = new BrowserFactory() )
        {
            IBrowser browser = await browserFactory.GetBrowserAsync();
            IPage page = await browser.NewPageAsync();
            image = await TakeScreenshotAsync( page, options );
            await page.CloseAsync();
        }

        return image;
    }

    public async Task<CashedBitmap> TakeScreenshotAsync( IPage page, ScreenshotOptions options )
    {
        Log( LogLevel.Information, "Loading a page", options.Uri );
        try
        {
            await page.GotoAsync( options.Uri.ToString() );
            await page.SetPageWidth( options.Width );
            await page.WaitForLoadStateAsync( LoadState.NetworkIdle );
        }
        catch ( Exception ex )
        {
            Log( ex, "Couldn't load page", options.Uri );
            throw;
        }

        Log( LogLevel.Information, "Creating a screenshot", options.Uri );
        try
        {
            return await CashedBitmapCreator.CreateAsync( 
                await page.ScreenshotAsync( _pageScreenshotOptions ) );
        }
        catch ( Exception ex )
        {
            Log( ex, "Couldn't create a screenshot", options.Uri );
            throw;
        }
    }

    private void Log( LogLevel logLevel, string message, Uri uri )
    {
        _logger.Log( logLevel, $"{message}\nUrl: {uri}" );
    }

    private void Log( Exception ex, string message, Uri uri )
    {
        _logger.LogCritical( ex, $"{message}\nUri: {uri}" );
    }
}