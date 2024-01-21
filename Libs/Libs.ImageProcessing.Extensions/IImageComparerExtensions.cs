using System.Drawing;
using Libs.ImageProcessing.Creators;
using Libs.ImageProcessing.Models;

namespace Libs.ImageProcessing.Extensions;

// ReSharper disable once InconsistentNaming
public static class IImageComparerExtensions
{
    public static async Task<ImageComparingResult> CompareAsync(
        this IImageComparer comparer,
        Bitmap firstImage,
        Bitmap secondImage )
    {
        Task<CashedBitmap> createFirstImageTask = CashedBitmapCreator.CreateAsync( firstImage );
        Task<CashedBitmap> createSecondImageTask = CashedBitmapCreator.CreateAsync( secondImage );

        return await comparer.CompareAsync(
            await createFirstImageTask,
            await createSecondImageTask );
    }

    public static async Task<ImageComparingResult> CompareAsync(
        this IImageComparer comparer,
        string pathToFirstImage,
        string pathToSecondImage )
    {
        Task<CashedBitmap> createFirstImageTask = CashedBitmapCreator.CreateAsync( pathToFirstImage );
        Task<CashedBitmap> createSecondImageTask = CashedBitmapCreator.CreateAsync( pathToSecondImage );

        return await comparer.CompareAsync(
            await createFirstImageTask,
            await createSecondImageTask );
    }
}