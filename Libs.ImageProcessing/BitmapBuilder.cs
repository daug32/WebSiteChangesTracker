using System.Drawing;
using System.IO;
using Libs.ImageProcessing.Implementation;
using Libs.ImageProcessing.Implementation.Utils;

namespace Libs.ImageProcessing
{
    public static class BitmapBuilder
    {
        public static Bitmap CreateFromByteArray( byte[] byteArray )
        {
            Bitmap bitmap;
            using ( var ms = new MemoryStream( byteArray ) )
            {
                bitmap = Image.FromStream( ms ) as Bitmap;
            }

            return Standardize( bitmap );
        }

        public static Bitmap CreateFromFile( string file )
        {
            var bitmap = new Bitmap( file );
            return Standardize( bitmap );
        }

        public static Bitmap CreateEmpty( int width, int height )
        {
            return new Bitmap( width, height, Constants.SupportedPixelFormat );
        }

        private static Bitmap Standardize( Bitmap bitmap )
        {
            if ( bitmap.PixelFormat == Constants.SupportedPixelFormat )
            {
                return bitmap;
            }
            
            Bitmap convertedBitmap = BitmapHelper.ConvertBitmapTo24RgbFormat( bitmap );
            bitmap.Dispose();

            return convertedBitmap;
        }
    }
}