﻿using System;
using System.IO;
using Libs.ImageProcessing.Models;
using WebSiteComparer.Core.Implementation.Extensions;
using WebSiteComparer.Core.Screenshots;

namespace WebSiteComparer.Core.Implementation.Screenshots;

internal class ScreenshotsRepository : IScreenshotRepository
{
    private readonly string _directory;

    public ScreenshotsRepository( WebSiteComparerConfiguration configuration )
    {
        _directory = configuration.ScreenshotDirectory;
    }

    public string? Get( Uri uri )
    {
        string path = BuildScreenshotPath( uri );
        if ( !File.Exists( path ) )
        {
            return null;
        }

        return path;
    }

    public void Save( CashedBitmap image, Uri uri )
    {
        image.Save( BuildScreenshotPath( uri ) );
    }

    private string BuildScreenshotPath( Uri uri )
    {
        return $"{_directory}/{uri.ToFilePath()}.png";
    }
}