using System;
using System.Drawing;
using System.IO;
using ImageProcessing.Implementation;
using ImageProcessing.Implementation.Utils;

namespace ImageProcessing.Creators;

public static class BitmapCreator
{
    public static Bitmap CreateFromByteArray( byte[] byteArray )
    {
        Bitmap bitmap;
        using ( var ms = new MemoryStream( byteArray ) )
        {
            bitmap = Image.FromStream( ms ) as Bitmap ?? throw new InvalidCastException();
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