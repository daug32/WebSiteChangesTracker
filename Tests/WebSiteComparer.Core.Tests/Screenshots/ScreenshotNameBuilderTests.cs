using NUnit.Framework;
using WebSiteComparer.Core.Screenshots.Implementation.Utils;

namespace WebSiteComparer.Core.Tests.Screenshots;

public class ScreenshotNameBuilderTests
{
    [TestCase( "test", "test.png" )]
    [TestCase( "test.com", "test.com.png" )]
    [TestCase( "test.com/", "test.com.png" )]
    [TestCase( "http://test.com", "test.com.png" )]
    [TestCase( "https://test.com", "test.com.png" )]
    [TestCase( "test.com/ru", "test.com_ru.png" )]
    [TestCase( "test.com/ru/", "test.com_ru.png" )]
    [TestCase( "test.com/folder/index.html", "test.com_folder_index.html.png" )]
    [TestCase( "127.0.0.1:5000/folder", "127.0.0.1_folder.png" )]
    public void ConvertUrlToImageName_ValidUrl_ExpectedImageName( string url, string expectedName )
    {
        // Act
        string result = ScreenshotNameBuilder.ConvertUrlToImageName( url );
        
        // Assert
        Assert.AreEqual( expectedName, result );
    }

    [TestCase( "" )]
    [TestCase( "  " )]
    [TestCase( null )]
    [TestCase( "://some")]
    public void ConvertUrlToImageName_InvalidUrl_UriFormatException( string url )
    {
        Assert.Throws<UriFormatException>( () => ScreenshotNameBuilder.ConvertUrlToImageName( url ) );
    }
}