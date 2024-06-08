using System.Drawing;
using ImageProcessing.Implementation.Utils;

namespace ImageProcessing.Implementation.Extensions;

internal static class ColorExtension
{
    public static Color ToMonochrome( this Color color )
    {
        int monochromeColor = ( color.R + color.B + color.G ) / 3;
            
        return Color.FromArgb(
            monochromeColor,
            monochromeColor,
            monochromeColor );
    }
        
    public static Color ToRedColor( this Color color )
    {
        return Color.FromArgb(
            MathAddOns.ChangeDiapason( color.R, 0, 255, 100, 200 ),
            MathAddOns.ChangeDiapason( color.G, 0, 255, 0, 40 ),
            MathAddOns.ChangeDiapason( color.B, 0, 255, 0, 40 ) );
    }
}