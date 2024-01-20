namespace Libs.ImageProcessing.Models
{
    public class ImageComparingResult
    {
        public CashedBitmap Bitmap { get; set; }
        
        public long ChangesNumber { get; set; }
        
        public float PercentOfChanges => 100f * ChangesNumber / Bitmap.Size.Height / Bitmap.Size.Width;
        
        public bool HasChanges => ChangesNumber > 0;
    }
}