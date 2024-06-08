using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using ImageProcessing.Implementation;

namespace ImageProcessing.Creators.Implementation;

internal static class BitmapExtension
{
    private const PixelFormat SupportedPixelFormat = Constants.SupportedPixelFormat;

    public static Task<Color[]> GetPixelArrayAsync( this Bitmap bitmap )
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

    private static unsafe Color[] GetPixelArrayInternal( this Bitmap bitmap )
    {
        int width = bitmap.Width;
        int height = bitmap.Height;

        var result = new Color[height * width];

        BitmapData data = bitmap.LockAllBits( ImageLockMode.ReadOnly, SupportedPixelFormat );

        var source = ( byte* )data.Scan0.ToPointer();

        for ( var y = 0; y < height; y++ )
        {
            int sourceRowIndex = y * data.Stride;
            int resultIndex = y * width;
            for ( var x = 0; x < width; x++ )
            {
                int sourcePos = sourceRowIndex + x * 3;

                Color color = Color.FromArgb(
                    source[sourcePos + 2],
                    source[sourcePos + 1],
                    source[sourcePos] );

                result[resultIndex + x] = color;
            }
        }
            
        bitmap.UnlockBits( data );

        return result;
    }
}