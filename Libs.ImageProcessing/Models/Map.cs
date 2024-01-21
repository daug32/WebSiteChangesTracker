using System;
using System.Collections.Generic;
using System.Drawing;

namespace Libs.ImageProcessing.Models;

public class Map<T>
{
    private T[] _data;
    public Size Size { get; private set; }

    public Map( 
        int width, 
        int height,
        List<T> data )
    {
        if ( data == null || data.Count != width * height )
        {
            throw new ArgumentException( nameof( data ) );
        }

        Size = new Size( width, height );
        _data = data.ToArray();
    }

    // ReSharper disable once UnusedMember.Global
    public T GetByOffset( int index )
    {
        if ( index < 0 || index >= _data.Length )
        {
            throw new ArgumentOutOfRangeException( nameof( index ) );
        }

        return _data[index];
    }

    public T Get( int x, int y ) 
    {
        ValidatePointOrThrow( x, y );

        return _data[y * Size.Width + x];
    }
        
    public void Set( int x, int y, T value )
    {
        ValidatePointOrThrow( x, y );

        _data[y * Size.Width + x] = value;
    }

    public void UpdateEach( Func<int, int, T, T> func )
    {
        for ( var y = 0; y < Size.Height; y++ )
        {
            int offset = y * Size.Width;
            for ( var x = 0; x < Size.Width; x++ )
            {
                _data[offset + x] = func( x, y, _data[offset + x] );
            }
        }
    }
        
    public void ForEach( Action<int, int, T> func )
    {
        for ( var y = 0; y < Size.Height; y++ )
        {
            int offset = y * Size.Width;
            for ( var x = 0; x < Size.Width; x++ )
            {
                func( x, y, _data[offset + x] );
            }
        }
    }

    public bool ContainsPoint( int x, int y )
    {
        return
            x >= 0 &&
            x < Size.Width &&
            y >= 0 &&
            y < Size.Height;
    }

    public void Resize( Size newSize, T defaultValue = default )
    {
        if ( newSize.Width < 0 || newSize.Height < 0 )
        {
            throw new ArgumentException( "Each size of new direction should not be less than 0" );
        }
            
        if ( newSize.Width == Size.Width && newSize.Height == Size.Height )
        {
            return;
        }
            
        var newData = new T[newSize.Width * newSize.Height];

        for ( var y = 0; y < newSize.Height; y++ )
        {
            int offset = y * newSize.Width;
            for ( var x = 0; x < newSize.Width; x++ )
            {
                T item = ContainsPoint( x, y )
                    ? _data[y * Size.Width + x]
                    : defaultValue;
                newData[offset + x] = item;
            }
        }

        Size = newSize;
        _data = newData;
    }

    private void ValidatePointOrThrow( int x, int y )
    {
        ThrowIfOutOfRange( x, 0, Size.Width );
        ThrowIfOutOfRange( y, 0, Size.Height );
    }

    private static void ThrowIfOutOfRange( int a, int min, int max )
    {
        if ( a < min || a >= max )
        {
            throw new ArgumentOutOfRangeException();
        }
    }
}