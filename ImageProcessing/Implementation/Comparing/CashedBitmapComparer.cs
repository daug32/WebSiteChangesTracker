using System;
using System.Drawing;
using System.Threading.Tasks;
using ImageProcessing.Models;
using ImageProcessing.Implementation.Extensions;

namespace ImageProcessing.Implementation.Comparing;

public class CashedBitmapComparer : IImageComparer
{
    public Task<ImageComparingResult> CompareAsync(
        CashedBitmap firstImage,
        CashedBitmap secondImage )
    {
        return Task.Run( () => Compare( firstImage, secondImage ) );
    }

    public ImageComparingResult Compare(
        CashedBitmap firstImage,
        CashedBitmap secondImage )
    {
        int maxWidth = Math.Max( firstImage.Size.Width, secondImage.Size.Width );
        int maxHeight = Math.Max( firstImage.Size.Height, secondImage.Size.Height );

        firstImage.Resize( new Size( maxWidth, maxHeight ), Color.Red );

        var changesNumber = 0;

        firstImage.UpdateEach(
            ( x, y, firstImageColor ) =>
            {
                if ( !secondImage.ContainsPoint( x, y ) )
                {
                    changesNumber++;
                    return firstImageColor;
                }

                Color secondImageColor = secondImage.GetPixel( x, y );
                if ( firstImageColor == secondImageColor )
                {
                    return firstImageColor.ToMonochrome();
                }

                changesNumber++;
                return firstImageColor.ToRedColor();
            } );

        return new ImageComparingResult( firstImage, changesNumber );
    }
}