using System.Drawing;
using NUnit.Framework;
using Libs.ImageProcessing.Models;

namespace Libs.ImageProcessing.Tests.ImageProcessing;

public class CashedBitmapTests
{
    private const int BigWidth = 4000;
    private const int BigHeight = 4000;

    private const int TestBitmapHeight = 5;
    private const int TestBitmapWidth = 10;
    private readonly Color _testBitmapColor = Color.Blue;

    [Test]
    public void CreateAsync_CreateForBigBitmap_DoesntThrow()
    {
        Assert.DoesNotThrow( () => CashedBitmap.CreateAsync( BitmapBuilder.CreateEmpty( BigWidth, BigHeight ) ).Wait() );
    }

    [Test]
    public async Task CreateAsync_CreateThreeCashedBitmapsForBigBitmaps_AsynchronousCreatingOfCashedBitmaps()
    {
        Task<CashedBitmap> a = CashedBitmap.CreateAsync( BitmapBuilder.CreateEmpty( BigWidth, BigHeight ) );
        Task<CashedBitmap> b = CashedBitmap.CreateAsync( BitmapBuilder.CreateEmpty( BigWidth, BigHeight ) );
        Task<CashedBitmap> c = CashedBitmap.CreateAsync( BitmapBuilder.CreateEmpty( BigWidth, BigHeight ) );

        await Task.WhenAll( a, b, c );
    }

    [TestCase( 0, 0 )]
    [TestCase( TestBitmapWidth - 1, 0 )]
    [TestCase( 0, TestBitmapHeight - 1 )]
    [TestCase( TestBitmapWidth - 1, TestBitmapHeight - 1 )]
    public async Task SquareBracketsOperator_GetColorOfExistingPoint_ReturnsWhite( int x, int y )
    {
        // Arrange
        CashedBitmap bitmap = await CreateTestCashedBitmapAsync();
        int expected = _testBitmapColor.ToArgb();

        // Act
        Color result = bitmap.GetPixel( x, y );

        // Assert
        Assert.AreEqual( expected, result.ToArgb() );
    }

    [TestCase( -1, -1 )]
    [TestCase( 2, -1 )]
    [TestCase( TestBitmapWidth, 3 )]
    [TestCase( 3, TestBitmapHeight )]
    [TestCase( TestBitmapWidth, TestBitmapHeight )]
    [TestCase( 230, 303 )]
    public async Task SquareBracketsOperator_GetColorOfNonexistentPoint_ThrowsException( int x, int y )
    {
        // Arrange
        CashedBitmap bitmap = await CreateTestCashedBitmapAsync();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => bitmap.GetPixel( x, y ) );
    }

    [TestCase( -1, -1 )]
    [TestCase( -1, 0 )]
    [TestCase( 0, -1 )]
    [TestCase( TestBitmapWidth - 1, TestBitmapHeight )]
    [TestCase( TestBitmapWidth, TestBitmapHeight - 1 )]
    [TestCase( TestBitmapWidth, TestBitmapHeight )]
    public async Task ContainsPoint_InputIsAPointOutOfRange_ReturnsFalse( int x, int y )
    {
        // Arrange
        CashedBitmap bitmap = await CreateTestCashedBitmapAsync();

        // Act
        bool result = bitmap.ContainsPoint( x, y );

        // Assert
        Assert.IsFalse( result );
    }

    [TestCase( 0, 0 )]
    [TestCase( 5, 3 )]
    [TestCase( TestBitmapWidth - 1, TestBitmapHeight - 1 )]
    public async Task ContainsPoint_InputIsAPointThatIsInRange_ReturnsTrue( int x, int y )
    {
        // Arrange
        CashedBitmap bitmap = await CreateTestCashedBitmapAsync();

        // Act
        bool result = bitmap.ContainsPoint( x, y );

        // Assert
        Assert.IsTrue( result );
    }

    [Test]
    public async Task Resize_CropSize_SizeIsChangedAndBitmapSizeIsAlsoCropped()
    {
        // Arrange
        CashedBitmap bitmap = await CreateTestCashedBitmapAsync();
        var newSize = new Size( 1, 1 );
        
        // Act
        bitmap.Resize( newSize );
        
        // Assert
        bitmap.CommitChangesIfNeed();
        Assert.AreEqual( newSize, bitmap.Size );
        Assert.AreEqual( newSize, bitmap.SourceBitmap.Size );
    }

    [Test]
    public async Task Resize_ExtendSize_SizeIsChangedAndBitmapSizeIsAlsoExchanged()
    {
        // Arrange
        CashedBitmap bitmap = await CreateTestCashedBitmapAsync();
        var newSize = new Size( TestBitmapWidth + 10, TestBitmapWidth + 10 );
        
        // Act
        bitmap.Resize( newSize );
        
        // Assert
        bitmap.CommitChangesIfNeed();
        Assert.AreEqual( newSize, bitmap.Size );
        Assert.AreEqual( newSize, bitmap.SourceBitmap.Size );
    }

    private Task<CashedBitmap> CreateTestCashedBitmapAsync()
    {
        Bitmap? bitmap = BitmapBuilder.CreateEmpty( TestBitmapWidth, TestBitmapHeight );

        for ( var y = 0; y < TestBitmapHeight; y++ )
        {
            for ( var x = 0; x < TestBitmapWidth; x++ )
            {
                bitmap.SetPixel( x, y, _testBitmapColor );
            }
        }

        return CashedBitmap.CreateAsync( bitmap );
    }
}