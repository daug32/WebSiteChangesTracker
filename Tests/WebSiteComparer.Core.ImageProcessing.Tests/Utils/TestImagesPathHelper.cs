namespace WebSiteComparer.Core.Tests.Utils;

public class TestImagesPathHelper
{
    public static string BuildInputPathDirectory( string fileName )
    {
        return Path.GetFullPath( $"../../../Sources/{fileName}" );
    }

    public static string BuildOutputPathDirectory( string functionName )
    {
        return Path.GetFullPath( $"../../../Sources/AutoTestResults/{functionName}.jpg" );
    }
}