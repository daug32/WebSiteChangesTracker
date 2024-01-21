using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Libs.ImageProcessing.Implementation;
using Libs.ImageProcessing.Implementation.Utils;

namespace Libs.ImageProcessing.Models;

public class CashedBitmap : IDisposable
{
    internal Bitmap SourceBitmap { get; private set; }
    private readonly Map<Color> _data;

    private bool _wasResized;
    private bool _hasChanges;

    public Size Size => _data.Size;

    internal CashedBitmap( Bitmap sourceBitmap, ICollection<Color> data )
    {
        ValidateBitmapOrThrow( sourceBitmap );
        SourceBitmap = sourceBitmap;
        _data = new Map<Color>( sourceBitmap.Width, sourceBitmap.Height, data );
    }

    public Color GetPixel( int x, int y )
    {
        return _data.Get( x, y );
    }

    // ReSharper disable once UnusedMember.Global
    public void SetPixel( int x, int y, Color value )
    {
        _hasChanges = true;
        _data.Set( x, y, value );
    }

    public void UpdateEach( Func<int, int, Color, Color> func )
    {
        _data.UpdateEach( func );
        _hasChanges = true;
    }
        
    // ReSharper disable once UnusedMember.Global
    public void ForEach( Action<int, int, Color> func )
    {
        _data.ForEach( func );   
    }

    public void Resize( Size newSize, Color? defaultColor = null )
    {
        if ( defaultColor == null )
        {
            defaultColor = Color.White;
        }

        _data.Resize( newSize, defaultColor.Value );
        _wasResized = true;
    }

    public void Save( string path, ImageFormat? imageFormat = null )
    {
        CommitChangesIfNeed();

        if ( imageFormat == null )
        {
            imageFormat = ImageFormat.Png;
        }

        string extension = $".{imageFormat.ToString().ToLower()}";
        if ( !path.EndsWith( extension ) )
        {
            path += extension;
        }
            
        string directory = Path.GetDirectoryName( path ) ?? String.Empty;
        if ( !Directory.Exists( directory ) )
        {
            Directory.CreateDirectory( directory );
        } 
            
        SourceBitmap.Save( path, imageFormat );
    }

    public bool ContainsPoint( int x, int y )
    {
        return _data.ContainsPoint( x, y );
    }

    public void Dispose()
    {
        SourceBitmap.Dispose();
    }
        
    internal void CommitChangesIfNeed()
    {
        if ( _wasResized )
        {
            SourceBitmap = BitmapHelper.Resize( SourceBitmap, Size );
            _wasResized = false;
        }
                
        // ReSharper disable once InvertIf
        if ( _hasChanges )
        {
            _data.ForEach( SourceBitmap.SetPixel );
            _hasChanges = false;
        }
    }

    private static void ValidateBitmapOrThrow( Image bitmap )
    {
        if ( bitmap.PixelFormat != Constants.SupportedPixelFormat )
        {
            throw new BadImageFormatException();
        }
    }
}