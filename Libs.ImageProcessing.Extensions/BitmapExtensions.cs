using System.Drawing;
using Libs.ImageProcessing.Models;

namespace Libs.ImageProcessing.Extensions;

public static class BitmapExtensions
{
    public static async Task<Bitmap> ToBitmapAsync( this Task<byte[]> getArrayTask )
    {
        return BitmapBuilder.CreateFromByteArray( await getArrayTask );
    }

    public static async Task<CashedBitmap> ToCashedBitmapAsync( this Task<Bitmap> getBitmapTask )
    {
        return await CashedBitmap.CreateAsync( await getBitmapTask );
    }

    public static async Task<CashedBitmap> ToCashedBitmapAsync( this Task<byte[]> getArrayTask )
    {
        return await CashedBitmap.CreateAsync(
            BitmapBuilder.CreateFromByteArray( 
                await getArrayTask ) );
    }
}