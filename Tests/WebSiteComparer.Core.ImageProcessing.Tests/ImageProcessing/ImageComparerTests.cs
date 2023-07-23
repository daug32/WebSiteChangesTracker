using System.Drawing;
using System.Drawing.Imaging;
using NUnit.Framework;
using WebSiteComparer.Core.ImageProcessing.Implementation.Comparing;
using WebSiteComparer.Core.ImageProcessing.Models;
using WebSiteComparer.Core.ImageProcessing.Tests.Utils;

namespace WebSiteComparer.Core.ImageProcessing.Tests.ImageProcessing;

public class ImageComparerTests
{
    private IImageComparer _imageComparer = null!;

    [SetUp]
    public void SetUp()
    {
        _imageComparer = new CashedBitmapComparer();
    }

    [Test]
    public async Task CompareAsync_CompareImagesWithSameSizeThatAreNotDifferent_ComparingResultHasNoChanges()
    {
        // Arrange
        CashedBitmap firstImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White }
            } );

        CashedBitmap secondImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White }
            } );

        // Act
        ImageComparingResult compareResult = await _imageComparer.CompareAsync( firstImage, secondImage );

        // Assert
        Assert.IsFalse( compareResult.HasChanges );
    }

    [Test]
    public async Task
        CompareAsync_CompareImagesWithSameSizeThatAreDifferentAtOnePoint_ComparingResultContainsThatPoint()
    {
        // Arrange
        CashedBitmap firstImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White }
            } );

        CashedBitmap secondImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.Black, Color.White },
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White }
            } );

        // Act
        ImageComparingResult compareResult = await _imageComparer.CompareAsync( firstImage, secondImage );

        // Assert
        Assert.IsTrue( compareResult.HasChanges );
        Assert.AreEqual( 1, compareResult.ChangesNumber );

        Assert.IsTrue( compareResult.Bitmap.GetPixel( 1, 1 ) != Color.White );
    }

    [Test]
    public async Task
        CompareAsync_CompareImagesWithSameSizeThatAreDifferentAtManyPoints_ComparingResultContainsThosePoint()
    {
        // Arrange
        CashedBitmap firstImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White }
            } );

        CashedBitmap secondImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.Black, Color.White },
                new[] { Color.Black, Color.White, Color.Black },
                new[] { Color.White, Color.White, Color.White }
            } );

        // Act
        ImageComparingResult compareResult = await _imageComparer.CompareAsync( firstImage, secondImage );

        // Assert
        Assert.IsTrue( compareResult.HasChanges );
        Assert.AreEqual( 3, compareResult.ChangesNumber );

        Assert.IsTrue( compareResult.Bitmap.GetPixel( 1, 1 ) != Color.White );
        Assert.IsTrue( compareResult.Bitmap.GetPixel( 0, 2 ) != Color.White );
        Assert.IsTrue( compareResult.Bitmap.GetPixel( 2, 2 ) != Color.White );
    }

    [Test]
    public async Task
        CompareAsync_CompareTwoWhiteImagesButFirstImageHasLesserWidthThanTheSecondOne_ComparingResultContainsAllPointsOutsideTheMinimumMatrix()
    {
        // Arrange
        CashedBitmap firstImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White },
                new[] { Color.White, Color.White }
            } );

        CashedBitmap secondImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White }
            } );

        // Act
        ImageComparingResult comparingResult = await _imageComparer.CompareAsync( firstImage, secondImage );

        // Assert
        Assert.IsTrue( comparingResult.HasChanges );

        Assert.AreEqual( 2, comparingResult.ChangesNumber );
        Assert.IsTrue( comparingResult.Bitmap.GetPixel( 2, 0 ) != Color.White );
        Assert.IsTrue( comparingResult.Bitmap.GetPixel( 2, 1 ) != Color.White );
    }

    [Test]
    public async Task
        CompareAsync_CompareTwoWhiteImagesButSecondImageHasLesserWidthThanTheFirstOne_ComparingResultContainsAllPointsOutsideTheMinimumMatrix()
    {
        // Arrange
        CashedBitmap firstImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White, Color.White },
                new[] { Color.White, Color.White, Color.White }
            } );

        CashedBitmap secondImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White },
                new[] { Color.White, Color.White }
            } );

        // Act
        ImageComparingResult comparingResult = await _imageComparer.CompareAsync( firstImage, secondImage );

        // Assert
        Assert.IsTrue( comparingResult.HasChanges );

        Assert.AreEqual( 2, comparingResult.ChangesNumber );
        Assert.IsTrue( comparingResult.Bitmap.GetPixel( 2, 0 ) != Color.White );
        Assert.IsTrue( comparingResult.Bitmap.GetPixel( 2, 1 ) != Color.White );
    }

    [Test]
    public async Task
        CompareAsync_CompareTwoWhiteImagesButFirstImageHasLesserHeightThanTheSecondOne_ComparingResultContainsAllPointsOutsideTheMinimumMatrix()
    {
        // Arrange
        CashedBitmap firstImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White },
                new[] { Color.White, Color.White }
            } );

        CashedBitmap secondImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White },
                new[] { Color.White, Color.White },
                new[] { Color.White, Color.White }
            } );

        // Act
        ImageComparingResult comparingResult = await _imageComparer.CompareAsync( firstImage, secondImage );

        // Assert
        Assert.IsTrue( comparingResult.HasChanges );

        Assert.AreEqual( 2, comparingResult.ChangesNumber );
        Assert.IsTrue( comparingResult.Bitmap.GetPixel( 0, 2 ) != Color.White );
        Assert.IsTrue( comparingResult.Bitmap.GetPixel( 1, 2 ) != Color.White );
    }

    [Test]
    public async Task
        CompareAsync_CompareTwoWhiteImagesButSecondImageHasLesserHeightThanTheFirstOne_ComparingResultContainsAllPointsOutsideTheMinimumMatrix()
    {
        // Arrange
        CashedBitmap firstImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White },
                new[] { Color.White, Color.White },
                new[] { Color.White, Color.White }
            } );

        CashedBitmap secondImage = await BitmapHelper.CreateCashedBitmapByColorMatrix(
            new[]
            {
                new[] { Color.White, Color.White },
                new[] { Color.White, Color.White }
            } );

        // Act
        ImageComparingResult comparingResult = await _imageComparer.CompareAsync( firstImage, secondImage );

        // Assert
        Assert.IsTrue( comparingResult.HasChanges );

        Assert.AreEqual( 2, comparingResult.ChangesNumber );
        Assert.IsTrue( comparingResult.Bitmap.GetPixel( 0, 2 ) != Color.White );
        Assert.IsTrue( comparingResult.Bitmap.GetPixel( 1, 2 ) != Color.White );
    }

    [Test]
    public async Task CompareAsync_CompareRealImageToItself_ComparingResultHasNoChanges()
    {
        // Arrange
        string firstImage = TestImagesPathHelper.BuildInputPathDirectory( "CurlyBracket1.jpg" );
        string fileToSave = TestImagesPathHelper.BuildOutputPathDirectory(
            nameof( CompareAsync_CompareRealImageToItself_ComparingResultHasNoChanges ) );

        // Act
        ImageComparingResult compareResult = await _imageComparer.CompareAsync( firstImage, firstImage );

        // Assert
        Assert.IsFalse( compareResult.HasChanges );
        compareResult.Bitmap.Save( fileToSave, ImageFormat.Jpeg );
    }

    [Test]
    public async Task CompareAsync_CompareTwoRealImages_ComparingResultHasChanges()
    {
        // Arrange
        string firstImage = TestImagesPathHelper.BuildInputPathDirectory( "CurlyBracket1.jpg" );
        string secondImage = TestImagesPathHelper.BuildInputPathDirectory( "CurlyBracket2.jpg" );
        string fileToSave = TestImagesPathHelper.BuildOutputPathDirectory(
            nameof( CompareAsync_CompareTwoRealImages_ComparingResultHasChanges ) );

        // Act
        ImageComparingResult compareResult = await _imageComparer.CompareAsync( firstImage, secondImage );

        // Assert
        Assert.IsTrue( compareResult.HasChanges );
        compareResult.Bitmap.Save( fileToSave, ImageFormat.Jpeg );
    }

    [Test]
    public async Task CompareAsync_CompareTwoBigRealImages_ComparingResultHasChanges()
    {
        // Arrange
        string firstImage = TestImagesPathHelper.BuildInputPathDirectory( "Dishonored1.png" );
        string secondImage = TestImagesPathHelper.BuildInputPathDirectory( "Dishonored2.png" );
        string fileToSave = TestImagesPathHelper.BuildOutputPathDirectory(
            nameof( CompareAsync_CompareTwoBigRealImages_ComparingResultHasChanges ) );

        // Act
        ImageComparingResult compareResult = await _imageComparer.CompareAsync( firstImage, secondImage );

        // Assert
        Assert.IsTrue( compareResult.HasChanges );
        Assert.IsTrue( compareResult.PercentOfChanges < 2 );
        compareResult.Bitmap.Save( fileToSave, ImageFormat.Jpeg );
    }
}