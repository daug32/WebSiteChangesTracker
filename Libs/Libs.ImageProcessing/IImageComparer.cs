using System.Threading.Tasks;
using Libs.ImageProcessing.Models;

namespace Libs.ImageProcessing;

public interface IImageComparer
{
    public Task<ImageComparingResult> CompareAsync(
        CashedBitmap firstImage,
        CashedBitmap secondImage );

    public ImageComparingResult Compare(
        CashedBitmap firstImage,
        CashedBitmap secondImage );
}