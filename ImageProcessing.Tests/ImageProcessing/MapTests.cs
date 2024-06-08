using System.Drawing;
using NUnit.Framework;
using ImageProcessing.Models;

namespace ImageProcessing.Tests.ImageProcessing;

public class MapTests
{
    private static readonly object[] _validCtorData =
    {
        new object[] { 0, 0, new List<int>() },
        new object[] { 1, 1, new List<int>() { 1 } },
        new object[] { 2, 3, new List<int>() { 1, 2, 3, 4, 5, 6 } }
    };

    [TestCaseSource( nameof( _validCtorData ) )]
    public void Ctor_ValidData_DoesntThrow( int width, int height, List<int> data )
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.DoesNotThrow( () => new Map<int>( width, height, data ) );
    }

    private static readonly object[] _invalidCtorData =
    {
        new object[] { 1, -1, new List<int>() { 1 } },
        new object[] { 1, 1, null! },
        new object[] { 2, 3, new List<int>() { 1 } }
    };

    [TestCaseSource( nameof( _invalidCtorData ) )]
    public void Ctor_InvalidData_ThrowsArgumentException( int width, int height, List<int> data )
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws( 
            Is.InstanceOf<Exception>(), 
            () => new Map<int>( width, height, data ) );
    }

    [Test]
    public void Get_FromExistentPosition_ReturnsActualData()
    {
        // Arrange
        var data = new List<int>()
        {
            00, 01, 02,
            10, 11, 12
        };

        var map = new Map<int>( 3, 2, data );

        // Act
        Assert.That( map.Get( 0, 0 ), Is.EqualTo( 00 ) );
        Assert.That( map.Get( 1, 0 ), Is.EqualTo( 01 ) );
        Assert.That( map.Get( 2, 0 ), Is.EqualTo( 02 ) );

        Assert.That( map.Get( 0, 1 ), Is.EqualTo( 10 ) );
        Assert.That( map.Get( 1, 1 ), Is.EqualTo( 11 ) );
        Assert.That( map.Get( 2, 1 ), Is.EqualTo( 12 ) );
    }

    [Test]
    public void Get_FromNonexistentPosition_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var data = new List<int>()
        {
            00, 01, 02,
            11, 11, 12
        };

        var map = new Map<int>( 3, 2, data );

        // Act
        Assert.Throws<ArgumentOutOfRangeException>( () => map.Get( -1, 0 ) );
        Assert.Throws<ArgumentOutOfRangeException>( () => map.Get( 4, 0 ) );

        Assert.Throws<ArgumentOutOfRangeException>( () => map.Get( 0, -1 ) );
        Assert.Throws<ArgumentOutOfRangeException>( () => map.Get( 0, 3 ) );
    }

    [Test]
    public void Resize_ExtendWidthAndFillNewItemsByOnes_SuccessfullyExtended()
    {
        // Arrange
        var data = new List<int>() { 0, 0, 0, 0 };
        var map = new Map<int>( 2, 2, data );

        // Act
        map.Resize( new Size( 3, 2 ), 1 );

        // Assert
        Assert.That( map.Size, Is.EqualTo( new Size( 3, 2 ) ) );

        // old pixels
        Assert.That( map.Get( 0, 0 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 1, 0 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 0, 1 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 1, 1 ), Is.EqualTo( 0 ) );

        // new column
        Assert.That( map.Get( 2, 0 ), Is.EqualTo( 1 ) );
        Assert.That( map.Get( 2, 1 ), Is.EqualTo( 1 ) );
    }

    [Test]
    public void Resize_ExtendHeightAndFillNewItemsByOnes_SuccessfullyExtended()
    {
        // Arrange
        var data = new List<int>() { 0, 0, 0, 0 };
        var map = new Map<int>( 2, 2, data );

        // Act
        map.Resize( new Size( 2, 3 ), 1 );

        // Assert
        Assert.That( map.Size, Is.EqualTo( new Size( 2, 3 ) ) );

        // old pixels
        Assert.That( map.Get( 0, 0 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 1, 0 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 0, 1 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 1, 1 ), Is.EqualTo( 0 ) );

        // new row
        Assert.That( map.Get( 0, 2 ), Is.EqualTo( 1 ) );
        Assert.That( map.Get( 1, 2 ), Is.EqualTo( 1 ) );
    }

    [Test]
    public void Resize_ExtendEachSizeDirectionAndFillNewItemsByOnes_SuccessfullyExtended()
    {
        // Arrange
        var map = new Map<int>( 2, 2, new List<int>() { 0, 0, 0, 0 } );

        // Act
        map.Resize( new Size( 3, 3 ), 1 );

        // Assert
        Assert.That( map.Size, Is.EqualTo( new Size( 3, 3 ) ) );

        // old pixels
        Assert.That( map.Get( 0, 0 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 1, 0 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 0, 1 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 1, 1 ), Is.EqualTo( 0 ) );

        // new column
        Assert.That( map.Get( 2, 0 ), Is.EqualTo( 1 ) );
        Assert.That( map.Get( 2, 1 ), Is.EqualTo( 1 ) );

        // new row
        Assert.That( map.Get( 0, 2 ), Is.EqualTo( 1 ) );
        Assert.That( map.Get( 1, 2 ), Is.EqualTo( 1 ) );
        Assert.That( map.Get( 2, 2 ), Is.EqualTo( 1 ) );
    }

    [Test]
    public void Resize_CropWidth_SuccessfullyCropped()
    {
        // Arrange
        var map = new Map<int>( 2, 2, new List<int>() { 0, 0, 0, 0 } );

        // Act
        map.Resize( new Size( 1, 2 ), 0 );

        // Assert
        Assert.That( map.Size, Is.EqualTo( new Size( 1, 2 ) ) );
        Assert.That( map.Get( 0, 0 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 0, 1 ), Is.EqualTo( 0 ) );
    }

    [Test]
    public void Resize_CropHeight_SuccessfullyCropped()
    {
        // Arrange
        var map = new Map<int>( 2, 2, new List<int>() { 0, 0, 0, 0 } );

        // Act
        map.Resize( new Size( 2, 1 ), 0 );

        // Assert
        Assert.That( map.Size, Is.EqualTo( new Size( 2, 1 ) ) );
        Assert.That( map.Get( 0, 0 ), Is.EqualTo( 0 ) );
        Assert.That( map.Get( 1, 0 ), Is.EqualTo( 0 ) );
    }

    [Test]
    public void Resize_CropSizeAtEachDirection_SuccessfullyCropped()
    {
        // Arrange
        var map = new Map<int>( 2, 2, new List<int>() { 0, 0, 0, 0 } );

        // Act
        map.Resize( new Size( 1, 1 ), 0 );

        // Assert
        Assert.That( map.Size, Is.EqualTo( new Size( 1, 1 ) ) );
        Assert.That( map.Get( 0, 0 ), Is.EqualTo( 0 ) );
    }
}