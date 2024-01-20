using System;
using Libs.ImageProcessing.Models;

namespace WebSiteComparer.Core.Screenshots;

public interface IScreenshotRepository
{
    public string? Get( Uri uri );
    public void Save( CashedBitmap image, Uri uri );
}