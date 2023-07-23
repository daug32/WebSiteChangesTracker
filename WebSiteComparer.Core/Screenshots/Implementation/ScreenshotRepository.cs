using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebSiteComparer.Core.ImageProcessing.Models;
using WebSiteComparer.Core.Screenshots.Implementation.Utils;

namespace WebSiteComparer.Core.Screenshots.Implementation
{
    internal class ScreenshotRepository : IScreenshotRepository
    {
        private readonly string _directory;

        public string DateFormatPrefix => ScreenshotNameBuilder.DateFormat;

        public ScreenshotRepository( string directory )
        {
            _directory = directory ?? throw new NullReferenceException();
        }
        
        public Task<CashedBitmap> GetPreviousVersion( string url )
        {
            IEnumerable<string> foldersWithPreviousVersions = Directory
                .GetDirectories( _directory )
                .Where( folder => IsFolderWithScreenshots( Path.GetFileName( folder ) ) )
                .OrderByDescending( file => file );

            string lastVersion = foldersWithPreviousVersions.FirstOrDefault();
            if ( lastVersion == null )
            {
                throw new DirectoryNotFoundException();
            }

            string screenshotToSearch = ScreenshotNameBuilder.ConvertUrlToImageName( url );
            string oldImagePath = Directory
                .GetFiles( lastVersion )
                .FirstOrDefault( file => string.Equals( screenshotToSearch, Path.GetFileName( file ) ) );
            if ( oldImagePath == null )
            {
                throw new FileNotFoundException();
            }

            return CashedBitmap.CreateAsync( oldImagePath );
        }

        public void Save( 
            string url,
            CashedBitmap bitmap,
            DateTime dateTime )
        {
            Save( 
                url,
                bitmap,
                dateTime.ToString( DateFormatPrefix ) );
        }

        public void Save(
            string url,
            CashedBitmap bitmap, 
            string subFolder )
        {
            bitmap.Save( $"{_directory}/{subFolder}/{ScreenshotNameBuilder.ConvertUrlToImageName( url )}" );
        }

        private static bool IsFolderWithScreenshots( string folder )
        {
            return ScreenshotNameBuilder.IsFolderInDateNotation( folder );
        }
    }
}