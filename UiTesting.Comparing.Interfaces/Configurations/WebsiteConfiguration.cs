namespace WebSiteComparer.Core.Configurations;

public class WebsiteConfiguration
{
    public List<string> Urls { get; set; } = new();
    public int ScreenshotWidth { get; set; } = 1280;
    public PageLoadingConfiguration? PageLoadingConfiguration { get; set; } = null;

    public static void ValidateOrThrow( WebsiteConfiguration configuration )
    {
        if ( configuration.Urls is null || !configuration.Urls.Any() )
        {
            throw new ArgumentException(
                "Value must not be null or empty",
                nameof( configuration.Urls ) );
        }

        if ( configuration.ScreenshotWidth < 1 )
        {
            throw new ArgumentException(
                "Value must not be less than 1",
                nameof( configuration.ScreenshotWidth ) );
        }
    }
}