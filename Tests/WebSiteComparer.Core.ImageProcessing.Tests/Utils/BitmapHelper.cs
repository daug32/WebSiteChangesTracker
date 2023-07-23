using System.Drawing;
using WebSiteComparer.Core.ImageProcessing.Models;

namespace WebSiteComparer.Core.ImageProcessing.Tests.Utils;

public class BitmapHelper
{
    public static async Task<CashedBitmap> CreateCashedBitmapByColorMatrix( Color[][] matrix )
    {
        Bitmap bitmap = CreateBitmapByColorMatrix( matrix );
        return await CashedBitmap.CreateAsync( bitmap );
    }

    private static Bitmap CreateBitmapByColorMatrix( Color[][] matrix )
    {
        int height = matrix.Length;
        int width = height > 0
            ? matrix[0].Length
            : 0;

        Bitmap bitmap = BitmapBuilder.CreateEmpty( width, height );

        for ( var y = 0; y < height; y++ )
        {
            if ( matrix[y].Length != width )
            {
                throw new FormatException( "Color matrix should has constant size" );
            }

            for ( var x = 0; x < width; x++ )
            {
                bitmap.SetPixel( x, y, matrix[y][x] );
            }
        }

        return bitmap;
    }
}