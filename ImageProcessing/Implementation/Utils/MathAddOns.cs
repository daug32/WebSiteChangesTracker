namespace ImageProcessing.Implementation.Utils;

internal static class MathAddOns
{
    public static int ChangeDiapason(
        int value,
        float oldMin, float oldMax,
        float newMin, float newMax )
    {
        return ( int )(
            ( value - oldMin ) / ( oldMax - oldMin ) * ( newMax - newMin ) + newMin
        );
    }
}