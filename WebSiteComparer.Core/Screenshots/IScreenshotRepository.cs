using System;
using System.Threading.Tasks;
using WebSiteComparer.Core.ImageProcessing.Models;

namespace WebSiteComparer.Core.Screenshots
{
    public interface IScreenshotRepository
    {
        // ReSharper disable once UnusedMemberInSuper.Global
        string DateFormatPrefix { get; }
        Task<CashedBitmap> GetPreviousVersion( string url );
        void Save( string url, CashedBitmap bitmap, string subFolder );
        void Save( string url, CashedBitmap bitmap, DateTime dateTime );
    }
}