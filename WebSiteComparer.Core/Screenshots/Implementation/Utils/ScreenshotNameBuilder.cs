using System;
using System.Globalization;
using System.Linq;

namespace WebSiteComparer.Core.Screenshots.Implementation.Utils
{
    internal static class ScreenshotNameBuilder
    {
        public static string DateFormat => "yy_MM_dd_HH_mm_ss";

        /// <summary>
        ///     Converts site url into image name.<br/>
        ///     Examples:<br/>
        ///     * "test.com" -> "test.com.png"<br/>
        ///     * "test.com/" -> "test.com.png"<br/>
        ///     * "https://test.com" -> "test.com.png"<br/>
        ///     * "test.com/ru" -> "test.com_ru.png"<br/>
        ///     * "http://test.com/folder/index.html" -> "test.com_folder_index.html.png"<br/>
        /// </summary>
        public static string ConvertUrlToImageName( string url )
        {
            if ( string.IsNullOrWhiteSpace( url ) )
            {
                throw new UriFormatException();
            }

            // Used for Uri.ctor because it expects url with specified scheme 
            if ( url.IndexOf( "://", StringComparison.Ordinal ) < 0 )
            {
                url = "http://" + url;
            }

            var parsedUrl = new Uri( url );

            var name = $"{parsedUrl.Host}{parsedUrl.AbsolutePath.Replace( "/", "_" )}";

            // If url ends with '/' then the image name will end with '_'
            // Image name shouldn't end with it
            if ( name.Last() == '_' )
            {
                name = name.Remove( name.Length - 1 );
            }

            return $"{name}.png";
        }

        public static bool IsFolderInDateNotation( string folder )
        {
            return TryGetDateFromFolderInDateNotation( folder, out DateTime _ );
        }

        public static bool TryGetDateFromFolderInDateNotation( string fileName, out DateTime date )
        {
            return DateTime.TryParseExact(
                fileName,
                DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date );
        }
    }
}