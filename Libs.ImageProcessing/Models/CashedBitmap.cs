using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Libs.ImageProcessing.Implementation;
using Libs.ImageProcessing.Implementation.Utils;
using Libs.ImageProcessing.Implementation.Extensions;

namespace Libs.ImageProcessing.Models
{
    public class CashedBitmap : IDisposable
    {
        internal Bitmap SourceBitmap { get; private set; }
        private readonly Map<Color> _data;

        private bool _wasResized;
        private bool _hasChanges;

        public Size Size => _data.Size;

    #region Ctors
        
        public static Task<CashedBitmap> CreateAsync( string file )
        {
            return CreateAsync( BitmapBuilder.CreateFromFile( file ) );
        }

        // ReSharper disable once UnusedMember.Global
        public static CashedBitmap CreateEmpty( int width, int height )
        {
            var data = new List<Color>( width * height );
            for ( var i = 0; i < width * height; i++ )
            {
                data.Add( Color.White );
            }
            
            return new CashedBitmap( 
                new Bitmap( width, height, Constants.SupportedPixelFormat ),
                data );
        }

        public static async Task<CashedBitmap> CreateAsync( Bitmap bitmap )
        {
            ValidateBitmapOrThrow( bitmap );
            List<Color> data = await bitmap.GetPixelArrayAsync();
            return new CashedBitmap( bitmap, data );
        }

        private CashedBitmap( Bitmap sourceBitmap, List<Color> data )
        {
            SourceBitmap = sourceBitmap;
            _data = new Map<Color>( sourceBitmap.Width, sourceBitmap.Height, data );
        }

    #endregion
        
    #region Data access
    
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
        
    #endregion Data access
        
    #region Other

        public void Resize( Size newSize, Color? defaultColor = null )
        {
            if ( defaultColor == null )
            {
                defaultColor = Color.White;
            }

            _data.Resize( newSize, defaultColor.Value );
            _wasResized = true;
        }

        public void Save( string path, ImageFormat imageFormat = null )
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
        
    #endregion Other

    #region Private methods
        
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
        
    #endregion Private methods
    
    }
}