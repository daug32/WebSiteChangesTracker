﻿using System.Drawing;
using ImageProcessing.Creators;

namespace ImageProcessing.Implementation.Utils;

internal static class BitmapHelper
{
    public static Bitmap Resize( Image bitmap, Size newSize )
    {
        Bitmap clone = BitmapCreator.CreateEmpty( newSize.Width, newSize.Height );

        using ( Graphics graphics = Graphics.FromImage( clone ) )
        {
            var fullScreenRectangle = new Rectangle(
                Point.Empty,
                newSize );

            graphics.DrawImage( bitmap, fullScreenRectangle );
        }

        return clone;
    }

    public static Bitmap ConvertBitmapTo24RgbFormat( Image bitmap )
    {
        int height = bitmap.Height;
        int width = bitmap.Width;

        Bitmap clone = BitmapCreator.CreateEmpty( width, height );

        using ( Graphics graphics = Graphics.FromImage( clone ) )
        {
            var fullScreenRectangle = new Rectangle(
                0,
                0,
                width,
                height );

            graphics.DrawImage( bitmap, fullScreenRectangle );
        }

        return clone;
    }
}