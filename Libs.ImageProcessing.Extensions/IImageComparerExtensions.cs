using System.Drawing;
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
        Task<CashedBitmap> createFirstImageTask = CashedBitmap.CreateAsync( firstImage );
        Task<CashedBitmap> createSecondImageTask = CashedBitmap.CreateAsync( secondImage );

        return await comparer.CompareAsync(
            await createFirstImageTask,
            await createSecondImageTask );
    }

    public static async Task<ImageComparingResult> CompareAsync(
        this IImageComparer comparer,
        string pathToFirstImage,
        string pathToSecondImage )
    {
        Task<CashedBitmap> createFirstImageTask = CashedBitmap.CreateAsync( pathToFirstImage );
        Task<CashedBitmap> createSecondImageTask = CashedBitmap.CreateAsync( pathToSecondImage );

        return await comparer.CompareAsync(
            await createFirstImageTask,
            await createSecondImageTask );
    }
}