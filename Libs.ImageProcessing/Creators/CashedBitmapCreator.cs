using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Libs.ImageProcessing.Implementation.Extensions;
using Libs.ImageProcessing.Models;

namespace Libs.ImageProcessing.Creators;

public static partial class CashedBitmapCreator
{
    public static Task<CashedBitmap> CreateAsync( byte[] byteArray )
    {
        return CreateAsync( BitmapCreator.CreateFromByteArray( byteArray ) );
    }

    public static Task<CashedBitmap> CreateAsync( string file )
    {
        return CreateAsync( BitmapCreator.CreateFromFile( file ) );
    }

    public static async Task<CashedBitmap> CreateAsync( Bitmap bitmap )
    {
        return new CashedBitmap(
            bitmap,
            await bitmap.GetPixelArrayAsync() );
    }

    public static CashedBitmap CreateEmpty( 
        int width,
        int height,
        Color? fillColor = null )
    {
        fillColor ??= Color.White;

        var data = new List<Color>( width * height );
        for ( var i = 0; i < width * height; i++ )
        {
            data.Add( fillColor.Value );
        }

        return new CashedBitmap(
            BitmapCreator.CreateEmpty( width, height ),
            data );
    }
}