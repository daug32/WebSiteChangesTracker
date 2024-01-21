using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Libs.ImageProcessing.Implementation.Extensions;

internal static class BitmapExtension
{
    private const PixelFormat SupportedPixelFormat = Constants.SupportedPixelFormat;

    public static Task<List<Color>> GetPixelArrayAsync( this Bitmap bitmap )
    {
        if ( bitmap == null )
        {
            throw new NullReferenceException();
        }

        return Task.Run( bitmap.GetPixelArrayInternal );
    }

    private static BitmapData LockAllBits(
        this Bitmap bitmap,
        ImageLockMode lockMode,
        PixelFormat pixelFormat )
    {
        var fullImageRectangle = new Rectangle(
            0,
            0,
            bitmap.Width,
            bitmap.Height );

        return bitmap.LockBits(
            fullImageRectangle,
            lockMode,
            pixelFormat );
    }

    private static unsafe List<Color> GetPixelArrayInternal( this Bitmap bitmap )
    {
        int width = bitmap.Width;
        int height = bitmap.Height;

        var result = new List<Color>( height * width );

        BitmapData data = bitmap.LockAllBits( ImageLockMode.ReadOnly, SupportedPixelFormat );

        var source = ( byte* )data.Scan0.ToPointer();

        for ( var y = 0; y < height; y++ )
        {
            int rowIndex = y * data.Stride;
            for ( var x = 0; x < width; x++ )
            {
                int pos = rowIndex + x * 3;

                Color color = Color.FromArgb(
                    source[pos + 2],
                    source[pos + 1],
                    source[pos] );

                result.Add( color );
            }
        }
            
        bitmap.UnlockBits( data );

        return result;
    }
}