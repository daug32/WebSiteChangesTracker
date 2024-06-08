using System.Threading.Tasks;
using ImageProcessing.Models;

namespace ImageProcessing;

public interface IImageComparer
{
    public Task<ImageComparingResult> CompareAsync(
        CashedBitmap firstImage,
        CashedBitmap secondImage );

    public ImageComparingResult Compare(
        CashedBitmap firstImage,
        CashedBitmap secondImage );
}