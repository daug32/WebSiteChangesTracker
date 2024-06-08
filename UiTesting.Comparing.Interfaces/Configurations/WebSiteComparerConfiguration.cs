namespace WebSiteComparer.Core.Configurations;

public class WebSiteComparerConfiguration
{
    public string ScreenshotDirectory { get; set; } = null!;
    public string ChangesTrackingOutputDirectory { get; set; } = null!;

    public void Validate()
    {
        if ( String.IsNullOrWhiteSpace( ScreenshotDirectory ) )
        {
            throw new ArgumentException( nameof( ScreenshotDirectory ) );
        }

        if ( String.IsNullOrWhiteSpace( ChangesTrackingOutputDirectory ) )
        {
            throw new ArgumentException( nameof( ChangesTrackingOutputDirectory ) );
        }
    }
}