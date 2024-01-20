using System.Collections.Generic;
using System.IO;
using System.Linq;
using Libs.ImageProcessing.Models;

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

    public void Save( CashedBitmap image, string imageName )
    {
        image.Save( $"{_directory}/{imageName}" );
    }
}