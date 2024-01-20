using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Libs.ImageProcessing.Extensions;
using Libs.ImageProcessing.Models;
using Libs.Microsoft.Playwright;
using Libs.Microsoft.Playwright.Factories;
using Libs.Microsoft.Playwright.Screenshots;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace WebSiteComparer.Core.Screenshots;

public class ScreenshotTaker : IScreenshotTaker
{
    private PagesHandler? _pagesHandler = null;
    private readonly ILogger _logger;

    public ScreenshotTaker( ILogger<ScreenshotTaker> logger )
    {
        _logger = logger;
    }

    public async Task TakeScreenshotAsync( IEnumerable<ScreenshotOptions> allScreenshotOptions )
    {
        PagesHandler pagesHandler = await GetPagesHandlerAsync();
        await Parallel.ForEachAsync(
            allScreenshotOptions,
            async ( options, _ ) =>
            {
                IPage page = await pagesHandler.CreateAsync();
                await TakeScreenshotInternalAsync( page, options );
                await _pagesHandler!.CloseAsync( page );
            } );
    }

    public async Task TakeScreenshotAsync( ScreenshotOptions options )
    {
        PagesHandler pagesHandler = await GetPagesHandlerAsync();
        IPage page = await pagesHandler.CreateAsync();
        await TakeScreenshotInternalAsync( page, options );
    }

    private async Task TakeScreenshotInternalAsync(
        IPage page,
        ScreenshotOptions options )
    {
        Log( LogLevel.Information, "Going to a page", options.Uri );
        try
        {
            await page.GotoAsync( options.Uri.ToString() );
            await page.SetPageWidth( options.Width );
        }
        catch ( Exception ex )
        {
            Log( ex, "Couldn't get to the page", options.Uri );
            return;
        }

        Log( LogLevel.Information, "Waiting for page to load", options.Uri );
        try
        {
            await page.WaitForLoadStateAsync( LoadState.NetworkIdle );
        }
        catch ( Exception ex )
        {
            Log( ex, "Couldn't load page", options.Uri );
            return;
        }

        Log( LogLevel.Information, "Creating a screenshot", options.Uri );
        CashedBitmap image;
        try
        {
            image = await page
                .TakeScreenshotAsync()
                .ToBitmapAsync()
                .ToCashedBitmapAsync();
        }
        catch ( Exception ex )
        {
            Log( ex, "Couldn't create a screenshot", options.Uri );
            return;
        }

        var screenshotPath = $"C:/Users/daug3/Downloads/todelete/{UriToFile( options.Uri )}";
        Log( LogLevel.Information, $"Saving the screenshot. Path: {screenshotPath}", options.Uri );
        try
        {
            image.Save( screenshotPath );
        }
        catch ( Exception ex )
        {
            Log( ex, "Couldn't save the screenshot", options.Uri );
            return;
        }
        
        Log( LogLevel.Information, $"Processing a page completed", options.Uri );
    }

    private static string UriToFile( Uri uri )
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

    private async Task<PagesHandler> GetPagesHandlerAsync()
    {
        _pagesHandler ??= new PagesHandler( await BrowserFactory.GetBrowserAsync() ); 
        return _pagesHandler;
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