using System;
using System.Collections.Generic;
using Libs.ImageProcessing.Models;

namespace WebSiteComparer.Core.Screenshots;

public interface IScreenshotRepository
{
    public List<string> GetAll();
    public void Save( CashedBitmap image, Uri uri );
    public string? Get( Uri uri );
}