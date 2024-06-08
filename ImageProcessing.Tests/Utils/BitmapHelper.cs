using System.Drawing;
using ImageProcessing.Creators;
using ImageProcessing.Models;

namespace ImageProcessing.Tests.Utils;

public class BitmapHelper
{
    public static async Task<CashedBitmap> CreateCashedBitmapByColorMatrix( Color[][] matrix )
    {
        return await CashedBitmapCreator.CreateAsync( 
            CreateBitmapByColorMatrix( matrix ) );
    }

    private static Bitmap CreateBitmapByColorMatrix( Color[][] matrix )
    {
        int height = matrix.Length;
        int width = height > 0
            ? matrix[0].Length
            : 0;

        Bitmap bitmap = BitmapCreator.CreateEmpty( width, height );

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