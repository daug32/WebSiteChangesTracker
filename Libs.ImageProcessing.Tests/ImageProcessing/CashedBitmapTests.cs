using System.Drawing;
using Libs.ImageProcessing.Creators;
using NUnit.Framework;
using Libs.ImageProcessing.Models;

namespace Libs.ImageProcessing.Tests.ImageProcessing;

[TestFixture]
[Parallelizable( ParallelScope.All )]
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
        Assert.DoesNotThrowAsync( async () => await CashedBitmapCreator.CreateAsync( BitmapCreator.CreateEmpty( BigWidth, BigHeight ) ) );
    }

    [Test]
    public void CreateAsync_InvalidArgument_ThrowsException()
    {
        Assert.ThrowsAsync<NullReferenceException>( () => CashedBitmapCreator.CreateAsync( ( Bitmap )null! ) );
    }

    [Test]
    public Task CreateAsync_CreateThreeCashedBitmapsForBigBitmaps_AsynchronousCreatingOfCashedBitmaps()
    {
        return Task.WhenAll( 
            CashedBitmapCreator.CreateAsync( BitmapCreator.CreateEmpty( BigWidth, BigHeight ) ),
            CashedBitmapCreator.CreateAsync( BitmapCreator.CreateEmpty( BigWidth, BigHeight ) ),
            CashedBitmapCreator.CreateAsync( BitmapCreator.CreateEmpty( BigWidth, BigHeight ) ) );
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
        Bitmap bitmap = BitmapCreator.CreateEmpty( TestBitmapWidth, TestBitmapHeight );

        for ( var y = 0; y < TestBitmapHeight; y++ )
        {
            for ( var x = 0; x < TestBitmapWidth; x++ )
            {
                bitmap.SetPixel( x, y, _testBitmapColor );
            }
        }

        return CashedBitmapCreator.CreateAsync( bitmap );
    }
}