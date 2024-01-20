using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Libs.ImageProcessing.Models;
using WebSiteComparer.Core.Extensions;

namespace WebSiteComparer.Core.Screenshots.Implementation;

internal class ScreenshotsRepository : IScreenshotRepository
{
    private readonly string _directory;

    public ScreenshotsRepository( WebSiteComparerConfiguration configuration )
    {
        _directory = configuration.ScreenshotDirectory;
    }

    public List<string> GetAll()
    {
        return Directory.GetFiles( _directory ).ToList();
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