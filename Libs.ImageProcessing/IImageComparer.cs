using System.Drawing;
using System.Threading.Tasks;
using Libs.ImageProcessing.Models;

namespace Libs.ImageProcessing
{
    public interface IImageComparer
    {
        Task<ImageComparingResult> CompareAsync(
            CashedBitmap firstImage,
            CashedBitmap secondImage );
        
        Task<ImageComparingResult> CompareAsync(
            Bitmap firstImage,
            Bitmap secondImage );
        
        Task<ImageComparingResult> CompareAsync(
            string pathToFirstImage,
            string pathToSecondImage );
    }
}