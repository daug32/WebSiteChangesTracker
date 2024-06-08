namespace Libs.ImageProcessing.Models;

public class ImageComparingResult
{
    public CashedBitmap Bitmap { get; }
        
    public long ChangesNumber { get; }
        
    public float PercentOfChanges => 100f * ChangesNumber / Bitmap.Size.Height / Bitmap.Size.Width;
        
    public bool HasChanges => ChangesNumber > 0;
    
    public ImageComparingResult( CashedBitmap bitmap, long changesNumber )
    {
        Bitmap = bitmap;
        ChangesNumber = changesNumber;
    }
}