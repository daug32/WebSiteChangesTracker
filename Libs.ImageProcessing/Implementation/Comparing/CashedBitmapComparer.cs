using System;
using System.Drawing;
using System.Threading.Tasks;
using Libs.ImageProcessing.Models;
using Libs.ImageProcessing.Implementation.Extensions;

namespace Libs.ImageProcessing.Implementation.Comparing;

public class CashedBitmapComparer : IImageComparer
{
    public async Task<ImageComparingResult> CompareAsync(
        string pathToFirstImage,
        string pathToSecondImage )
    {
        Task<CashedBitmap> createFirstImageTask = CashedBitmap.CreateAsync( pathToFirstImage );
        Task<CashedBitmap> createSecondImageTask = CashedBitmap.CreateAsync( pathToSecondImage );

        return await CompareAsync(
            await createFirstImageTask,
            await createSecondImageTask );
    }

    public async Task<ImageComparingResult> CompareAsync(
        Bitmap firstImage,
        Bitmap secondImage )
    {
        Task<CashedBitmap> createFirstImageTask = CashedBitmap.CreateAsync( firstImage );
        Task<CashedBitmap> createSecondImageTask = CashedBitmap.CreateAsync( secondImage );

        return await CompareAsync(
            await createFirstImageTask,
            await createSecondImageTask );
    }

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

        var result = new ImageComparingResult
        {
            Bitmap = firstImage
        };

        firstImage.UpdateEach(
            ( x, y, firstImageColor ) =>
            {
                if ( !secondImage.ContainsPoint( x, y ) )
                {
                    result.ChangesNumber++;
                    return firstImageColor;
                }

                Color secondImageColor = secondImage.GetPixel( x, y );
                if ( firstImageColor == secondImageColor )
                {
                    return firstImageColor.ToMonochrome();
                }

                result.ChangesNumber++;
                return firstImageColor.ToRedColor();
            } );

        return result;
    }
}